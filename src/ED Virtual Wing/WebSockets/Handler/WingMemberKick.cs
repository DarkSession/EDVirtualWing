using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.WebSockets.Messages;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingMemberKick : WebSocketHandler
    {
        class WingMemberKickData
        {
            public string WingId { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
        }

        protected override Type? MessageDataType { get; } = typeof(WingMemberKickData);
        private WebSocketServer WebSocketServer { get; }

        public WingMemberKick(WebSocketServer webSocketServer)
        {
            WebSocketServer = webSocketServer;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingMemberKickData? data = message.Data?.ToObject<WingMemberKickData>();
            if (data != null && Guid.TryParse(data.WingId, out Guid wingId) && await applicationDbContext.Wings.AnyAsync(w => w.WingId == wingId && w.Owner == user && w.Status == WingStatus.Active))
            {
                WingMember? wingMember = await applicationDbContext.WingMembers
                    .Include(w => w.User)
                    .Include(w => w.Wing)
                    .FirstOrDefaultAsync(w => w.Wing!.WingId == wingId && w.User!.Id == data.UserId);
                if (wingMember != null && wingMember.Status == WingMembershipStatus.Joined && wingMember.User != user)
                {
                    wingMember.Status = WingMembershipStatus.Banned;
                    IEnumerable<WebSocketSession> webSocketSessionsWithWingActive = WebSocketServer.ActiveSessions
                            .Where(w => w.ActiveWing == wingMember.Wing && w.User == wingMember.User);
                    WebSocketMessage wingUnsubscribed = new("WingUnsubscribed", new WingUnsubscribedData(wingMember.Wing!.WingId));
                    foreach (WebSocketSession webSocketSessionWithWingActive in webSocketSessionsWithWingActive)
                    {
                        await wingUnsubscribed.Send(webSocketSessionWithWingActive);
                    }
                    return new WebSocketHandlerResultSuccess();
                }
            }
            return new WebSocketHandlerResultError();
        }
    }
}
