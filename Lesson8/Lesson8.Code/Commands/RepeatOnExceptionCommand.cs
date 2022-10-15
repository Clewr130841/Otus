using Lesson5.Code.Commands;
using Lesson8.Code.ExceptionHandlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lesson8.Code.Commands
{
    public class RepeatOnExceptionCommand : ICommand
    {
        int _maxRepetitions;
        ICommand _command;
        ICommandQueue _queue;

        public RepeatOnExceptionCommand(ICommandQueue queue, ICommand command, int maxRepetitions)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (maxRepetitions < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRepetitions));
            }

            _queue = queue;
            _command = command;
            _maxRepetitions = maxRepetitions;
        }

        public void Execute()
        {
            try
            {
                _maxRepetitions--;
                _command.Execute();
            }
            catch (Exception ex)
            {
                if (_maxRepetitions < 0)
                {
                    throw ex;
                }
                else
                {
                    throw new RepeatException(this, _queue);
                }
            }
        }
    }
}
