using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Text;

namespace ED_Virtual_Wing.WebSockets
{
    public class WebSocketMessage
    {
        public string Name { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string? MessageId { get; set; }

        public WebSocketMessage()
        {
        }

        public WebSocketMessage(string name, object? data = null, string? messageId = null)
        {
            Name = name;
            Data = data;
            MessageId = messageId ?? Guid.NewGuid().ToString();
        }

        public ValueTask Send(WebSocketSession webSocketSession)
        {
            return Send(webSocketSession.WebSocket);
        }

        public ValueTask Send(WebSocket ws)
        {
            if (ws.State == WebSocketState.Open)
            {
                string msg = JsonConvert.SerializeObject(this);
                ReadOnlyMemory<byte> message = new(Encoding.UTF8.GetBytes(msg));
                return ws.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            return ValueTask.CompletedTask;
        }
    }

    public class WebSocketResponseMessage : WebSocketMessage
    {
        public bool Success { get; set; }
        public WebSocketResponseMessage(string name, bool success, object? data = null, string? messageId = null) : base(name, data, messageId)
        {
            Success = success;
        }
    }

    public class WebSocketErrorMessage : WebSocketResponseMessage
    {
        public List<string> Errors { get; }
        public WebSocketErrorMessage(string name, List<string> errors, string? messageId = null) : base(name, false, null, messageId)
        {
            Errors = errors;
        }
    }

    public class WebSocketMessageReceived
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public JObject? Data { get; set; }
        public string? MessageId { get; set; }
    }
}
