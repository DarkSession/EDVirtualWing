using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ED_Virtual_Wing.Controllers
{
    [AllowAnonymous]
    [Route("ws")]
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
                throw new Exception("!IsWebSocketRequest");
            }
            var user = HttpContext.User;
            var um = UserManager;
            await WebSocketServer.ProcessRequest(HttpContext, um);
        }
    }
}
