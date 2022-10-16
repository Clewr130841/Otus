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
            //Тут настраиваем наш контейнер
            var registrator = _container.Resolve<IRegistration>();

            //Для проверки value типов
            registrator.Register<int>()
                       .AsSingleton(() => 1) //Не совсем имеет смысл синглтона, т.к. ValueTypes будут копироваться, то тем не менее
                       .Complete();

            registrator.Register<long>()
                       .As(() => 2L)
                       .Complete();

            registrator.Register<float>() //Тут должно получиться дефолтное значение
                       .Complete();

            registrator.Register<ulong>()
                       .As((c) => (ulong)c.Resolve<long>())
                       .Complete();

            //Для проверки ref типов
            registrator.Register<IList<int>>() //Интерфейс регаем как синглтон
                       .As<List<int>>()
                       .AsSingleton()
                       .Complete();

            registrator.Register<List<int>>() //Тип регаем как много инстансов
                       .Complete();

            registrator.Register<TestClass>() //Тип для автоподстановки инстансов
                       .Complete();

            registrator.Register<TestClass>()
                       .WithName("TestClass")
                       .Complete();
        }

        //Хз как писать многопоточные тесты, наверное так
        [Test]
        public void TestLifeTimeScopes()
        {
            //Тестим лайфтайм скоупы
            Parallel.For(5, 105, (i) =>
            {
                //Создаем скоуп
                using (var scope = _container.Resolve<ILifetimeScope>())
                {
                    //Перерегестрируем в нем резолв лонга
                    scope.Resolve<IRegistration>().Register<long>().AsSingleton(() => (long)i).Complete();

                    //Тут лучше притормозить и дать всем потокам установить значения для скоупов
                    Thread.Sleep(100);

                    //Лонг и ulong должны совпадать, т.к. ulong берется из через IoC из типа Long
                    //Сразу тестируем цепочку зависимостей
                    Assert.That(i == (int)scope.Resolve<ulong>());

                    //Тут не должы быть равны, т.к. новый скоуп
                    Assert.That(scope.Resolve<ulong>() != _container.Resolve<ulong>());
                }
            });
        }

        [Test]
        public void TestThreadScopes()
        {
            //Тут, если не попадем в поток, будет значение -1
            _container.Resolve<IRegistration>().Register<double>().As(_ => -1d).Complete();

            //Тестим лайфтайм скоупы
            Parallel.For(0, 1000, (i) =>
            {
                //Создаем скоуп
                var scope = _container.Resolve<IThreadScope>();
                scope.Resolve<IRegistration>().Register<double>().As(_ => (double)Thread.CurrentThread.ManagedThreadId).Complete();
            });

            Parallel.For(0, 1000, (i) =>
            {
                //Создаем скоуп
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

        //Тестим value types
        [Test]
        public void TestValueTypes()
        {
            //Тестим простую проверку
            Assert.That(_container.Resolve<int>() == 1);
            Assert.That(_container.Resolve<long>() == 2);
            Assert.That(_container.Resolve<float>() == 0f);
            Assert.That(_container.Resolve<ulong>() == (ulong)(long)_container.Resolve(typeof(long)));

            //Тестим перерегистрацию
            _container.Resolve<IRegistration>()
                      .Register<long>()
                      .As(() => 4L)
                      .Complete();

            //После перерегистрации лонг должен будет вернуть новое значение, а т.к. ulong вернется при помощи резолвера, значения должны быть равны
            Assert.That(_container.Resolve<ulong>() == (ulong)(long)_container.Resolve(typeof(long)));

            //Вернем обратно, для остальных тестов
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
            //Тут тестим IList, он зареган как синглтон
            _container.Resolve<IList<int>>().Add(0);
            _container.Resolve<IList<int>>().Add(1);

            Assert.That(_container.Resolve<IList<int>>().Count == 2);

            for (var i = 0; i < _container.Resolve<IList<int>>().Count; i++)
            {
                Assert.That(_container.Resolve<IList<int>>()[i] == i);
            }

            //А вот лист не зареган как синглтон
            _container.Resolve<List<int>>().Add(0);

            Assert.That(_container.Resolve<List<int>>().Count == 0);

            //Тестим передачу параметров
            Assert.That(_container.Resolve<List<int>>(10).Capacity == 10);

            //Тестим автопараметры, контейнер должен заполнить все аргументы сам
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