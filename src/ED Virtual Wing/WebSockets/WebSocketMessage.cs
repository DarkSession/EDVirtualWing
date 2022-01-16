using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Text;

namespace ED_Virtual_Wing.WebSockets
{
    public class WebSocketMessage
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public object? Data { get; set; }
        [Required]
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

        public ValueTask Send(WebSocket ws)
        {
            string msg = JsonConvert.SerializeObject(this);
            ReadOnlyMemory<byte> message = new(Encoding.UTF8.GetBytes(msg));
            return ws.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
