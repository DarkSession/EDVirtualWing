using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.WebSockets.Messages;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingLeaveDisband : WebSocketHandler
    {
        class WinLeaveDisbandData
        {
            public string WingId { get; set; } = string.Empty;
        }

        protected override Type? MessageDataType { get; } = typeof(WinLeaveDisbandData);
        private WebSocketServer WebSocketServer { get; }

        public WingLeaveDisband (WebSocketServer webSocketServer)
        {
            WebSocketServer = webSocketServer;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WinLeaveDisbandData? data = message.Data?.ToObject<WinLeaveDisbandData>();
            if (data != null && Guid.TryParse(data.WingId, out Guid wingId))
            {
                Wing? wing = await applicationDbContext.Wings
                    .Include(w => w.Owner)
                    .FirstOrDefaultAsync(w => w.WingId == wingId && w.Status == WingStatus.Active);
                if (wing != null)
                {
                    if (wing.Owner == user)
                    {
                        if (wing.Status == WingStatus.Active)
                        {
                            wing.Status = WingStatus.Deleted;
                            IEnumerable<WebSocketSession> webSocketSessionsWithWingActive = WebSocketServer.ActiveSessions
                                .Where(w => w.ActiveWing == wing);
                            WebSocketMessage wingUnsubscribed = new("WingUnsubscribed", new WingUnsubscribedData(wing.WingId));
                            foreach (WebSocketSession webSocketSessionWithWingActive in webSocketSessionsWithWingActive)
                            {
                                await wingUnsubscribed.Send(webSocketSessionWithWingActive);
                            }
                            return new WebSocketHandlerResultSuccess();
                        }
                        return new WebSocketHandlerResultError("This wing is already deleted.");
                    }
                    WingMember? wingMember = await applicationDbContext.WingMembers
                        .FirstOrDefaultAsync(w => w.User == user && w.Wing == wing && w.Status == WingMembershipStatus.Joined);
                    if (wingMember != null)
                    {
                        wingMember.Status = WingMembershipStatus.Left;
                        IEnumerable<WebSocketSession> webSocketSessionsWithWingActive = WebSocketServer.ActiveSessions
                            .Where(w => w.ActiveWing == wing && w.User == user);
                        WebSocketMessage wingUnsubscribed = new("WingUnsubscribed", new WingUnsubscribedData(wing.WingId));
                        foreach (WebSocketSession webSocketSessionWithWingActive in webSocketSessionsWithWingActive)
                        {
                            await wingUnsubscribed.Send(webSocketSessionWithWingActive);
                        }
                        return new WebSocketHandlerResultSuccess();
                    }
                    return new WebSocketHandlerResultError("You are not a member of this wing");
                }
            }
            return new WebSocketHandlerResultError("Wing not found");
        }
    }
}
