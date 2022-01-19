using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.PlayerJournal.Events.Exploration
{
    public class Scan : JournalEventHandler
    {
        public long SystemAddress { get; set; }
        public string BodyName { get; set; } = string.Empty;
        public int BodyID { get; set; }

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            StarSystem? starSystem = await applicationDbContext.StarSystems.FirstOrDefaultAsync(s => s.SystemAddress == SystemAddress);
            if (starSystem != null)
            {
                if (!await applicationDbContext.StarSystemBodies.AnyAsync(s => s.StarSystem == starSystem && s.BodyId == BodyID))
                {
                    applicationDbContext.StarSystemBodies.Add(new StarSystemBody()
                    {
                        StarSystem = starSystem,
                        BodyId = BodyID,
                        Name = BodyName,
                    });
                }
            }
        }
    }
}
