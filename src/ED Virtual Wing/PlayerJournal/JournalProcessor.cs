namespace ED_Virtual_Wing.PlayerJournal
{
    public class JournalProcessor
    {
        private Dictionary<string, Type> JournalEntryProcessors { get; } = new();
        public List<string> RelevantJournalEvents { get; }

        public JournalProcessor()
        {
            IEnumerable<Type> webSocketHandlerTypes = GetType().Assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(JournalEntryHandler)));
            foreach (Type type in webSocketHandlerTypes)
            {
                JournalEntryProcessors[type.Name] = type;
            }
            RelevantJournalEvents = JournalEntryProcessors.Keys.ToList();
        }

        public ValueTask ProcessUserJournalEntry()
        {
            return ValueTask.CompletedTask;
        }
    }
}
