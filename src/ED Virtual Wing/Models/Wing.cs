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

        [NotMapped]
        public string? OwnerName
        {
            get
            {
                return Owner?.UserName;
            }
        }

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

        [JsonIgnore]
        public IEnumerable<WingInvite>? Invites { get; set; }

        public static bool operator ==(Wing? lhs, Wing? rhs)
        {
            if (lhs is null)
            {
                return (rhs is null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Wing? lhs, Wing? rhs) => !(lhs == rhs);

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object? obj)
        {
            return (obj is Wing w && Equals(w));
        }

        public bool Equals(Wing? w)
        {
            if (w is null)
            {
                return false;
            }
            else if (ReferenceEquals(this, w))
            {
                return true;
            }
            else if (GetType() != w.GetType())
            {
                return false;
            }
            return (w.Id == Id);
        }
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
