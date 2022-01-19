using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    class StartJump : JournalEventHandler
    {
        public string? JumpType { get; set; }
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.GameActivity = JumpType switch
            {
                "Hyperspace" => GameActivity.Hyperspace,
                "Supercruise" => GameActivity.Supercruise,
                _ => commander.GameActivity,
            };
            commander.Location.Station = null;
            commander.Target.ResetShipTarget();
            return ValueTask.CompletedTask;
        }
    }
}
