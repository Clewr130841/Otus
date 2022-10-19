using Lesson14.Code.Loops.Commands;
using Lesson8.Code;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops
{
    public class AsyncLoopModule : IContainerModule
    {
        //Регаем зависимости для цикла
        public void RegisterModule(IRegistration registration)
        {
            registration.Register<IThreadSafeCommandQueue>().As<ThreadSafeCommandQueue>().Complete();

            registration.Register<ILoopCommand>().As<StartLoopCommand>().WithName(AsyncLoop.START).Complete();
            registration.Register<ILoopCommand>().As<SoftStopLoopCommand>().WithName(AsyncLoop.STOP).Complete();
            registration.Register<ILoopCommand>().As<HardStopLoopCommand>().WithName(AsyncLoop.HARD_STOP).Complete();
            registration.Register<ILoopCommand>().As<EnqueueCommandToLoop>().WithName(AsyncLoop.ENQUEUE).Complete();
            registration.Register<ILoopCommand>().As<WaitForLoopEndCommand>().WithName(AsyncLoop.WAIT_LOOP_END).Complete();
            registration.Register<ILoopCommand>().As<CheckStateCommand>().WithName(AsyncLoop.CHECK_STATE).Complete();
            registration.Register<ILoopCommand>().As<TerminateLoopCommand>().WithName(AsyncLoop.TERMINATE).Complete();

            registration.Register<ICommandExceptionHandler>().As<FakeCommandExceptionHandler>().Complete();
            registration.Register<IAsyncLoop>().As<AsyncLoop>().AsSingleton().Complete();

            registration.Register<CancellationTokenSource>().Complete();
        }
    }
}
