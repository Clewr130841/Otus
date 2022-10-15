using Lesson5.Code.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson8.Code
{
    public interface ICommandQueue
    {
        int Count { get; }
        ICommand Dequeue();
        void Enqueue(ICommand command);
    }
}