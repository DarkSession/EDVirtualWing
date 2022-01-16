using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal;

namespace ED_Virtual_Wing.WebSockets.Handler
{
    public class GetJournalRelevantEvents : WebSocketHandler
    {
        private JournalProcessor JournalProcessor { get; } 
        public GetJournalRelevantEvents(JournalProcessor journalProcessor)
        {
            JournalProcessor = journalProcessor;
        }

        class GetJournalRelevantEventsResponse
        {
            public List<string> Events { get; }
            public GetJournalRelevantEventsResponse(List<string> events)
            {
                Events = events;
            }
        }

        public override ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessage message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext)
        {
            return ValueTask.FromResult<WebSocketHandlerResult>(CreateResponseFromMessage(message, new GetJournalRelevantEventsResponse(JournalProcessor.RelevantJournalEvents)));
        }
    }
}
