using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ED_Virtual_Wing.Controllers
{
    [ApiController]
    [Route("user")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        public class LoginRequest
        {
            [Required]
            public string? UserName { get; set; }
            [Required]
            public string? Password { get; set; }
        }

        public class LoginResponse
        {
            public bool Success { get; set; }
            public LoginResponse(bool success)
            {
                Success = success;
            }
        }

        private UserManager<ApplicationUser> UserManager { get; }
        private SignInManager<ApplicationUser> SignInManager { get; }

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext applicationDbContext)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await SignInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, true, false);
            return new LoginResponse(signInResult.Succeeded);
        }

        public class RegistrationRequest
        {
            [Required]
            public string? UserName { get; set; }
            [Required]
            public string? Password { get; set; }
            [Required]
            [EmailAddress]
            public string? Email { get; set; }
        }

        public class RegistrationResponse
        {
            public bool Success { get; }
            public List<string>? Error { get; }
            public RegistrationResponse(bool success)
            {
                Success = success;
            }

            public RegistrationResponse(List<string> errors) : this(false)
            {
                Error = errors;
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest registrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            ApplicationUser user = new()
            {
                UserName = registrationRequest.UserName,
                Email = registrationRequest.Email,
            };
            IdentityResult result = await UserManager.CreateAsync(user, registrationRequest.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, true);
                return new RegistrationResponse(true);
            }
            return new RegistrationResponse(result.Errors.Select(e => e.Description).ToList());
        }
    }
}
