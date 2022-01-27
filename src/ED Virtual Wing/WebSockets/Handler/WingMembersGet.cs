using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingMembersGet : WebSocketHandler
    {
        class WingMembersGetData
        {
            public string WingId { get; set; } = string.Empty;
        }

        protected override Type? MessageDataType { get; } = typeof(WingMembersGetData);

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingMembersGetData? data = message.Data?.ToObject<WingMembersGetData>();
            if (data != null && Guid.TryParse(data.WingId, out Guid wingId) && await applicationDbContext.Wings.AnyAsync(w => w.WingId == wingId && w.Owner == user && w.Status == WingStatus.Active))
            {
                List<WingMember> wingMembers = await applicationDbContext.WingMembers
                    .AsNoTracking()
                    .Include(w => w.User)
                    .Include(w => w.User!.Commander)
                    .Where(w => w.Wing!.WingId == wingId && w.Status == WingMembershipStatus.Joined)
                    .ToListAsync();
            }
        }
    }
}
