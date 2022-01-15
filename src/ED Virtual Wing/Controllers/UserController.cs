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

        private SignInManager<ApplicationUser> SignInManager { get; }

        public UserController(SignInManager<ApplicationUser> signInManager, ApplicationDbContext applicationDbContext)
        {
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
    }
}
