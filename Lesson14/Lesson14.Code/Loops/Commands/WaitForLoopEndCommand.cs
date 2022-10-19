using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code.Loops.Commands
{
    internal class WaitForLoopEndCommand : LoopCommandBase
    {
        int _timeout;
        public WaitForLoopEndCommand(string loopKey, IContainer container, int timeout = int.MaxValue) : base(loopKey, container)
        {
            _timeout = timeout;
        }

        public override void Execute()
        {
            if (_timeout > 0)
            {
                Thread?.Join(_timeout); // Ждем завершение потока
            }
            else
            {
                Thread?.Join();
            }
        }
    }
}
