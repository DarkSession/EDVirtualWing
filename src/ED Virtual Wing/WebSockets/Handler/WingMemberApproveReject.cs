using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingMemberApproveReject : WebSocketHandler
    {
        class WingMemberApproveRejectData
        {
            public string WingId { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public bool Approve { get; set; }
        }

        protected override Type? MessageDataType { get; } = typeof(WingMemberApproveRejectData);


        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingMemberApproveRejectData? data = message.Data?.ToObject<WingMemberApproveRejectData>();
            if (data != null && Guid.TryParse(data.WingId, out Guid wingId) && await applicationDbContext.Wings.AnyAsync(w => w.WingId == wingId && w.Owner == user && w.Status == WingStatus.Active))
            {
                WingMember? wingMember = await applicationDbContext.WingMembers
                    .Include(w => w.User)
                    .Include(w => w.Wing)
                    .FirstOrDefaultAsync(w => w.Wing!.WingId == wingId && w.User!.Id == data.UserId && w.Status == WingMembershipStatus.PendingApproval);
                if (wingMember != null && wingMember.User != user)
                {
                    if (data.Approve)
                    {
                        wingMember.Status = WingMembershipStatus.Joined;
                    }
                    else
                    {
                        wingMember.Status = WingMembershipStatus.Banned;
                    }
                    return new WebSocketHandlerResultSuccess();
                }
            }
            return new WebSocketHandlerResultError();
        }
    }
}
