using Lesson8.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Tests
{
    public class CommandsExecutorTests : CommandTestsBase
    {
        [Test]
        public void TestExecution()
        {
            var counter = 0;


            var queue = CreateQueue();

            var executor = new CommandsExecutor(CreateCommandExceptionHandler((ex) => counter++, queue));

            Assert.Throws<ArgumentNullException>(() => executor.ExecuteCommandsQueue(null));

            queue.Enqueue(CreateCommand(() => throw new Exception()));
            queue.Enqueue(CreateCommand(() => counter++));
            queue.Enqueue(CreateCommand(() => throw CreateHandledException(() => counter++)));

            Assert.DoesNotThrow(() => executor.ExecuteCommandsQueue(queue));
            Assert.That(counter == 3);
        }

        [Test]
        public void TestCreation()
        {
            Assert.DoesNotThrow(() => new CommandsExecutor(CreateCommandExceptionHandler()));
            Assert.Throws<ArgumentNullException>(() => new CommandsExecutor(null));
        }
    }
}
