using Lesson5.Code.Commands;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    /// <summary>
    /// Базовый класс для луп комманд
    /// </summary>
    public abstract class LoopCommandBase : ILoopCommand
    {
        string _loopKey;
        IContainer _container;
        public LoopCommandBase(string loopKey, IContainer container)
        {
            if (loopKey == null)
            {
                throw new ArgumentNullException(nameof(loopKey));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _loopKey = $"loops:{loopKey}";
            _container = container;
        }

        protected string GetLoopKey(string suffix)
        {
            return $"{_loopKey}:{suffix}";
        }

        protected void CheckExistance()
        {
            if (State == LoopStateEnum.NotExists)
            {
                throw new Exception($"Loop with key {_loopKey} is already exists");
            }
        }

        protected void InitQueueAndToken()
        {
            Queue = Queue ?? _container.Resolve<IThreadSafeCommandQueue>();
            State = LoopStateEnum.Init;
        }

        protected LoopStateEnum State
        {
            get
            {
                var key = GetLoopKey(nameof(Queue));

                if (_container.CanResolve<LoopStateEnum>(key))
                {
                    return _container.Resolve<LoopStateEnum>(key);
                }

                return LoopStateEnum.NotExists;
            }
            set
            {
                var key = GetLoopKey(nameof(Queue));

                if (value == LoopStateEnum.NotExists)
                {
                    _container.Resolve<IRegistration>()
                        .Unregister<LoopStateEnum>(key);
                }
                else
                {
                    _container.Resolve<IRegistration>()
                        .Register<LoopStateEnum>()
                        .WithName(GetLoopKey(nameof(Queue)))
                        .As(() => value)
                        .Complete();
                }
            }
        }

        protected Thread Thread
        {
            get
            {
                var key = GetLoopKey(nameof(Thread));

                if (_container.CanResolve<Thread>(key))
                {
                    return _container.Resolve<Thread>(key);
                }

                return null;
            }
            set
            {
                var key = GetLoopKey(nameof(Thread));

                if (value == null)
                {
                    _container.Resolve<IRegistration>().Unregister<Thread>(key);
                }
                else
                {
                    _container.Resolve<IRegistration>()
                        .Register<Thread>()
                        .WithName(key)
                        .As(() => value)
                        .Complete();
                }
            }
        }

        protected IThreadSafeCommandQueue Queue
        {
            get
            {
                var key = GetLoopKey(nameof(Queue));

                if (_container.CanResolve<IThreadSafeCommandQueue>(key))
                {
                    return _container.Resolve<IThreadSafeCommandQueue>(key);
                }

                return null;
            }
            set
            {
                var key = GetLoopKey(nameof(Queue));

                if (value == null)
                {
                    _container.Resolve<IRegistration>()
                        .Unregister<IThreadSafeCommandQueue>(key);
                }
                else
                {
                    _container.Resolve<IRegistration>()
                        .Register<IThreadSafeCommandQueue>()
                        .WithName(key)
                        .As(() => value)
                        .Complete();
                }
            }
        }

        public abstract void Execute();
    }
}
