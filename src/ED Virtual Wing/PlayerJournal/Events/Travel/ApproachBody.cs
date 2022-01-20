using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class ApproachBody : JournalEventHandler
    {
        public long SystemAddress { get; set; }
        public int BodyID { get; set; }
        public string Body { get; set; } = string.Empty;
        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            StarSystem? starSystem = await applicationDbContext.StarSystems.FirstOrDefaultAsync(s => s.SystemAddress == SystemAddress);
            if (starSystem != null && commander.Location != null)
            {
                StarSystemBody? starSystemBody = await applicationDbContext.StarSystemBodies.FirstOrDefaultAsync(s => s.StarSystem == starSystem && s.BodyId == BodyID);
                if (starSystemBody == null)
                {
                    starSystemBody = new()
                    {
                        StarSystem = starSystem,
                        Name = Body,
                    };
                    applicationDbContext.StarSystemBodies.Add(starSystemBody);
                    await applicationDbContext.SaveChangesAsync();
                }
                commander.Location.SetLocationBody(starSystem, starSystemBody);
            }
        }
    }
}
