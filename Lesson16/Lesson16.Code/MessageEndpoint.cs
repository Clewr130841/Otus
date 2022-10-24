using Lesson12.Code.Exceptions;
using Lesson5.Code.Commands;
using Lesson8.Code;
using Lesson9.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        public void HandleMessage(string messageJson)
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
                    _container.Resolve<IMessageHandler>(commandType).HandleMessageJson(jsonData);
                }
                else
                {
                    throw new Exception($"Message handler for command type {commandType} is not registered");
                }
            }
            catch (SelfHandlingException ex)
            {
                ex.Handle();
            }
        }
    }
}
