
using Lesson14.Code;
using Lesson14.Code.Loops;
using Lesson14.Code.Loops.Commands;
using Lesson9.Code;

namespace Lesson14.Tests
{
    public class Tests
    {
        IContainer _container;


        [SetUp]
        public void Setup()
        {
            _container = new Container();

            //Регаем модуль лупа
            _container.Resolve<IRegistration>().RegisterModule(new AsyncLoopModule());
        }


        /// <summary>
        /// 5. Написать тест, который проверяет, что после команды старт поток запущен - 1балл и 
        /// 4 балла - если используются условные события синхронизации.
        /// </summary>
        [Test]
        public void TestCheckAfterStart()
        {
            object[] testLoopKey = new[] { "some loop" };

            var x = 0;

            //Сначала луп должен не существовать, но после вызова Enqueue или Start должен перейти в Init
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.NotExists);

            var ewh = new EventWaitHandle(false, EventResetMode.ManualReset);

            Thread loopThread = null;

            var commXCrossCoss = new ActionCommand(() =>
            {
                x++;
                loopThread = Thread.CurrentThread;
                ewh.Set(); //А вот и +4 балла за синхронизацию, еще внутри семафор кстати и CancelationToken
            });

            _container.Resolve<IAsyncLoop>(testLoopKey).Enqueue(commXCrossCoss);

            //Тут уже должен перейти в init
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.Init);

            //Запускаем луп
            _container.Resolve<IAsyncLoop>(testLoopKey).Start();

            //Ждем выполения комманды
            ewh.WaitOne();

            //Проверяем, что все действительно выполнилось в другом треде
            Assert.That(loopThread != null && loopThread != Thread.CurrentThread);

            //Состояние должно быть Running

            Assert.That(x == 1);

            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.Running);

            //6. Написать тест, который проверяет, что после команды hard stop, поток завершается - 1 балл

            //Сбрасываем Event wait handle
            ewh.Reset();


            _container.Resolve<IAsyncLoop>(testLoopKey).Enqueue((token) =>
            {
                ewh.Set();
                try
                {
                    x = -1;
                    //Ставим такой срок, который можно будет сбросить только с помощью CancelationToken
                    Task.Delay(TimeSpan.FromDays(2), token).Wait();
                }
                catch (Exception ex) //Ловим ошибку 
                {
                    x--;
                }
            });

            //Ждем когда комманда начнет ждать, чтобы отменить ее хардстопом
            ewh.WaitOne();

            //Теперь хардстоп
            _container.Resolve<IAsyncLoop>(testLoopKey).StopHard();

            //Ждем, когда все закончится
            _container.Resolve<IAsyncLoop>(testLoopKey).Wait();

            Assert.That(x == -2);
            //Проверяем состояние
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.Stopped);

            //Удаляем ресурсы лупа
            _container.Resolve<IAsyncLoop>(testLoopKey).Terminate();

            //Проверяем состояние
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.NotExists);

            x = 0;

            //Написать тест, который проверяет, что после команды soft stop,
            //поток завершается только после того, как все задачи закончились - 2 балла
            for (var i = 0; i < 100; i++)
            {
                _container.Resolve<IAsyncLoop>(testLoopKey).Enqueue((token) =>
                {
                    Thread.Sleep(2);
                    x++;
                });
            }

            //Запускаем
            _container.Resolve<IAsyncLoop>(testLoopKey).Start();
            //Останавливаем мягко
            _container.Resolve<IAsyncLoop>(testLoopKey).Stop();
            //Ждем, когда все отработает
            _container.Resolve<IAsyncLoop>(testLoopKey).Wait();

            Assert.AreEqual(x, 100);
        }
    }
}