using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("WingInvite")]
    [Index(nameof(Invite), IsUnique = true)]
    public class WingInvite
    {
        [JsonIgnore]
        [Column]
        public int Id { get; set; }

        [JsonIgnore]
        [ForeignKey("WingId")]
        public Wing? Wing { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string Invite { get; set; } = string.Empty;

        [Column]
        public DateTimeOffset Created { get; set; }

        [Column]
        public DateTimeOffset? Expires { get; set; }

        [Column]
        public WingInviteStatus Status { get; set; } = WingInviteStatus.Active;
    }

    public enum WingInviteStatus : byte
    {
        Expires = 0,
        Active,
    }
}
