using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingSubscribe : WebSocketHandler
    {
        class WingSubscribeRequestData
        {
            [Required]
            public string WingId { get; set; } = string.Empty;
        }

        class WingSubscribeResponse
        {
            public Wing Wing { get; set; }
            public List<Commander> Commanders { get; set; }
            public WingSubscribeResponse(Wing wing, List<Commander> commanders)
            {
                Wing = wing;
                Commanders = commanders;
            }
        }

        protected override Type? MessageDataType { get; } = typeof(WingSubscribeRequestData);

        public WingSubscribe()
        {
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingSubscribeRequestData? data = message.Data?.ToObject<WingSubscribeRequestData>();
            if (data != null && Guid.TryParse(data.WingId, out Guid wingId))
            {
                Wing? wing = await applicationDbContext.Wings.FirstOrDefaultAsync(w => w.WingId == wingId && w.Members!.Any(m => m.Status == WingMembershipStatus.Joined && m.User == user));
                if (wing == null)
                {
                    return new WebSocketHandlerResultError("You are not a member of this wing.");
                }
                webSocketSession.ActiveWing = wing;

                List<Commander> commanders = await applicationDbContext.Commanders
                    .Include(c => c.Target)
                    .Include(c => c.Target!.Body)
                    .Include(c => c.Target!.StarSystem)
                    .Include(c => c.Location)
                    .Include(c => c.Location!.StarSystem)
                    .Include(c => c.Location!.SystemBody)
                    .Include(c => c.Location!.Station)
                    .Where(c => c.User.WingMemberships!.Any(w => w.Wing == wing && w.Status == WingMembershipStatus.Joined))
                    .ToListAsync();

                return new WebSocketHandlerResultSuccess(new WingSubscribeResponse(wing, commanders));
            }
            return new WebSocketHandlerResultError();
        }
    }
}
