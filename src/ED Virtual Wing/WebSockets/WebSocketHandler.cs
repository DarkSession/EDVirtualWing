using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
namespace ED_Virtual_Wing.WebSockets
{
    public abstract class WebSocketHandler
    {
        public abstract ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessage message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext);

        protected WebSocketHandlerResultSuccess CreateResponseFromMessage(WebSocketMessage message, object data)
        {
            return new WebSocketHandlerResultSuccess(new WebSocketMessage(message.Name, data, message.MessageId));
        }
    }

    public abstract class WebSocketHandlerResult
    {
        public WebSocketMessage? Response { get; }
        protected WebSocketHandlerResult(WebSocketMessage response)
        {
            Response = response;
        }
    }

    /// <summary>
    /// Use this class as a result of a WebSocketHandler.ProcessMessage indicating that everything went well and you want to send a response back to the client.
    /// </summary>
    public class WebSocketHandlerResultSuccess : WebSocketHandlerResult
    {
        public WebSocketHandlerResultSuccess(WebSocketMessage response) : base(response)
        {
        }
    }

    /// <summary>
    /// Use this class as a result of a WebSocketHandler.ProcessMessage indicating that everything went well but there is no response going back to the client.
    /// </summary>
    public class WebSocketHandlerResultVoid : WebSocketHandlerResult
    {
        public WebSocketHandlerResultVoid() : base(null)
        {
        }
    }

    /// <summary>
    /// Use this class as a result of a WebSocketHandler.ProcessMessage indicating that something went wrong and you want to send an error message back to the client.
    /// </summary>
    public class WebSocketHandlerResultError : WebSocketHandlerResult
    {
        public List<string> Errors { get; }
        public WebSocketHandlerResultError(List<string>? errors = null) : base(null)
        {
            Errors = errors ?? new List<string>();
        }
    }
}
