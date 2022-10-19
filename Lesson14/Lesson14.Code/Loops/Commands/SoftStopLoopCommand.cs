using Lesson5.Code.Commands;
using Lesson9.Code;
using System;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    public class SoftStopLoopCommand : LoopCommandBase
    {
        public SoftStopLoopCommand(string loopKey, IContainer container) : base(loopKey, container)
        {
        }

        public override void Execute()
        {
            CheckExistance();
            //Ставим комманду остановки в конец очереди
            Queue.Enqueue(new ActionCommand(() =>
            {
                State = LoopStateEnum.Stopping;
            }));
        }
    }
}
