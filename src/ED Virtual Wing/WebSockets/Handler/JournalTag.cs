using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.WebSockets.Messages;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class JournalTag : WebSocketHandler
    {
        protected override Type? MessageDataType { get; }
        private WebSocketServer WebSocketServer { get; }
        public JournalTag(WebSocketServer webSocketServer)
        {
            WebSocketServer = webSocketServer;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            Commander commander = await user.GetCommander(applicationDbContext);
            commander.LastActivity = DateTimeOffset.Now;
            await commander.DistributeCommanderData(WebSocketServer, applicationDbContext);
            return new WebSocketHandlerResultSuccess();
        }
    }
}
