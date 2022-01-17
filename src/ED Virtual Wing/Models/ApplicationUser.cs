using ED_Virtual_Wing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Commander? Commander { get; set; }

        public async Task<Commander> GetCommander(ApplicationDbContext applicationDbContext)
        {
            Commander? commander = await applicationDbContext.Commanders
                .Include(c => c.CurrentStarSystem)
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
            }
            return commander;
        }
    }
}