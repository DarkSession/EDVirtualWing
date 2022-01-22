using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingsGet : WebSocketHandler
    {
        class WingsGetResponse
        {
            public List<Wing> Wings { get; set; }

            public WingsGetResponse(List<Wing> wings)
            {
                Wings = wings;
            }
        }

        protected override Type? MessageDataType { get; } = null;

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            return new WebSocketHandlerResultSuccess(new WingsGetResponse(await user.GetWings(applicationDbContext)));
        }
    }
}
