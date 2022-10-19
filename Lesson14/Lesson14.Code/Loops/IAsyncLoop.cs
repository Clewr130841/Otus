using Lesson14.Code.Loops.Commands;
using Lesson5.Code.Commands;
using System;
using System.Threading;

namespace Lesson14.Code.Loops
{
    public interface IAsyncLoop
    {
        void Enqueue(Action<CancellationToken> action);
        void Enqueue(ICommand command);
        LoopStateEnum GetState();
        void Start();
        void Stop();
        void StopHard();
        void Terminate();
        void Wait(int? timeout = null);
    }
}