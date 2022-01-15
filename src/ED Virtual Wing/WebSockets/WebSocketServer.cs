using ED_Virtual_Wing.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.WebSockets;

namespace ED_Virtual_Wing.WebSockets
{
    public static class WebSocketServer
    {
        public class AuthenticationStatus
        {
            public bool IsAuthenticated { get; set; }

            public AuthenticationStatus(bool isAuthenticated)
            {
                IsAuthenticated = isAuthenticated;
            }
        }

        public static async Task ProcessRequest(HttpContext httpContext, UserManager<ApplicationUser> userManager)
        {
            WebSocket ws = await httpContext.WebSockets.AcceptWebSocketAsync();
            if (ws.State != WebSocketState.Open)
            {
                return;
            }
            bool isAuthenticated = false;
            WebSocketMessage authenticationMessage = new("Authentication", new AuthenticationStatus(isAuthenticated));
            await authenticationMessage.Send(ws);
            if (!isAuthenticated)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                return;
            }
            ArraySegment<byte> buffer = new(new byte[4096]);
            bool disconnect = false;
            while (ws.State == WebSocketState.Open && !disconnect)
            {
                await using MemoryStream message = new();
                WebSocketReceiveResult webSocketReceiveResult;
                do
                {
                    webSocketReceiveResult = await ws.ReceiveAsync(buffer, CancellationToken.None);
                    if (buffer.Array != null)
                    {
                        message.Write(buffer.Array, buffer.Offset, buffer.Count);
                    }
                }
                while (!webSocketReceiveResult.EndOfMessage);
                message.Position = 0;
                switch (webSocketReceiveResult.MessageType)
                {
                    case WebSocketMessageType.Text:
                        {
                            break;
                        }
                    case WebSocketMessageType.Binary:
                        {
                            break;
                        }
                    case WebSocketMessageType.Close:
                        {
                            disconnect = true;
                            break;
                        }
                }
            }
            if (ws.State == WebSocketState.Open)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }
        }
    }
}
