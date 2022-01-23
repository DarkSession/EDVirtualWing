﻿using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Validation;

namespace ED_Virtual_Wing.WebSockets
{
    public abstract class WebSocketHandler
    {
        protected abstract Type? MessageDataType { get; }

        public abstract ValueTask<WebSocketHandlerResult> ProcessMessage(WebSocketMessageReceived message, WebSocketSession webSocketSession, ApplicationUser user, ApplicationDbContext applicationDbContext);

        public bool ValidateMessageData(JObject? data)
        {
            if (data == null)
            {
                return (MessageDataType == null);
            }
            else if (MessageDataType == null)
            {
                return true;
            }
            JsonSchema schema = JsonSchema.FromType(MessageDataType);
            ICollection<ValidationError> errors = schema.Validate(data);
            return errors.Count == 0;
        }
    }

    public abstract class WebSocketHandlerResult
    {
    }

    /// <summary>
    /// Use this class as a result of a WebSocketHandler.ProcessMessage indicating that everything went well and you want to send a response back to the client.
    /// </summary>
    public class WebSocketHandlerResultSuccess : WebSocketHandlerResult
    {
        public object ResponseData { get; }
        public WebSocketHandlerResultSuccess(object responseData)
        {
            ResponseData = responseData;
        }

        public WebSocketHandlerResultSuccess()
        {
            ResponseData = new WebSocketHandlerResultSuccessData()
            {
                Success = true,
            };
        }
    }

    /// <summary>
    /// Use this class as a result of a WebSocketHandler.ProcessMessage indicating that everything went well but there is no response going back to the client.
    /// </summary>
    public class WebSocketHandlerResultVoid : WebSocketHandlerResult
    {
    }

    /// <summary>
    /// Use this class as a result of a WebSocketHandler.ProcessMessage indicating that something went wrong and you want to send an error message back to the client.
    /// </summary>
    public class WebSocketHandlerResultError : WebSocketHandlerResult
    {
        public List<string> Errors { get; }
        public WebSocketHandlerResultError(List<string>? errors = null)
        {
            Errors = errors ?? new List<string>();
        }

        public WebSocketHandlerResultError(string error) : this(new List<string>() { error })
        {
        }
    }

    public class WebSocketHandlerResultSuccessData
    {
        public bool Success { get; set; }
    }
}
