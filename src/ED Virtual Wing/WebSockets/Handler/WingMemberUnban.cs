using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingMemberUnban : WebSocketHandler
    {
        class WingMemberUnbanData
        {
            public string WingId { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
        }

        protected override Type? MessageDataType { get; } = typeof(WingMemberUnbanData);

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingMemberUnbanData? data = message.Data?.ToObject<WingMemberUnbanData>();
            if (data != null && Guid.TryParse(data.WingId, out Guid wingId) && await applicationDbContext.Wings.AnyAsync(w => w.WingId == wingId && w.Owner == user && w.Status == WingStatus.Active))
            {
                WingMember? wingMember = await applicationDbContext.WingMembers
                    .Include(w => w.User)
                    .Include(w => w.Wing)
                    .FirstOrDefaultAsync(w => w.Wing!.WingId == wingId && w.User!.Id == data.UserId && w.Status == WingMembershipStatus.Banned);
                if (wingMember != null && wingMember.User != user)
                {
                    wingMember.Status = WingMembershipStatus.Left;
                    return new WebSocketHandlerResultSuccess();
                }
            }
            return new WebSocketHandlerResultError();
        }
    }
}
