using Lesson8.Code;
using Lesson8.Code.Commands;
using Moq;

namespace Lesson8.Tests
{
    public class Tests: CommandTestsBase
    {
        [Test]
        public void TestExecuting()
        {
            Exception testEx = null;

            var log = CreateLog((ex) => testEx = ex);

            var loggedEx = new Exception();

            var command = new LogExceptionCommand(log, loggedEx);

            Assert.DoesNotThrow(() => command.Execute());
            Assert.That(loggedEx == testEx);
        }

        [Test]
        public void TestCreating()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LogExceptionCommand(CreateLog(), null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                new LogExceptionCommand(null, new Exception());
            });

            Assert.DoesNotThrow(() =>
            {
                new LogExceptionCommand(CreateLog(), new Exception());
            });
        }
    }
}