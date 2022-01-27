using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingsGet : WebSocketHandler
    {
        class WingsGetResponse
        {
            public List<WingDetail> Wings { get; set; }

            public WingsGetResponse(List<WingDetail> wings)
            {
                Wings = wings;
            }
        }

        class WingDetail : Wing
        {
            public int MemberCount { get; set; }
            public int MemberOnline { get; set; }
        }

        protected override Type? MessageDataType { get; }
        private WebSocketServer WebSocketServer { get; }
        public WingsGet(WebSocketServer webSocketServer)
        {
            WebSocketServer = webSocketServer;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            List<ApplicationUser> onlineUsers = WebSocketServer.ActiveSessions
                .Where(a => a.StreamingJournal)
                .Select(a => a.User)
                .Distinct()
                .ToList();
            var wings = await applicationDbContext.Wings
                .Where(w => w.Status == WingStatus.Active && w.Members!.Any(m => m.User == user && m.Status == WingMembershipStatus.Joined))
                .Select(w =>
                new
                {
                    Wing = w,
                    MemberCount = w.Members!.Count(),
                    MemberOnline = w.Members!.Count(m => m.Status == WingMembershipStatus.Joined && onlineUsers.Any(o => o == m.User)),
                })
                .ToListAsync();
            List<WingDetail> result = new();
            foreach (var wing in wings)
            {
                result.Add(new WingDetail()
                {
                    WingId = wing.Wing.WingId,
                    Name = wing.Wing.Name,
                    MemberCount = wing.MemberCount,
                    MemberOnline = wing.MemberOnline,
                });
            }
            return new WebSocketHandlerResultSuccess(new WingsGetResponse(result));
        }
    }
}
