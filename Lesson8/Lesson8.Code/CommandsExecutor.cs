using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson8.Code
{
    public class CommandsExecutor : ICommandExecutor
    {
        ICommandExceptionHandler _exceptionHandler;

        public CommandsExecutor(ICommandExceptionHandler exceptionHandler)
        {
            if (exceptionHandler == null)
            {
                throw new ArgumentNullException(nameof(exceptionHandler));
            }

            _exceptionHandler = exceptionHandler;
        }


        public void ExecuteCommandsQueue(ICommandQueue commandQueue)
        {
            if (commandQueue == null)
            {
                throw new ArgumentNullException(nameof(commandQueue));
            }

            while (commandQueue.Count > 0)
            {
                var currentCommand = commandQueue.Dequeue();

                try
                {
                    currentCommand.Execute();
                }
                catch (Exception ex)
                {
                    _exceptionHandler.Handle(ex, currentCommand);
                }
            }
        }
    }
}
