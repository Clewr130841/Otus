using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class MacroCommandException : CommandException
    {
        public ICommand Command { get; private set; }

        public MacroCommandException(ICommand command, Exception exception) : base(exception.Message, exception)
        {
            Command = command;
        }
    }
}
