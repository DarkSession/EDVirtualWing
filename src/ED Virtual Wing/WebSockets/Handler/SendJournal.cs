using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class SendJournal : WebSocketHandler
    {
        public override ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessage message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            return ValueTask.FromResult<WebSocketHandlerResult>(new WebSocketHandlerResultError());
        }
    }
}
