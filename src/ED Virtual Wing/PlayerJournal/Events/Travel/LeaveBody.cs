using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class LeaveBody : JournalEventHandler
    {
        public long SystemAddress { get; set; }
        public int BodyID { get; set; }
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            // We are missing an API / database where we can query the body.
            return ValueTask.CompletedTask;
        }
    }
}
