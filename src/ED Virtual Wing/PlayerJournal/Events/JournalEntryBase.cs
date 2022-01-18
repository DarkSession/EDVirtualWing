using Newtonsoft.Json;

namespace ED_Virtual_Wing.PlayerJournal.Events
{
    public class JournalEntryBase
    {
        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
        [JsonProperty("event")]
        public string Event { get; set; } = string.Empty;
    }
}
