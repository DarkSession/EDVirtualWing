using ED_Virtual_Wing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("ApplicationUser")]
    //[Index(nameof(FDevCustomerId))]
    public class ApplicationUser : IdentityUser
    {
        // [Column]
        // public long FDevCustomerId { get; set; }
        public Commander? Commander { get; set; }
        public List<WingMember>? WingMemberships { get; set; }

        public async Task<Commander> GetCommander(ApplicationDbContext applicationDbContext)
        {
            Commander? commander = await applicationDbContext.Commanders
                .Include(c => c.Location)
                .Include(c => c.Location!.StarSystem)
                .Include(c => c.Location!.Station)
                .Include(c => c.Location!.SystemBody)
                .Include(c => c.Target)
                .Include(c => c.Target!.StarSystem)
                .Include(c => c.Target!.Body)
                .FirstOrDefaultAsync(c => c.User == this);
            if (commander == null)
            {
                commander = new()
                {
                    User = this,
                    JournalLastEventDate = default,
                    Name = string.Empty,
                };
                applicationDbContext.Commanders.Add(commander);
                await applicationDbContext.SaveChangesAsync();
            }
            if (commander.Location == null)
            {
                commander.Location = new()
                {
                    Commander = commander,
                };
            }
            if (commander.Target == null)
            {
                commander.Target = new()
                {
                    Commander = commander,
                };
            }
            return commander;
        }

        public Task<List<Wing>> GetWings(ApplicationDbContext applicationDbContext)
        {
            return applicationDbContext.Wings
                .Where(w => w.Members!.Any(m => m.User == this && m.Status == WingMembershipStatus.Joined))
                .ToListAsync();
        }

        public static bool operator ==(ApplicationUser? lhs, ApplicationUser? rhs)
        {
            if (lhs is null)
            {
                return (rhs is null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ApplicationUser? lhs, ApplicationUser? rhs) => !(lhs == rhs);

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object? obj)
        {
            return (obj is ApplicationUser u && Equals(u));
        }

        public bool Equals(ApplicationUser? u)
        {
            if (u is null)
            {
                return false;
            }
            else if (ReferenceEquals(this, u))
            {
                return true;
            }
            else if (GetType() != u.GetType())
            {
                return false;
            }
            return (u.Id == Id);
        }
    }
}