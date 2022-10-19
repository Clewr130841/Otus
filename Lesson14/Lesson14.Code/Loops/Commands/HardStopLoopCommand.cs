using Lesson5.Code.Commands;
using Lesson9.Code;
using System;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    public class HardStopLoopCommand : LoopCommandBase
    {
        CancellationTokenSource _cancellationToken;
        public HardStopLoopCommand(string loopKey, IContainer container, CancellationTokenSource cancellationToken) : base(loopKey, container)
        {
            _cancellationToken = cancellationToken;
        }

        public override void Execute()
        {
            State = LoopStateEnum.Stopping;
            _cancellationToken.Cancel();
        }
    }
}
