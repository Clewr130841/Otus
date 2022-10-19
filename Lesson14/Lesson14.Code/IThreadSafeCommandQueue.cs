using Lesson5.Code.Commands;
using Lesson8.Code;
using System.Threading;

namespace Lesson14.Code
{
    public interface IThreadSafeCommandQueue : ICommandQueue
    {
        ICommand Dequeue(CancellationToken cancellationToken);
    }
}