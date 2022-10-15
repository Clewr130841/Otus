using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson8.Code.ExceptionHandlers
{
    public class RepeatException : HandledExceptionBase
    {
        public ICommand _command;
        public ICommandQueue _commandQueue;
        public RepeatException(ICommand command, ICommandQueue commandQueue)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (commandQueue == null)
            {
                throw new ArgumentNullException(nameof(commandQueue));
            }

            _command = command;
            _commandQueue = commandQueue;
        }

        public override void HandleException()
        {
            _commandQueue.Enqueue(_command);
        }
    }
}
