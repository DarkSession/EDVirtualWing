using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.WebSockets.Messages
{
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

    public static class CommanderExtensions
    {
        public static async Task DistributeCommanderData(this Commander commander, WebSocketServer webSocketServer, ApplicationDbContext applicationDbContext)
        {
            if (!string.IsNullOrEmpty(commander.Name))
            {
                List<Wing> wings = await commander.User.GetWings(applicationDbContext);
                IEnumerable<WebSocketSession> sessions = webSocketServer.ActiveSessions
                    .Where(a => wings.Any(w => w.Id == a.ActiveWing?.Id));
                foreach (WebSocketSession session in sessions)
                {
                    WebSocketMessage updateMessage = new("CommanderUpdated", new CommanderUpdatedMessage(commander, session.ActiveWing!));
                    await updateMessage.Send(session);
                }
            }
        }

        public static async ValueTask OtherCommanderWsInstancesNotifyStreaming(this Commander commander, WebSocketServer webSocketServer, bool status)
        {
            IEnumerable<WebSocketSession> sessions = webSocketServer.ActiveSessions
                .Where(a => a.User == commander.User);
            foreach (WebSocketSession session in sessions)
            {
                WebSocketMessage updateMessage = new("JournalStreamingChanged", new JournalStreamingChangedMessage(status));
                await updateMessage.Send(session);
            }
        }
    }
}
