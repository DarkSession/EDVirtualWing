namespace ED_Virtual_Wing.WebSockets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WebSocketActionAttribute : Attribute
    {
        public WebSocketAction Action { get; }
        public WebSocketActionAttribute(WebSocketAction action)
        {
            Action = action;
        }
    }

    public interface IWebSocketAction
    {
        public ValueTask Process(WebSocketSession webSocketSession);
    }

    public enum WebSocketAction : byte
    {
        OnUserConnected,
        OnUserDisconnected,
    }
}
