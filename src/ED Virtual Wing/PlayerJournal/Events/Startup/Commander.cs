using ED_Virtual_Wing.Data;

namespace ED_Virtual_Wing.PlayerJournal.Events.Startup.CMDR
{
    public class Commander : JournalEventHandler
    {
        public string? Name { get; set; }
        public override ValueTask ProcessEntry(Models.Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                commander.Name = Name;
            }
            return ValueTask.CompletedTask;
        }
    }
}
