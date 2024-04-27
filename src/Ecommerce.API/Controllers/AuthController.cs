using AutoMapper;
using Ecommerce.API.DTOs;
using Ecommerce.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private SignInManager<IdentityUser> _signInManager { get; set; }
        private UserManager<IdentityUser> _userManager { get; set; }

        public AuthController(SignInManager<IdentityUser> signInManager
                              ,UserManager<IdentityUser> userManager
                              ,IMapper mapper 
                              ,INotificator notificator)
            : base(mapper, notificator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
                return CustomResponse(registerUser);
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

            return CustomResponse(loginUser);
        }
    }
}
