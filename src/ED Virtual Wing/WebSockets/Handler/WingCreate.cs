using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingCreate : WebSocketHandler
    {
        class WingCreateRequestData
        {
            [Required]
            [MinLength(6)]
            [MaxLength(64)]
            public string Name { get; set; } = string.Empty;
        }

        class WingCreateResponse
        {
        }

        protected override Type? MessageDataType { get; } = typeof(WingCreateRequestData);

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingCreateRequestData? data = message.Data?.ToObject<WingCreateRequestData>();
            if (data != null)
            {
                if (await applicationDbContext.Wings.AnyAsync(w => w.Owner == user && w.Status == WingStatus.Active && EF.Functions.Like(w.Name, data.Name)))
                {
                    // A wing with the provided name already exists for the specified user
                    return new WebSocketHandlerResultError("A wing with this name already exists.");
                }
                Wing wing = new()
                {
                    Created = DateTimeOffset.Now,
                    Name = data.Name,
                    Owner = user,
                };
                applicationDbContext.Wings.Add(wing);
                await applicationDbContext.SaveChangesAsync();
                applicationDbContext.WingMembers.Add(new WingMember()
                {
                    Wing = wing,
                    Joined = DateTimeOffset.Now,
                    Status = WingMembershipStatus.Joined,
                    User = user,
                });
                return new WebSocketHandlerResultSuccess();
            }
            return new WebSocketHandlerResultError();
        }
    }
}
