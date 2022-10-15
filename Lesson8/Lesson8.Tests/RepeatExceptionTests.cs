using Lesson8.Code.Commands;
using Lesson8.Code.ExceptionHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Tests
{
    public class RepeatExceptionTests : CommandTestsBase
    {
        [Test]
        public void TestExecuting()
        {
            var queue = CreateQueue();
            var comm = CreateCommand();
            var ex = new RepeatException(comm, queue);

            Assert.DoesNotThrow(() => ex.HandleException());
            var comm2 = queue.Dequeue();
            Assert.That(comm2 == comm);
            Assert.That(queue.Count == 0);
        }

        [Test]
        public void TestCreating()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RepeatException(null, CreateQueue());
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                new RepeatException(CreateCommand(), null);
            });

            Assert.DoesNotThrow(() =>
            {
                new RepeatException(CreateCommand(), CreateQueue());
            });
        }
    }
}
