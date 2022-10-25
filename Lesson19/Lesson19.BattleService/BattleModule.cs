using Lesson16.Code;
using Lesson16.Code.Handlers;
using Lesson19.CrossLib;
using Lesson19.CrossLib.Keys;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.BattleService
{
    public class BattleModule : IContainerModule
    {
        public void RegisterModule(IRegistration reg)
        {
            reg.RegisterModule(new GameModule());

            reg.Register<IJwtOptions>().As<JwtOptions>().AsSingleton().Complete();

            reg.Register<IMessageHandler>()
                .As((c) => new CheckJwtHandlerDecorator(new GameCommandHandler(c), c.Resolve<IJwtValidateService>()))
                .WithName("GAMEOBJECT_COMMAND")
                .AsSingleton()
                .Complete();

            reg.Register<IMessageHandler>()
                .As((c) => new CheckJwtHandlerDecorator(new NewGameHandler(c), c.Resolve<IJwtValidateService>()))
                .WithName("NEWGAME_COMMAND")
                .AsSingleton()
                .Complete();
        }
    }
}
