using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Validation;
using System.Net.WebSockets;
using System.Text;

namespace ED_Virtual_Wing.WebSockets
{
    public class WebSocketServer
    {
        private ILogger Logger { get; }
        private List<WebSocketSession> WebSocketSessions { get; } = new();
        private Dictionary<string, Type> WebSocketHandlers { get; } = new();
        private JsonSchema WebSocketMessageReceivedSchema { get; } = JsonSchema.FromType<WebSocketMessageReceived>();
        public WebSocketServer(ILogger<WebSocketServer> logger)
        {
            Logger = logger;
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
                        message.Write(buffer.Array, buffer.Offset, webSocketReceiveResult.Count);
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
            JObject messageObject = JObject.Parse(message);
            ICollection<ValidationError> validationErrors = WebSocketMessageReceivedSchema.Validate(messageObject);
            if (validationErrors.Count == 0)
            {
                WebSocketMessageReceived? webSocketMessage = messageObject.ToObject<WebSocketMessageReceived>();
                if (webSocketMessage?.Name != null && (WebSocketHandlers?.TryGetValue(webSocketMessage.Name, out Type? messageHandler) ?? false))
                {
                    ApplicationDbContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    ApplicationUser? user = await applicationDbContext.Users.FindAsync(webSocketSession.User.Id);
                    if (user != null)
                    {
                        try
                        {
                            WebSocketHandler webSocketHandler = (WebSocketHandler)ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, messageHandler);
                            if (webSocketHandler.ValidateMessageData(webSocketMessage.Data))
                            {
                                WebSocketHandlerResult result = await webSocketHandler.ProcessMessage(webSocketMessage, webSocketSession, user, applicationDbContext);
                                if (result is WebSocketHandlerResultSuccess webSocketHandlerResultSuccess)
                                {
                                    WebSocketMessage responseMessage = new(webSocketMessage.Name, webSocketHandlerResultSuccess.ResponseData, webSocketMessage.MessageId);
                                    await responseMessage.Send(webSocketSession.WebSocket);
                                }
                                else if (result is WebSocketHandlerResultError webSocketHandlerResultError)
                                {
                                    WebSocketErrorMessage webSocketErrorMessage = new(webSocketMessage.Name, webSocketHandlerResultError.Errors, webSocketMessage.MessageId);
                                    await webSocketErrorMessage.Send(webSocketSession.WebSocket);
                                }
                                await applicationDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                WebSocketErrorMessage webSocketErrorMessage = new(webSocketMessage.Name, new List<string>() { "The message data received is not in the expected format." });
                                await webSocketErrorMessage.Send(webSocketSession.WebSocket);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, "Error processing WebSocket Message");
                        }
                    }
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
