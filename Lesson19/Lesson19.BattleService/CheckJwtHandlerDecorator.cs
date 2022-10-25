using Lesson16.Code;
using Lesson19.CrossLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.BattleService
{
    public class CheckJwtHandlerDecorator : IMessageHandler
    {
        IMessageHandler _innerHandler;
        IJwtValidateService _jwtValidateService;

        public CheckJwtHandlerDecorator(IMessageHandler innerHandler, IJwtValidateService jwtValidateService)
        {
            if (innerHandler == null)
            {
                throw new ArgumentNullException(nameof(innerHandler));
            }

            if (jwtValidateService == null)
            {
                throw new ArgumentNullException(nameof(jwtValidateService));
            }

            _innerHandler = innerHandler;
            _jwtValidateService = jwtValidateService;
        }

        public object HandleMessageJson(string dataJson)
        {
            if (dataJson == null)
            {
                throw new ArgumentNullException(nameof(dataJson));
            }

            var jObject = JObject.Parse(dataJson);

            if (jObject["jwt"] != null)
            {
                var token = jObject.Value<string>("jwt");

                if (token != null && _jwtValidateService.Check(token))
                {
                    return _innerHandler.HandleMessageJson(dataJson);
                }
            }

            throw new ExceptionWithCode(401, "Unauthorized");
        }
    }
}
