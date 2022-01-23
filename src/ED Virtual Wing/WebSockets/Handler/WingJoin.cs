using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class WingJoin : WebSocketHandler
    {
        class WingJoinRequestData
        {
            [Required]
            public string Invite { get; set; } = string.Empty;
            [Required]
            public bool Join { get; set; }
        }

        class WingJoinResponse
        {
            public bool Joined { get; set; }
            public WingMembershipStatus Status { get; set; }
            public Wing Wing { get; set; }
            public WingJoinResponse(bool joined, WingMembershipStatus status, Wing wing)
            {
                Joined = joined;
                Status = status;
                Wing = wing;
            }
        }

        protected override Type? MessageDataType { get; } = typeof(WingJoinRequestData);

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            WingJoinRequestData? data = message.Data?.ToObject<WingJoinRequestData>();
            if (data != null)
            {
                Wing? wing = await applicationDbContext.Wings
                            .Include(w => w.Owner)
                            .FirstOrDefaultAsync(w => w.Invites!.Any(i => i.Invite == data.Invite && i.Status == WingInviteStatus.Active) && w.Status == WingStatus.Active);
                if (wing != null)
                {
                    WingMember? wingMember = await applicationDbContext.WingMembers.FirstOrDefaultAsync(w => w.Wing == wing && w.User == user);
                    switch (wingMember?.Status)
                    {
                        case WingMembershipStatus.Banned:
                            {
                                return new WebSocketHandlerResultError("You have been banned from this wing/team.");
                            }
                        case WingMembershipStatus.PendingApproval:
                            {
                                return new WebSocketHandlerResultError("You already submitted a request to join this wing/team but it hasn't been processed yet.");
                            }
                        case WingMembershipStatus.Joined:
                            {
                                return new WebSocketHandlerResultError("You are already a member of this wing/team");
                            }
                    }
                    if (data.Join)
                    {
                        WingMember wingMemberJoin = new()
                        {
                            Status = (wing.JoinRequirement == WingJoinRequirement.Invite) ? WingMembershipStatus.Joined : WingMembershipStatus.PendingApproval,
                            User = user,
                            Joined = DateTimeOffset.Now,
                            Wing = wing,
                        };
                        applicationDbContext.WingMembers.Add(wingMemberJoin);
                        return new WebSocketHandlerResultSuccess(new WingJoinResponse(true, wingMemberJoin.Status, wing));
                    }
                    return new WebSocketHandlerResultSuccess(new WingJoinResponse(true, WingMembershipStatus.Left, wing));
                }
            }
            return new WebSocketHandlerResultError("Invite not found.");
        }
    }
}
