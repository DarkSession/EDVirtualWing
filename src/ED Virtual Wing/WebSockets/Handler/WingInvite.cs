using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingInvite : WebSocketHandler
    {
        class WingInviteRequest
        {
            [Required]
            public string WingId { get; set; } = string.Empty;
        }

        class WingInviteResponse
        {
            public string Invite { get; set; }
            public WingInviteResponse(string invite)
            {
                Invite = invite;
            }
        }

        protected override Type? MessageDataType { get; } = typeof(WingInviteRequest);

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingInviteRequest? data = message.Data?.ToObject<WingInviteRequest>();
            if (data != null && Guid.TryParse(data.WingId, out Guid wingId))
            {
                Wing? wing = await applicationDbContext.Wings
                    .Include(w => w.Owner)
                    .FirstOrDefaultAsync(w => w.WingId == wingId && w.Status == WingStatus.Active);
                if (wing != null)
                {
                    if (wing.Owner != user)
                    {
                        return new WebSocketHandlerResultError("Only the owner of the team can create invites.");
                    }
                    Models.WingInvite wingInvite = new()
                    {
                        Wing = wing,
                        Created = DateTimeOffset.Now,
                        Status = WingInviteStatus.Active,
                        Invite = Functions.GenerateString(8),
                    };
                    applicationDbContext.Add(wingInvite);
                    return new WebSocketHandlerResultSuccess(wingInvite);
                }
            }
            return new WebSocketHandlerResultError();
        }
    }
}
