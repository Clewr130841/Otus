using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    public class CheckStateCommand : LoopCommandBase
    {
        Action<LoopStateEnum> _action;
        public CheckStateCommand(string loopKey, IContainer container, Action<LoopStateEnum> action) : base(loopKey, container)
        {
            _action = action;
        }

        public override void Execute()
        {
            _action.Invoke(State);
        }
    }
}
