using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class LeaveBody : JournalEventHandler
    {
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (commander.Location != null)
            {
                commander.Location.SetLocationBody(null);
            }
            return ValueTask.CompletedTask;
        }
    }
}
