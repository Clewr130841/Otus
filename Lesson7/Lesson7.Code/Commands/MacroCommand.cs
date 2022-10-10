using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson7.Code.Commands
{
    public class MacroCommand : ICommand
    {
        ICommand[] _commands;

        public MacroCommand(params ICommand[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            _commands = commands;
        }

        public void Execute()
        {
            foreach (var command in _commands)
            {
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    throw new MacroCommandException(command, ex);
                }
            }
        }
    }
}
