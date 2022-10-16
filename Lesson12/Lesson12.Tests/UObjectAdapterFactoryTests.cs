using Lesson12.Code.Compilation;
using Lesson12.Code;
using Lesson5.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson9.Code;
using System.Numerics;
using System.Collections;

namespace Lesson12.Tests
{
    public class UObjectAdapterFactoryTests
    {
        IRuntimeCompiler _compiler;
        IContainer _container;
        IUObjectAdapterFactory _factory;
        IUObject _uObject;

        [SetUp]
        public void Setup()
        {
            _compiler = new RuntimeCompiler();
            _container = new Container();
            _factory = new UObjectAdapterFactory(_compiler);
            _uObject = new UObject();
        }

        private class Counter
        {
            public int Value { get; set; }
        }


        [Test]
        public void TestAutoAdapter()
        {
            IMovable adapter = null;

            var sets = new Counter();
            var gets = new Counter();

            Func<IContainer, IUObject, Vector2> get = (c, u) =>
            {
                gets.Value++;
                return u.GetProperty<Vector2>(nameof(IMovable.Position));
            };


            Action<IContainer, IUObject, Vector2> set = (c, u, v) =>
            {
                sets.Value++;
                u.SetProperty(nameof(IMovable.Position), v + Vector2.One);
            };

            Assert.DoesNotThrow(() =>
            {
                adapter = _factory.Adapt<IMovable>(_uObject, _container);
            });

            Assert.DoesNotThrow(() =>
            {
                //Устанавливаем стратегию для get и set
                _factory.AdaptStrategyForPropery(
                    _container,
                    nameof(IMovable.Position),
                    get,
                    set
                );
            });

            adapter.Position = new Vector2(100, 100);

            Assert.That(adapter.Position == new Vector2(101, 101));

            Assert.DoesNotThrow(() =>
            {
                _factory.RemoveAdaptedStrategyForProperty(_container, nameof(IMovable.Position), PropertyTypeEnum.Get | PropertyTypeEnum.Set);
            });

            adapter.Position = new Vector2(100, 100);

            Assert.That(adapter.Position == new Vector2(100, 100));

            //Тестим что стратегии сработали
            Assert.That(sets.Value == 1);
            Assert.That(gets.Value == 1);

            //Null тесты
            Assert.Throws<ArgumentNullException>(() => new UObjectAdapterFactory(null));
            Assert.Throws<ArgumentNullException>(() => _factory.Adapt<IMovable>(null, _container));
            Assert.Throws<ArgumentNullException>(() => _factory.Adapt<IMovable>(_uObject, null));

            Assert.Throws<ArgumentNullException>(() => _factory.AdaptStrategyForPropery(null, "propName", get, set));
            Assert.Throws<ArgumentNullException>(() => _factory.AdaptStrategyForPropery(_container, null, get, set));
            Assert.Throws<AggregateException>(() => _factory.AdaptStrategyForPropery<Vector2>(_container, "propName", null, null));

            Assert.Throws<ArgumentException>(() => _factory.RemoveAdaptedStrategyForProperty(_container, "test", (PropertyTypeEnum)0));
            Assert.Throws<ArgumentNullException>(() => _factory.RemoveAdaptedStrategyForProperty(null, "test", PropertyTypeEnum.Get));
            Assert.Throws<ArgumentNullException>(() => _factory.RemoveAdaptedStrategyForProperty(_container, null, PropertyTypeEnum.Get));
        }
    }
}
