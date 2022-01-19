using ED_Virtual_Wing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser
    {
        public Commander? Commander { get; set; }

        public async Task<Commander> GetCommander(ApplicationDbContext applicationDbContext)
        {
            Commander? commander = await applicationDbContext.Commanders
                .Include(c => c.Location)
                .Include(c => c.Location.StarSystem)
                .Include(c => c.Location.Station)
                .Include(c => c.Location.SystemBody)
                .Include(c => c.Target)
                .Include(c => c.Target.StarSystem)
                .Include(c => c.Target.Body)
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
    }
}