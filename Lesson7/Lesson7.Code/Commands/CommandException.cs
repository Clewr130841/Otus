using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class CommandException : Exception
    {
        public CommandException(string message) : base(message)
        {
        }

        public CommandException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
