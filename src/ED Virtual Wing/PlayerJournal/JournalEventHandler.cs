using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal
{
    public abstract class JournalEventHandler
    {
        public abstract ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext);
    }
}
