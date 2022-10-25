using Lesson16.Code;
using Lesson19.CrossLib;
using Lesson9.Code;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.BattleService
{
    //public Guid GameGuid { get; set; }
    //public Guid GameObjectGuid { get; set; }

    public class CheckRightsMultiplayerGameCommandsHandlerDecorator : IMessageHandler
    {
        IMessageHandler _innerGameCommandHandler;
        IContainer _container;

        public CheckRightsMultiplayerGameCommandsHandlerDecorator(IMessageHandler innerGameCommandHandler, IContainer container)
        {
            if (innerGameCommandHandler == null)
            {
                throw new ArgumentNullException(nameof(innerGameCommandHandler));
            }

            _innerGameCommandHandler = innerGameCommandHandler;
            _container = container;
        }

        public object HandleMessageJson(string dataJson)
        {
            if (dataJson == null)
            {
                throw new ArgumentNullException(nameof(dataJson));
            }

            var jObject = JObject.Parse(dataJson);

            if (jObject["gameGuid"] != null && jObject["gameObjectGuid"] != null)
            {
                var gameGuid = jObject.Value<string>("gameGuid");
                var gameObjectGuid = jObject.Value<string>("gameObjectGuid");



                if (gameGuid != null && gameObjectGuid != null)
                {


                    return _innerGameCommandHandler.HandleMessageJson(dataJson);
                }
            }

            throw new ExceptionWithCode(401, "Unauthorized");
        }
    }
}
