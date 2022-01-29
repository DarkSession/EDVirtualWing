using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.WebSockets.Messages;

namespace ED_Virtual_Wing.WebSockets.Actions
{
    [WebSocketAction(WebSocketAction.OnUserDisconnected)]
    public class OnUserDisconnected : IWebSocketAction
    {
        private ApplicationDbContext ApplicationDbContext { get; }
        private WebSocketServer WebSocketServer { get; }

        public OnUserDisconnected(ApplicationDbContext applicationDbContext, WebSocketServer webSocketServer)
        {
            ApplicationDbContext = applicationDbContext;
            WebSocketServer = webSocketServer;
        }

        public async ValueTask Process(WebSocketSession webSocketSession)
        {
            if (webSocketSession.StreamingJournal)
            {
                ApplicationUser? user = await ApplicationDbContext.Users.FindAsync(webSocketSession.User.Id);
                if (user != null)
                {
                    Commander commander = await user.GetCommander(ApplicationDbContext);
                    commander.LastEventDate = null;
                    await ApplicationDbContext.SaveChangesAsync();
                    await commander.OtherCommanderWsInstancesNotifyStreaming(WebSocketServer, false);
                    await commander.DistributeCommanderData(WebSocketServer, ApplicationDbContext);
                }
            }
        }
    }
}
