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
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private WebSocketServer WebSocketServer { get; }

        public WebSocketController(UserManager<ApplicationUser> userManager, IServiceScopeFactory serviceScopeFactory, WebSocketServer webSocketServer)
        {
            UserManager = userManager;
            ServiceScopeFactory = serviceScopeFactory;
            WebSocketServer = webSocketServer;
        }

        public async Task Get()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                // BadRequest();
                throw new Exception("!IsWebSocketRequest");
            }
            ApplicationUser? applicationUser = null;
            if (User != null)
            {
                applicationUser = await UserManager.GetUserAsync(User);
            }
            await WebSocketServer.ProcessRequest(HttpContext, applicationUser, ServiceScopeFactory);
        }
    }
}
