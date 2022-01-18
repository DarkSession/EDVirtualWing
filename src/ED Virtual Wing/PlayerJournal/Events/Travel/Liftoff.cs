using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class Liftoff : JournalEventHandler
    {
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.GameActivity = GameActivity.None;
            return ValueTask.CompletedTask;
        }
    }
}
