using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    class SupercruiseEntry : JournalEventHandler
    {
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.GameActivity = GameActivity.Supercruise;
            if (commander.Location != null)
            {
                commander.Location.SetLocationStation(null);
            }
            commander.Target?.ResetShipTarget();
            return ValueTask.CompletedTask;
        }
    }
}
