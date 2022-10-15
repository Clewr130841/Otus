using Lesson5.Code.Commands;
using Lesson8.Code;
using Lesson8.Code.ExceptionHandlers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Tests
{
    public abstract class CommandTestsBase
    {
        public CommandExceptionHandler CreateCommandExceptionHandler()
        {
            return CreateCommandExceptionHandler(default(ILog), null);
        }

        public CommandExceptionHandler CreateCommandExceptionHandler(Action<Exception> logAction = null, ICommandQueue commandQueue = null)
        {
            if (logAction == null)
            {
                logAction = (ex) => { };
            }

            var log = CreateLog(logAction);

            return CreateCommandExceptionHandler(log, commandQueue);
        }

        public CommandExceptionHandler CreateCommandExceptionHandler(ILog log = null, ICommandQueue commandQueue = null)
        {
            if (log == null)
            {
                log = CreateLog();
            }

            if (commandQueue == null)
            {
                commandQueue = CreateQueue();
            }

            var exceptionHandler = new CommandExceptionHandler(log, commandQueue);

            return exceptionHandler;
        }

        public HandledExceptionBase CreateHandledException(Action action = null)
        {
            if (action == null)
            {
                action = () => { };
            }

            var mock = new Mock<HandledExceptionBase>();
            mock.Setup(x => x.HandleException()).Callback(action);

            return mock.Object;
        }

        public ILog CreateLog(Action<Exception> action = null)
        {
            if (action == null)
            {
                action = (ex) => { };
            }

            var mock = new Mock<ILog>();
            mock.Setup(x => x.Log(It.IsAny<Exception>())).Callback<Exception>(action);
            return mock.Object;
        }


        public ICommand CreateCommand(Action action = null)
        {
            if (action == null)
            {
                action = () => { };
            }

            var mock = new Mock<ICommand>();
            mock.Setup(x => x.Execute()).Callback(action);
            return mock.Object;
        }

        public ICommandQueue CreateQueue()
        {
            var queue = new Queue<ICommand>();

            var mock = new Mock<ICommandQueue>();
            mock.Setup(x => x.Dequeue()).Returns(
                () => queue.Dequeue()
            );
            mock.Setup(x => x.Enqueue(It.IsAny<ICommand>())).Callback<ICommand>((comm) => queue.Enqueue(comm));
            mock.SetupGet(x => x.Count).Returns(() => queue.Count);

            return mock.Object;
        }
    }
}
