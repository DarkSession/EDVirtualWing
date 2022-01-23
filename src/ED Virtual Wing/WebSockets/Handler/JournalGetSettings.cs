using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class JournalGetSettings : WebSocketHandler
    {
        class JournalGetSettingsResponseData
        {
            public List<string> Events { get; }
            public DateTimeOffset JournalLastEventDate { get; }
            public JournalGetSettingsResponseData(List<string> events, Commander commander)
            {
                Events = events;
                JournalLastEventDate = commander.JournalLastEventDate;
            }
        }
        protected override Type? MessageDataType { get; } = null;
        private JournalProcessor JournalProcessor { get; }
        public JournalGetSettings(JournalProcessor journalProcessor)
        {
            JournalProcessor = journalProcessor;
        }

        public override async ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            Commander commander = await user.GetCommander(applicationDbContext);
            JournalGetSettingsResponseData getJournalSettingsResponse = new(JournalProcessor.RelevantJournalEvents, commander);
            return new WebSocketHandlerResultSuccess(getJournalSettingsResponse);
        }
    }
}
