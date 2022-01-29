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

        class WingMembersGetResponseData
        {
            public List<WingMemberData> WingMembers { get; set; }
            public WingMembersGetResponseData(List<WingMemberData> wingMembers)
            {
                WingMembers = wingMembers;
            }
        }

        class WingMemberData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public DateTimeOffset Joined { get; set; }
            public bool CanModify { get; set; }
            public WingMembershipStatus Status { get; set; }

            public WingMemberData(WingMember wingMember, bool canModify)
            {
                Id = wingMember.User!.Id ?? string.Empty;
                if (!string.IsNullOrEmpty(wingMember.User!.Commander?.Name))
                {
                    Name = $"CMDR {wingMember.User!.Commander?.Name} ({wingMember.User!.UserName})";
                }
                else
                {
                    Name = wingMember.User!.UserName;
                }
                Joined = wingMember.Joined;
                CanModify = canModify;
                Status = wingMember.Status;
            }
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
                    .Where(w => w.Wing!.WingId == wingId && w.Status != WingMembershipStatus.Left)
                    .ToListAsync();
                return new WebSocketHandlerResultSuccess(
                    new WingMembersGetResponseData(wingMembers
                        .Select(w => new WingMemberData(w, w.User != user))
                        .OrderBy(w => w.Name)
                        .ToList()));
            }
            return new WebSocketHandlerResultError();
        }
    }
}
