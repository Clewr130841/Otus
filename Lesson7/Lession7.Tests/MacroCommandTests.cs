using Lesson5.Code.Commands;
using Lesson7.Code;
using Lesson7.Code.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lession7.Tests
{
    public class MacroCommandTests
    {
        [Test]
        public void TestNotThrow()
        {
            var commandsExecuted = 0;

            var fakeMock = new Mock<ICommand>();
            fakeMock.Setup(x => x.Execute()).Callback(() => commandsExecuted++);

            var commands = new ICommand[]
            {
                fakeMock.Object, fakeMock.Object
            };

            var command = new MacroCommand(commands);

            Assert.DoesNotThrow(() => command.Execute());
            Assert.That(commands.Length == commandsExecuted);
        }

        private class ExecutedCommands
        {
            public int Count { get; set; }
        }

        [Test]
        public void TestThrow()
        {
            var executedCommands = new ExecutedCommands();

            var fakeMock = new Mock<ICommand>();
            fakeMock.Setup(x => x.Execute()).Callback(
                () => executedCommands.Count++
            );

            var exMock = new Mock<ICommand>();
            exMock.Setup(x => x.Execute()).Callback(() => throw new Exception());

            var commands = new ICommand[]
            {
                fakeMock.Object,
                fakeMock.Object,
                exMock.Object
            };

            var command = new MacroCommand(commands);

            Assert.Throws<MacroCommandException>(() => command.Execute());
            Assert.That(commands.Length - 1 == executedCommands.Count);
        }

        [Test]
        public void NullReferenceTest()
        {
            Assert.Throws<ArgumentNullException>(() => new MacroCommand(null));
        }
    }
}
