using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal;
using Newtonsoft.Json.Linq;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class SendJournal : WebSocketHandler
    {
        class SendJournalRequestData
        {
            public List<JObject>? Entries { get; set; }
        }

        private JournalProcessor JournalProcessor { get; }
        public SendJournal(JournalProcessor journalProcessor)
        {
            JournalProcessor = journalProcessor;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            SendJournalRequestData? data = message.Data?.ToObject<SendJournalRequestData>();
            if (data?.Entries != null)
            {
                Commander commander = await user.GetCommander(applicationDbContext);
                foreach (JObject userJournalEntry in data.Entries)
                {
                    await JournalProcessor.ProcessUserJournalEntry(userJournalEntry, commander, applicationDbContext);
                }
                return new WebSocketHandlerResultSuccess();
            }
            return new WebSocketHandlerResultError();
        }
    }
}
