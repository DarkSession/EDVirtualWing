using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal.Events;
using Newtonsoft.Json.Linq;

namespace ED_Virtual_Wing.PlayerJournal
{
    public class JournalProcessor
    {
        private ILogger Logger { get; }
        private Dictionary<string, Type> JournalEntryProcessors { get; } = new();
        public List<string> RelevantJournalEvents { get; }

        public JournalProcessor(ILogger<JournalProcessor> logger)
        {
            Logger = logger;
            IEnumerable<Type> webSocketHandlerTypes = GetType().Assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(JournalEventHandler)));
            foreach (Type type in webSocketHandlerTypes)
            {
                JournalEntryProcessors[type.Name] = type;
            }
            RelevantJournalEvents = JournalEntryProcessors.Keys.ToList();
        }

        public async ValueTask ProcessUserJournalEntry(JObject userJournalEntry, Commander commander, ApplicationDbContext applicationDbContext)
        {
            JournalEntryBase? journalEntry = userJournalEntry.ToObject<JournalEntryBase>();
            if (journalEntry != null && journalEntry.Timestamp >= commander.JournalLastEventDate)
            {
                if (journalEntry.Event != "Status")
                {
                    commander.JournalLastEventDate = journalEntry.Timestamp;
                }
                if (JournalEntryProcessors.TryGetValue(journalEntry.Event, out Type? eventHandlerType))
                {
                    JournalEventHandler? journalEventHandler = (JournalEventHandler?)userJournalEntry.ToObject(eventHandlerType);
                    if (journalEventHandler != null)
                    {
                        await journalEventHandler.ProcessEntry(commander, applicationDbContext);
                        Logger.Log(LogLevel.Debug, "Processed '{event}' event", journalEntry.Event);
                    }
                    else
                    {
                        Logger.Log(LogLevel.Warning, "Ignored '{event}' event", journalEntry.Event);
                    }
                }
            }
        }
    }
}
