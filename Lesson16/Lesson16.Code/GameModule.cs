using Lesson12.Code;
using Lesson12.Code.Compilation;
using Lesson14.Code.Loops;
using Lesson16.Code.Commands;
using Lesson16.Code.Handlers;
using Lesson5.Code.Commands;
using Lesson7.Code.Commands;
using Lesson8.Code;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public class GameModule : IContainerModule
    {
        public void RegisterModule(IRegistration reg)
        {
            reg.RegisterModule(new AsyncLoopModule());

            reg.Register<IUObject>().As<UObject>().Complete();
            reg.Register<IGame>().As<Game>().Complete();
            reg.Register<IRuntimeCompiler>().As<RuntimeCompiler>().AsSingleton().Complete();
            reg.Register<IUObjectAdapterFactory>().As<UObjectAdapterFactory>().AsSingleton().Complete();
            reg.Register<IGameLoop>().As<GameAsyncLoop>().AsSingleton().Complete();
            reg.Register<IMessageEndpoint>().As<MessageEndpoint>().AsSingleton().Complete();
            reg.Register<IInterpretCommand>().As<InterpretCommand>().Complete();
            reg.Register<ILog>().As<FakeLog>().AsSingleton().Complete();

            reg.Register<IMessageHandler>().As<GameCommandHandler>().WithName("GAMEOBJECT_COMMAND").AsSingleton().Complete();
            reg.Register<ICommand>().As<ChangeVelocityAdaptCommand>().WithName("CHANGE_VELOCITY").Complete();
            reg.Register<ICommand>().As<ChangeVelocityCommand>().WithName("CHANGE_VELOCITY_COMMAND").Complete();
        }
    }
}
