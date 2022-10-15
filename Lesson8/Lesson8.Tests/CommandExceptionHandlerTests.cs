using Lesson8.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Tests
{
    public class CommandExceptionHandlerTests : CommandTestsBase
    {
        [Test]
        public void TestExecuting()
        {
            var queue = CreateQueue();
            var exceptionHandler = CreateCommandExceptionHandler(null as Action<Exception>, queue);
            var testValue = false;

            Assert.DoesNotThrow(() =>
            {
                exceptionHandler.Handle(CreateHandledException(() => testValue = true), CreateCommand());
            });

            Assert.That(testValue);
            Assert.Throws<ArgumentNullException>(() => { exceptionHandler.Handle(null, CreateCommand()); });
            Assert.Throws<ArgumentNullException>(() => { exceptionHandler.Handle(CreateHandledException(() => testValue = true), null); });
        }

        [Test]
        public void TestCreation()
        {
            Assert.DoesNotThrow(() => new CommandExceptionHandler(CreateLog(), CreateQueue()));

            Assert.Throws<ArgumentNullException>(() => new CommandExceptionHandler(null, CreateQueue()));
            Assert.Throws<ArgumentNullException>(() => new CommandExceptionHandler(CreateLog(), null));
        }
    }
}
