using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class JournalGetSettings : WebSocketHandler
    {
        class JournalGetSettingsResponseData
        {
            public bool StreamingJournal { get; set; }
            public List<string> Events { get; }
            public DateTimeOffset JournalLastEventDate { get; }
            public JournalGetSettingsResponseData(List<string> events, Commander commander, bool streamingJournal)
            {
                Events = events;
                JournalLastEventDate = commander.JournalLastEventDate;
                StreamingJournal = streamingJournal;
            }
        }
        protected override Type? MessageDataType { get; }
        private JournalProcessor JournalProcessor { get; }
        private WebSocketServer WebSocketServer { get; }
        public JournalGetSettings(JournalProcessor journalProcessor, WebSocketServer webSocketServer)
        {
            JournalProcessor = journalProcessor;
            WebSocketServer = webSocketServer;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            Commander commander = await user.GetCommander(applicationDbContext);
            bool streamingJournal = WebSocketServer.ActiveSessions.Any(a => a.User == user && a.StreamingJournal);
            JournalGetSettingsResponseData getJournalSettingsResponse = new(JournalProcessor.RelevantJournalEvents, commander, streamingJournal);
            return new WebSocketHandlerResultSuccess(getJournalSettingsResponse);
        }
    }
}
