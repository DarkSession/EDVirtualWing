using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class FSDJump : JournalEventHandler
    {
        public string StarSystem { get; set; } = string.Empty;
        public long SystemAddress { get; set; }
        public List<decimal>? StarPos { get; set; }

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.GameActivity = GameActivity.Supercruise;
            StarSystem? starSystem = await applicationDbContext.StarSystems.FindAsync(SystemAddress);
            if (starSystem == null && StarPos != null && StarPos.Count == 3)
            {
                starSystem = new()
                {
                    SystemAddress = SystemAddress,
                    Name = StarSystem,
                    LocationX = StarPos[0],
                    LocationY = StarPos[1],
                    LocationZ = StarPos[2],
                };
                applicationDbContext.StarSystems.Add(starSystem);
                await applicationDbContext.SaveChangesAsync();
            }
            if (commander.Location != null && starSystem != null)
            {
                if (commander.Location.StarSystem?.SystemAddress == starSystem.SystemAddress)
                {
                    // This means the the CMDR probably got hyperdicted by Thargoids
                    commander.ExtraFlags |= GameExtraFlags.Hyperdicted;
                }
                commander.Location.SetLocationSystem(starSystem);
            }
        }
    }
}
