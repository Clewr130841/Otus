using Lesson14.Code.Loops.Commands;
using Lesson5.Code.Commands;
using Lesson9.Code;
using Lesson9.Code.Scopes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops
{
    /// <summary>
    /// Обертка для лупа
    /// </summary>
    public class AsyncLoop : IAsyncLoop
    {
        public const string START = "start";
        public const string ENQUEUE = "enqueue";
        public const string STOP = "stop";
        public const string HARD_STOP = "stop hard";
        public const string WAIT_LOOP_END = "wait";
        public const string CHECK_STATE = "check state";
        public const string TERMINATE = "terminate";

        IContainer _container;
        string _loopKey;
        CancellationTokenSource _cancellationTokenSource;
        public AsyncLoop(string loopKey, IContainer container)
        {
            if (loopKey == null)
            {
                throw new ArgumentNullException(nameof(loopKey));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _cancellationTokenSource = new CancellationTokenSource();

            _container = container.Resolve<ILifetimeScope>(); // Для лупа лучше сделать свой скоуп

            _container.Resolve<IRegistration>()
                .Register<CancellationTokenSource>()
                .As(() => _cancellationTokenSource)
                .Complete();

            _loopKey = loopKey;
        }

        public AsyncLoop(IContainer container) : this(Guid.NewGuid().ToString(), container)
        {

        }

        public void Start()
        {
            _container.Resolve<ILoopCommand>(START, new object[] { _loopKey, _cancellationTokenSource }).Execute();
        }

        public void Stop()
        {
            _container.Resolve<ILoopCommand>(STOP, new object[] { _loopKey }).Execute();
        }

        public void StopHard()
        {
            _container.Resolve<ILoopCommand>(HARD_STOP, new object[] { _loopKey, _cancellationTokenSource }).Execute();
        }

        public void Enqueue(Action<CancellationToken> action)
        {
            Enqueue(new ActionCommand(action, _cancellationTokenSource));
        }

        public void Enqueue(ICommand command)
        {
            _container.Resolve<ILoopCommand>(ENQUEUE, new object[] { _loopKey, command }).Execute();
        }

        public void Wait(int? timeout = null)
        {
            if (timeout.HasValue)
            {
                _container.Resolve<ILoopCommand>(WAIT_LOOP_END, new object[] { _loopKey, timeout.Value }).Execute();
            }
            else
            {
                _container.Resolve<ILoopCommand>(WAIT_LOOP_END, new object[] { _loopKey }).Execute();
            }
        }

        public LoopStateEnum GetState()
        {
            var result = LoopStateEnum.NotExists;

            _container.Resolve<ILoopCommand>(CHECK_STATE, new object[] {
                _loopKey,
                new Action<LoopStateEnum>((state) =>
                {
                    result = state;
                })
            }).Execute();

            return result;
        }

        public void Terminate()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _container.Resolve<ILoopCommand>(TERMINATE, new object[] { _loopKey }).Execute();
        }
    }
}
