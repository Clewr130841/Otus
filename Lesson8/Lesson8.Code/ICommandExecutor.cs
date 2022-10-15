using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson8.Code
{
    public interface ICommandExecutor
    {
        public void ExecuteCommandsQueue(ICommandQueue commandQueue);
    }
}
