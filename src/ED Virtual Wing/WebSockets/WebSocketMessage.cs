using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Text;

namespace ED_Virtual_Wing.WebSockets
{
    public class WebSocketMessage
    {
        [Required]
        public string? Name { get; set; }
        public object? Data { get; set; } = null;

        public WebSocketMessage()
        {
        }

        public WebSocketMessage(string name, object? data = null)
        {
            Name = name;
            Data = data;
        }

        public ValueTask Send(WebSocket ws)
        {
            string msg = JsonConvert.SerializeObject(this);
            ReadOnlyMemory<byte> message = new(Encoding.UTF8.GetBytes(msg));
            return ws.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
