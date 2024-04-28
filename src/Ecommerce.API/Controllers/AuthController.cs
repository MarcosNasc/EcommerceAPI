using System.IdentityModel.Tokens.Jwt;
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
                return Ok(GenerateJWT());
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

            return Ok(GenerateJWT());
        }

        private string GenerateJWT()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidIn,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }
    }
}
