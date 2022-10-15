using Lesson5.Code.Commands;
using Lesson8.Code;
using Lesson8.Code.Commands;
using Lesson8.Code.ExceptionHandlers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Tests
{
    public class RepeatOnExceptionCommandTests : CommandTestsBase
    {
        [Test]
        public void TestExecuting()
        {
            var quque = CreateQueue();

            var comm = new RepeatOnExceptionCommand(quque, CreateCommand(() =>
            {
                throw new Exception();
            }), 2);

            Assert.Throws<RepeatException>(() => comm.Execute());
            Assert.Throws<RepeatException>(() => comm.Execute());
            Assert.Throws<Exception>(() => comm.Execute());

            comm = new RepeatOnExceptionCommand(quque, CreateCommand(), 1);

            Assert.DoesNotThrow(() => { comm.Execute(); });
        }

        [Test]
        public void TestCreating()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RepeatOnExceptionCommand(null, CreateCommand(), 1);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                new RepeatOnExceptionCommand(CreateQueue(), null, 1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new RepeatOnExceptionCommand(CreateQueue(), CreateCommand(), 0);
            });

            Assert.DoesNotThrow(() =>
            {
                new RepeatOnExceptionCommand(CreateQueue(), CreateCommand(), 1);
            });
        }
    }
}
