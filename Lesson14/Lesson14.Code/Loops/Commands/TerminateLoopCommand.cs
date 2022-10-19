using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    public class TerminateLoopCommand: LoopCommandBase
    {
        public TerminateLoopCommand(string loopKey, IContainer container) : base(loopKey, container)
        {
        }

        public override void Execute()
        {
            Queue = null;
            Thread = null;
            State = LoopStateEnum.NotExists;
        }
    }
}
