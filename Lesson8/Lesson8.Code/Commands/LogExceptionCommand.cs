using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson8.Code.Commands
{
    public class LogExceptionCommand : ICommand
    {
        ILog _log;
        Exception _ex;

        public LogExceptionCommand(ILog log, Exception ex)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            _log = log;
            _ex = ex;
        }

        public void Execute()
        {
            _log.Log(_ex);
        }
    }
}
