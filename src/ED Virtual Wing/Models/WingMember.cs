using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("WingMember")]
    public class WingMember
    {
        [JsonIgnore]
        [Column]
        public int Id { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [ForeignKey("WingId")]
        public Wing? Wing { get; set; }

        [Column]
        public WingMembershipStatus Status { get; set; }

        [Column]
        public DateTimeOffset Joined { get; set; }

    }

    public enum WingMembershipStatus : short
    {
        Left = 0,
        PendingApproval,
        Joined,
        Banned,
    }
}
