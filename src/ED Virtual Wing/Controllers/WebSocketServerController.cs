using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ED_Virtual_Wing.Controllers
{
    public class WebSocketController : ControllerBase
    {
        private UserManager<ApplicationUser> UserManager { get; }
        private ApplicationDbContext DatabaseContext { get; }

        public WebSocketController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            UserManager = userManager;
            DatabaseContext = applicationDbContext;
        }

        public async Task Get()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                // BadRequest();
            }
        }
    }
}
