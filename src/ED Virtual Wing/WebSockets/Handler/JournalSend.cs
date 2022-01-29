using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal;
using ED_Virtual_Wing.WebSockets.Messages;
using Newtonsoft.Json.Linq;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class JournalSend : WebSocketHandler
    {
        class JournalSendRequestData
        {
            public List<JObject>? Entries { get; set; }
        }

        protected override Type? MessageDataType { get; } = typeof(JournalSendRequestData);
        private JournalProcessor JournalProcessor { get; }
        private WebSocketServer WebSocketServer { get; }
        public JournalSend(JournalProcessor journalProcessor, WebSocketServer webSocketServer)
        {
            JournalProcessor = journalProcessor;
            WebSocketServer = webSocketServer;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            JournalSendRequestData? data = message.Data?.ToObject<JournalSendRequestData>();
            if (data?.Entries != null)
            {
                Commander commander = await user.GetCommander(applicationDbContext);
                webSocketSession.StreamingJournal = true;
                if (!commander.IsStreaming)
                {
                    await commander.OtherCommanderWsInstancesNotifyStreaming(WebSocketServer, true);
                }
                foreach (JObject userJournalEntry in data.Entries)
                {
                    await JournalProcessor.ProcessUserJournalEntry(userJournalEntry, commander, applicationDbContext);
                }
                commander.LastEventDate = DateTimeOffset.Now;
                commander.LastActivity = DateTimeOffset.Now;
                await applicationDbContext.SaveChangesAsync();
                await commander.DistributeCommanderData(WebSocketServer, applicationDbContext);
                return new WebSocketHandlerResultSuccess();
            }
            return new WebSocketHandlerResultError();
        }
    }
}
