using Lesson8.Code;
using Lesson8.Code.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Tests
{
    public class Point8Tests : CommandTestsBase
    {
        /// <summary>
        /// Пункт 8 решил реализовать в виде тестов
        /// С помощью Команд из пункта 4 и пункта 6 реализовать следующую обработку исключений:
        /// при первом выбросе исключения повторить команду, при повторном выбросе исключения записать информацию в лог.
        /// </summary>
        [Test]
        public void Test()
        {
            var ex = new Exception();
            var exceptionsCounter = 0;
            var executionsCounter = 0;

            var log = CreateLog(
                (ex) => exceptionsCounter++
             );

            var queue = CreateQueue();

            var executor = new CommandsExecutor(CreateCommandExceptionHandler(log, queue));

            queue.Enqueue(new RepeatOnExceptionCommand(queue, CreateCommand(() =>
            {
                executionsCounter++;
                throw new Exception();
            }), 1));

            Assert.DoesNotThrow(() => executor.ExecuteCommandsQueue(queue));

            Assert.IsTrue(exceptionsCounter == 1);
            Assert.IsTrue(executionsCounter == 2);
        }
    }
}
