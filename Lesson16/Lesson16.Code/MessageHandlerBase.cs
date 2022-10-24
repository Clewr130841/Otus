using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public abstract class MessageHandlerBase<T> : IMessageHandler
    {
        public void HandleMessageJson(string dataJson)
        {
            if (dataJson == null)
            {
                throw new ArgumentNullException(nameof(dataJson));
            }

            var data = JsonConvert.DeserializeObject<T>(dataJson);

            HandleMessage(data);
        }

        public abstract void HandleMessage(T data);
    }
}
