using ED_Virtual_Wing.Models;
using System.Net.WebSockets;

namespace ED_Virtual_Wing.WebSockets
{
    public class WebSocketSession
    {
        public WebSocket WebSocket { get; }
        public ApplicationUser User { get; }
        public Wing? ActiveWing { get; set; }
        public bool StreamingJournal { get; set; }

        public WebSocketSession(WebSocket webSocket, ApplicationUser user)
        {
            WebSocket = webSocket;
            User = user;
        }
    }
}
