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
        private ApplicationDbContext ApplicationDbContext { get; }
        //private FDevApi.FDevApi FDevApi { get; }

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext
            // FDevApi.FDevApi fdevApi
            )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            ApplicationDbContext = applicationDbContext;
            // FDevApi = fdevApi;
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
                await user.GetCommander(ApplicationDbContext);
                await ApplicationDbContext.SaveChangesAsync();
                await SignInManager.SignInAsync(user, true);
                return new RegistrationResponse(true);
            }
            return new RegistrationResponse(result.Errors.Select(e => e.Description).ToList());
        }
        /*
        public class FDevAuthRequest
        {
            [Required]
            public string State { get; set; } = string.Empty;
            [Required]
            public string Code { get; set; } = string.Empty;
        }

        [HttpPost("fdev-auth")]
        public async Task<ActionResult<RegistrationResponse>> FDevAuth(FDevAuthRequest requestData)
        {
            FDevApiAuthCode? fdevApiAuthCode = await ApplicationDbContext.FDevApiAuthCodes.FirstOrDefaultAsync(f => f.State == requestData.State);
            if (fdevApiAuthCode != null)
            {
                ApplicationDbContext.FDevApiAuthCodes.Remove(fdevApiAuthCode);
                FDevApiResult? fdevApiResult = await FDevApi.AuthenticateUser(requestData.Code, fdevApiAuthCode.Code);
                if (fdevApiResult != null)
                {
                    ApplicationUser? user = await ApplicationDbContext.Users.FirstOrDefaultAsync(u => u.FDevCustomerId == fdevApiResult.CustomerId);
                    if (user == null)
                    {
                        string userName = $"{Functions.GenerateString(8)}-{fdevApiResult.CustomerId}";
                        user = new()
                        {
                            UserName = userName,
                            Email = $"{userName}@vw.edct.dev",
                            FDevCustomerId = fdevApiResult.CustomerId,
                        };
                        IdentityResult result = await UserManager.CreateAsync(user, Functions.GenerateString(32));
                        if (result.Succeeded)
                        {
                            await user.GetCommander(ApplicationDbContext);
                            await ApplicationDbContext.SaveChangesAsync();
                            await SignInManager.SignInAsync(user, true);
                            return new RegistrationResponse(true);
                        }
                        return new RegistrationResponse(new List<string>() { "Registration could not be completed." });
                    }
                    else
                    {
                        await SignInManager.SignInAsync(user, true);
                        return new RegistrationResponse(true);
                    }
                }
            }
            return new RegistrationResponse(new List<string>() { "Authentication failed!" });
        }

        public class FDevGetStateResponse
        {
            public string Url { get; }
            public FDevGetStateResponse(string url)
            {
                Url = url;
            }
        }

        [HttpPost("fdev-get-state")]
        public async Task<ActionResult<FDevGetStateResponse>> FDevGetState()
        {
            string url = FDevApi.CreateAuthorizeUrl(ApplicationDbContext);
            await ApplicationDbContext.SaveChangesAsync();
            return new FDevGetStateResponse(url);
        }
        */
    }
}
