using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    class FSDJump : JournalEventHandler
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
            }
            commander.CurrentStarSystem = starSystem;
        }
    }
}
