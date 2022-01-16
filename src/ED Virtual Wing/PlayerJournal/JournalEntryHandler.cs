namespace ED_Virtual_Wing.PlayerJournal
{
    public abstract class JournalEntryHandler
    {
        public abstract ValueTask ProcessEntry();
    }
}
