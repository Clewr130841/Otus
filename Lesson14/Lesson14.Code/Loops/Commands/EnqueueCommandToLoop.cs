using Lesson5.Code.Commands;
using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    public class EnqueueCommandToLoop : LoopCommandBase
    {
        ICommand _command;

        public EnqueueCommandToLoop(string loopKey, ICommand command, IContainer container) : base(loopKey, container)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _command = command;
        }

        public override void Execute()
        {
            InitQueueAndToken();
            Queue.Enqueue(_command);
        }
    }
}
