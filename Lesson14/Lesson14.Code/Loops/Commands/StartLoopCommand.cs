using Lesson5.Code.Commands;
using Lesson8.Code;
using Lesson9.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    public class StartLoopCommand : LoopCommandBase
    {
        ICommandExceptionHandler _commandExceptionHandler;
        IContainer _container;
        string _loopKey;
        CancellationTokenSource _cancellationToken;
        public StartLoopCommand(string loopKey, IContainer container, ICommandExceptionHandler commandExceptionHandler, CancellationTokenSource cancellationToken) : base(loopKey, container)
        {
            _loopKey = loopKey;
            _commandExceptionHandler = commandExceptionHandler;
            _cancellationToken = cancellationToken;
        }

        public override void Execute()
        {
            // Тут сильной потокобезопасности делать не буду
            if (State > LoopStateEnum.Init)
            {
                throw new Exception($"Loop with key {_loopKey} is already run");
            }

            InitQueueAndToken();

            var thread = new Thread(Loop)
            {
                IsBackground = true,
                Name = $"loop: {_loopKey}",
            };

            Thread = thread;
            thread.Start();
        }

        private void Loop()
        {
            try
            {
                State = LoopStateEnum.Running;

                while (State == LoopStateEnum.Running)
                {
                    ICommand command = null;

                    try
                    {
                        command = Queue.Dequeue(_cancellationToken.Token);
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        _commandExceptionHandler.Handle(ex, command);
                    }
                }
            }
            finally
            {
                //Убиваем ресурсы занятые лупом
                State = LoopStateEnum.Stopped;
            }
        }
    }
}
