using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("Wing")]
    [Index(nameof(WingId), IsUnique = true)]
    public class Wing
    {
        [JsonIgnore]
        [Column]
        public int Id { get; set; }

        [Column]
        public Guid WingId { get; set; } = Guid.NewGuid();

        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; } = string.Empty;

        [ForeignKey("OwnerId")]
        [JsonIgnore]
        public ApplicationUser? Owner { get; set; }

        [Column]
        [JsonIgnore]
        public DateTimeOffset Created { get; set; }

        [Column]
        [JsonIgnore]
        public WingStatus Status { get; set; } = WingStatus.Active;

        [Column]
        [JsonIgnore]
        public WingJoinRequirement JoinRequirement { get; set; }

        [JsonIgnore]
        public IEnumerable<WingMember>? Members { get; set; }
    }

    public enum WingStatus : short
    {
        Deleted = 0,
        Active,
    }

    public enum WingJoinRequirement : short
    {
        Invite = 0,
        Approval,
    }
}
