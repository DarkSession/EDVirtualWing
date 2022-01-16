using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace ED_Virtual_Wing.WebSockets
{
    public class WebSocketServer
    {
        private List<WebSocketSession> WebSocketSessions { get; } = new();
        private Dictionary<string, Type> WebSocketHandlers { get; } = new();
        public WebSocketServer()
        {
            IEnumerable<Type> webSocketHandlerTypes = GetType().Assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(WebSocketHandler)));
            foreach (Type type in webSocketHandlerTypes)
            {
                WebSocketHandlers[type.Name] = type;
            }
        }

        public async Task ProcessRequest(HttpContext httpContext, ApplicationUser? applicationUser, IServiceScopeFactory serviceScopeFactory)
        {
            using WebSocket ws = await httpContext.WebSockets.AcceptWebSocketAsync();
            if (ws.State != WebSocketState.Open)
            {
                return;
            }
            bool isAuthenticated = (applicationUser != null);
            WebSocketMessage authenticationMessage = new("Authentication", new AuthenticationStatus(isAuthenticated));
            await authenticationMessage.Send(ws);
            if (!isAuthenticated || applicationUser == null)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                return;
            }
            WebSocketSession webSocketSession = new(ws, applicationUser);
            lock (WebSocketSessions)
            {
                WebSocketSessions.Add(webSocketSession);
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
                            await ProcessMessage(webSocketSession, message, serviceScopeFactory);
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
            lock (WebSocketSessions)
            {
                WebSocketSessions.Remove(webSocketSession);
            }
            if (ws.State == WebSocketState.Open)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }
        }

        private async Task ProcessMessage(WebSocketSession webSocketSession, MemoryStream messageStream, IServiceScopeFactory serviceScopeFactory)
        {
            using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
            string message = Encoding.UTF8.GetString(messageStream.ToArray());
            WebSocketMessage? webSocketMessage = JsonConvert.DeserializeObject<WebSocketMessage>(message);
            if (webSocketMessage?.Name != null && (WebSocketHandlers?.TryGetValue(webSocketMessage.Name, out Type? messageHandler) ?? false))
            {
                ApplicationDbContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                ApplicationUser? user = await applicationDbContext.Users.FindAsync(webSocketSession.User.Id);
                if (user != null)
                {
                    WebSocketHandler webSocketHandler = (WebSocketHandler)ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, messageHandler);
                    var result = await webSocketHandler.ProcessMessage(webSocketMessage, webSocketSession, user, applicationDbContext);
                    await applicationDbContext.SaveChangesAsync();
                }
            }
        }

        class AuthenticationStatus
        {
            public bool IsAuthenticated { get; set; }
            public AuthenticationStatus(bool isAuthenticated)
            {
                IsAuthenticated = isAuthenticated;
            }
        }
    }
}
