using Lesson5.Code.Commands;
using Lesson8.Code.Commands;
using Lesson8.Code.ExceptionHandlers;
using System;

namespace Lesson8.Code
{
    public class CommandExceptionHandler : ICommandExceptionHandler
    {
        ICommandQueue _commandQueue;
        ILog _log;

        public CommandExceptionHandler(ILog log, ICommandQueue commandQueue)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            if (commandQueue == null)
            {
                throw new ArgumentNullException(nameof(commandQueue));
            }

            _commandQueue = commandQueue;
            _log = log;
        }

        public void Handle(Exception ex, ICommand command)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (ex is HandledExceptionBase handledEx)
            {
                handledEx.HandleException();
            }
            else
            {
                _commandQueue.Enqueue(new LogExceptionCommand(_log, ex));
            }
        }
    }
}
