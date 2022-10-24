using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public abstract class MessageHandlerBase<T1, T2> : IMessageHandler
    {
        public object HandleMessageJson(string dataJson)
        {
            if (dataJson == null)
            {
                throw new ArgumentNullException(nameof(dataJson));
            }

            var data = JsonConvert.DeserializeObject<T1>(dataJson);

            return JsonConvert.SerializeObject(HandleMessage(data));
        }

        public abstract T2 HandleMessage(T1 data);
    }
}
