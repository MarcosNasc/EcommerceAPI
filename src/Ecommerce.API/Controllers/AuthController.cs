using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Ecommerce.API.DTOs;
using Ecommerce.API.Extensions;
using Ecommerce.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private SignInManager<IdentityUser> _signInManager { get; set; }
        private UserManager<IdentityUser> _userManager { get; set; }
        private readonly AppSettings _appSettings;
        public AuthController(SignInManager<IdentityUser> signInManager
                              ,UserManager<IdentityUser> userManager
                              ,IMapper mapper 
                              ,INotificator notificator
                              ,IOptions<AppSettings> appSettings
                              )
            : base(mapper, notificator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user,registerUser.Password);

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return CustomResponse(await GenerateJWT(user.Email));
            }

            foreach(var error in result.Errors)
            {
                NotifyError(error.Description);
            }


            return CustomResponse(registerUser);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            if(!ModelState.IsValid) return CustomResponse(loginUser);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email , loginUser.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                NotifyError(Resources.Validations.LockoutUserLogin);
            }

            if(!result.Succeeded)
            {
                NotifyError(Resources.Validations.FailedUserLogin);
                return CustomResponse(loginUser);
            }

            return CustomResponse(await GenerateJWT(loginUser.Email));
        }

        private async Task<LoginResponseDTO> GenerateJWT(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            var defaultClaims = new string[] { "sub", "email", "jti", "nbf", "iat" };
            var response = new LoginResponseDTO
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserTokenDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Where(c => !defaultClaims.Contains(c.Type))
                        .Select(c => new ClaimDTO {Type = c.Type , Value = c.Value})
                }
            };
            
            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
        {
           return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }
    }
}
