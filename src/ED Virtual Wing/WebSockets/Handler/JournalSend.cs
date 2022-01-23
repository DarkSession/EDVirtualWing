using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal;
using Newtonsoft.Json.Linq;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class JournalSend : WebSocketHandler
    {
        class JournalSendRequestData
        {
            public List<JObject>? Entries { get; set; }
        }

        class CommanderUpdatedMessage
        {
            public Commander Commander { get; set; }
            public Wing Wing { get; set; }
            public CommanderUpdatedMessage(Commander commander, Wing wing)
            {
                Commander = commander;
                Wing = wing;
            }
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
                foreach (JObject userJournalEntry in data.Entries)
                {
                    await JournalProcessor.ProcessUserJournalEntry(userJournalEntry, commander, applicationDbContext);
                }
                await applicationDbContext.SaveChangesAsync();
                {
                    List<Wing> wings = await user.GetWings(applicationDbContext);
                    IEnumerable<WebSocketSession> sessions = WebSocketServer.ActiveSessions
                        .Where(a => wings.Any(w => w.Id == a.ActiveWing?.Id));
                    foreach (WebSocketSession session in sessions)
                    {
                        WebSocketMessage updateMessage = new("CommanderUpdated", new CommanderUpdatedMessage(commander, session.ActiveWing!));
                        await updateMessage.Send(session);
                    }
                }
                return new WebSocketHandlerResultSuccess();
            }
            return new WebSocketHandlerResultError();
        }
    }
}
