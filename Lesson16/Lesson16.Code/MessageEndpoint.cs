using Lesson12.Code.Exceptions;
using Lesson5.Code.Commands;
using Lesson8.Code;
using Lesson9.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Lesson16.Code
{
    public class MessageEndpoint : IMessageEndpoint
    {
        string COMMAND_JSON_TYPE = "type";
        string COMMAND_JSON_DATA = "data";

        IContainer _container;
        public MessageEndpoint(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
        }

        public IMessageHandlerResponse HandleMessage(string messageJson)
        {
            if (messageJson == null)
            {
                throw new ArgumentNullException(nameof(messageJson));
            }

            try
            {
                var jObject = JObject.Parse(messageJson);
                var commandType = jObject.Value<string>(COMMAND_JSON_TYPE);

                if (_container.CanResolve<IMessageHandler>(commandType))
                {
                    var jsonData = jObject[COMMAND_JSON_DATA].ToString();
                    var result = _container.Resolve<IMessageHandler>(commandType).HandleMessageJson(jsonData);

                    return new MessageHandlerResponse()
                    {
                        Data = result,
                        Success = true,
                        Status = 200,
                    };
                }
                else
                {
                    return new MessageHandlerResponse()
                    {
                        Error = $"Message handler for command type {commandType} is not registered",
                        Status = 400,
                        Success = false,
                    };
                }
            }
            catch (ExceptionWithCode ex)
            {
                return new MessageHandlerResponse()
                {
                    Error = ex.Message,
                    Status = ex.Code,
                    Success = false,
                };
            }
            catch (Exception ex)
            {
                return new MessageHandlerResponse()
                {
                    Error = ex.Message,
                    Status = 500,
                    Success = false,
                };
            }
        }
    }
}
