using System.Net.WebSockets;

namespace ED_Virtual_Wing.WebSockets
{
    public static class WebSocketServer
    {
        public static async Task ProcessRequest(HttpContext httpContext)
        {
            WebSocket ws = await httpContext.WebSockets.AcceptWebSocketAsync();
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
