using Lesson5.Code.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson14.Code
{
    public class ThreadSafeCommandQueue : IThreadSafeCommandQueue
    {
        SemaphoreSlim _semaphore;
        ConcurrentQueue<ICommand> _commandQueue;
        public ThreadSafeCommandQueue()
        {
            _semaphore = new SemaphoreSlim(0);
            _commandQueue = new ConcurrentQueue<ICommand>();
        }

        public int Count => _commandQueue.Count;

        public ICommand Dequeue(CancellationToken cancellationToken)
        {
            _semaphore.Wait(cancellationToken);
            _commandQueue.TryDequeue(out var result);
            return result;
        }

        public ICommand Dequeue()
        {
            _semaphore.Wait();
            _commandQueue.TryDequeue(out var result);
            return result;
        }

        public void Enqueue(ICommand command)
        {
            _commandQueue.Enqueue(command);
            _semaphore.Release();
        }
    }
}
