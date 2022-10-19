
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

            //������ ������ ����
            _container.Resolve<IRegistration>().RegisterModule(new AsyncLoopModule());
        }


        /// <summary>
        /// 5. �������� ����, ������� ���������, ��� ����� ������� ����� ����� ������� - 1���� � 
        /// 4 ����� - ���� ������������ �������� ������� �������������.
        /// </summary>
        [Test]
        public void TestCheckAfterStart()
        {
            object[] testLoopKey = new[] { "some loop" };

            var x = 0;

            //������� ��� ������ �� ������������, �� ����� ������ Enqueue ��� Start ������ ������� � Init
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.NotExists);

            var ewh = new EventWaitHandle(false, EventResetMode.ManualReset);

            Thread loopThread = null;

            var commXCrossCoss = new ActionCommand(() =>
            {
                x++;
                loopThread = Thread.CurrentThread;
                ewh.Set(); //� ��� � +4 ����� �� �������������, ��� ������ ������� ������ � CancelationToken
            });

            _container.Resolve<IAsyncLoop>(testLoopKey).Enqueue(commXCrossCoss);

            //��� ��� ������ ������� � init
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.Init);

            //��������� ���
            _container.Resolve<IAsyncLoop>(testLoopKey).Start();

            //���� ��������� ��������
            ewh.WaitOne();

            //���������, ��� ��� ������������� ����������� � ������ �����
            Assert.That(loopThread != null && loopThread != Thread.CurrentThread);

            //��������� ������ ���� Running

            Assert.That(x == 1);

            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.Running);

            //6. �������� ����, ������� ���������, ��� ����� ������� hard stop, ����� ����������� - 1 ����

            //���������� Event wait handle
            ewh.Reset();


            _container.Resolve<IAsyncLoop>(testLoopKey).Enqueue((token) =>
            {
                ewh.Set();
                try
                {
                    x = -1;
                    //������ ����� ����, ������� ����� ����� �������� ������ � ������� CancelationToken
                    Task.Delay(TimeSpan.FromDays(2), token).Wait();
                }
                catch (Exception ex) //����� ������ 
                {
                    x--;
                }
            });

            //���� ����� �������� ������ �����, ����� �������� �� ����������
            ewh.WaitOne();

            //������ ��������
            _container.Resolve<IAsyncLoop>(testLoopKey).StopHard();

            //����, ����� ��� ����������
            _container.Resolve<IAsyncLoop>(testLoopKey).Wait();

            Assert.That(x == -2);
            //��������� ���������
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.Stopped);

            //������� ������� ����
            _container.Resolve<IAsyncLoop>(testLoopKey).Terminate();

            //��������� ���������
            Assert.That(_container.Resolve<IAsyncLoop>(testLoopKey).GetState() == LoopStateEnum.NotExists);

            x = 0;

            //�������� ����, ������� ���������, ��� ����� ������� soft stop,
            //����� ����������� ������ ����� ����, ��� ��� ������ ����������� - 2 �����
            for (var i = 0; i < 100; i++)
            {
                _container.Resolve<IAsyncLoop>(testLoopKey).Enqueue((token) =>
                {
                    Thread.Sleep(2);
                    x++;
                });
            }

            //���������
            _container.Resolve<IAsyncLoop>(testLoopKey).Start();
            //������������� �����
            _container.Resolve<IAsyncLoop>(testLoopKey).Stop();
            //����, ����� ��� ����������
            _container.Resolve<IAsyncLoop>(testLoopKey).Wait();

            Assert.AreEqual(x, 100);
        }
    }
}