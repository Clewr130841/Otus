using Lesson9.Code;
using Lesson9.Code.Scopes;

namespace Lesson9.Tests
{
    public class ContainerTests
    {
        IContainer _container;

        [SetUp]
        public void Setup()
        {
            _container = new Container();
            //��� ����������� ��� ���������
            var registrator = _container.Resolve<IRegistration>();

            //��� �������� value �����
            registrator.Register<int>()
                       .AsSingleton(() => 1) //�� ������ ����� ����� ���������, �.�. ValueTypes ����� ������������, �� ��� �� �����
                       .Complete();

            registrator.Register<long>()
                       .As(() => 2L)
                       .Complete();

            registrator.Register<float>() //��� ������ ���������� ��������� ��������
                       .Complete();

            registrator.Register<ulong>()
                       .As((c) => (ulong)c.Resolve<long>())
                       .Complete();

            //��� �������� ref �����
            registrator.Register<IList<int>>() //��������� ������ ��� ��������
                       .As<List<int>>()
                       .AsSingleton()
                       .Complete();

            registrator.Register<List<int>>() //��� ������ ��� ����� ���������
                       .Complete();

            registrator.Register<TestClass>() //��� ��� ��������������� ���������
                       .Complete();

            registrator.Register<TestClass>()
                       .WithName("TestClass")
                       .Complete();
        }

        //�� ��� ������ ������������� �����, �������� ���
        [Test]
        public void TestLifeTimeScopes()
        {
            //������ �������� ������
            Parallel.For(5, 105, (i) =>
            {
                //������� �����
                using (var scope = _container.Resolve<ILifetimeScope>())
                {
                    //���������������� � ��� ������ �����
                    scope.Resolve<IRegistration>().Register<long>().AsSingleton(() => (long)i).Complete();

                    //��� ����� ������������ � ���� ���� ������� ���������� �������� ��� �������
                    Thread.Sleep(100);

                    //���� � ulong ������ ���������, �.�. ulong ������� �� ����� IoC �� ���� Long
                    //����� ��������� ������� ������������
                    Assert.That(i == (int)scope.Resolve<ulong>());

                    //��� �� ����� ���� �����, �.�. ����� �����
                    Assert.That(scope.Resolve<ulong>() != _container.Resolve<ulong>());
                }
            });
        }

        [Test]
        public void TestThreadScopes()
        {
            //���, ���� �� ������� � �����, ����� �������� -1
            _container.Resolve<IRegistration>().Register<double>().As(_ => -1d).Complete();

            //������ �������� ������
            Parallel.For(0, 1000, (i) =>
            {
                //������� �����
                var scope = _container.Resolve<IThreadScope>();
                scope.Resolve<IRegistration>().Register<double>().As(_ => (double)Thread.CurrentThread.ManagedThreadId).Complete();
            });

            Parallel.For(0, 1000, (i) =>
            {
                //������� �����
                var scope = _container.Resolve<IThreadScope>();
                Assert.That((int)scope.Resolve<double>() == Thread.CurrentThread.ManagedThreadId || scope.Resolve<double>() == -1d);
            });
        }

        [Test]
        public void TestNamedScopes()
        {
            Parallel.For(0, 100, (i) =>
            {
                var namedScope = _container.Resolve<INamedScope>().WithName(i.ToString());
                namedScope.Resolve<IRegistration>().Register<uint>().AsSingleton(() => (uint)i).Complete();
            });

            Parallel.For(0, 100, (i) =>
            {
                var namedScope = _container.Resolve<INamedScope>().WithName(i.ToString());
                Assert.That(namedScope.Resolve<uint>() == (uint)i);
            });
        }

        //������ value types
        [Test]
        public void TestValueTypes()
        {
            //������ ������� ��������
            Assert.That(_container.Resolve<int>() == 1);
            Assert.That(_container.Resolve<long>() == 2);
            Assert.That(_container.Resolve<float>() == 0f);
            Assert.That(_container.Resolve<ulong>() == (ulong)(long)_container.Resolve(typeof(long)));

            //������ ���������������
            _container.Resolve<IRegistration>()
                      .Register<long>()
                      .As(() => 4L)
                      .Complete();

            //����� ��������������� ���� ������ ����� ������� ����� ��������, � �.�. ulong �������� ��� ������ ���������, �������� ������ ���� �����
            Assert.That(_container.Resolve<ulong>() == (ulong)(long)_container.Resolve(typeof(long)));

            //������ �������, ��� ��������� ������
            _container.Resolve<IRegistration>()
                      .Register<long>()
                      .As(() => 2L)
                      .Complete();
        }

        public class TestClass
        {
            public IList<int> List { get; set; }
            public IContainer Container { get; set; }

            public TestClass(IList<int> list, IContainer container)
            {
                List = list;
                Container = container;
            }
        }

        [Test]
        public void TestReferenceTypes()
        {
            //��� ������ IList, �� ������� ��� ��������
            _container.Resolve<IList<int>>().Add(0);
            _container.Resolve<IList<int>>().Add(1);

            Assert.That(_container.Resolve<IList<int>>().Count == 2);

            for (var i = 0; i < _container.Resolve<IList<int>>().Count; i++)
            {
                Assert.That(_container.Resolve<IList<int>>()[i] == i);
            }

            //� ��� ���� �� ������� ��� ��������
            _container.Resolve<List<int>>().Add(0);

            Assert.That(_container.Resolve<List<int>>().Count == 0);

            //������ �������� ����������
            Assert.That(_container.Resolve<List<int>>(10).Capacity == 10);

            //������ �������������, ��������� ������ ��������� ��� ��������� ���
            Assert.That(_container.Resolve<TestClass>().List == _container.Resolve<IList<int>>());
            Assert.That(_container.Resolve<TestClass>().Container == _container);
        }

        [Test]
        public void TestGetByName()
        {
            var testClass = _container.Resolve<TestClass>("TestClass");
            Assert.That(testClass != null);
        }
    }
}