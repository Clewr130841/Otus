using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;

namespace Lesson9.Code
{
    public class Container : IContainer, IDisposable
    {
        /// <summary>
        /// На всякий случай конкурентный словарь, для избежания ошибок при создании и регистрации с разных потоков;
        /// </summary>
        ConcurrentDictionary<string, ConstructorBase> _constructors;
        List<IRegistrationOptionWithCopy> _registrationOptions;
        private bool _disposedValue;
        IContainer _parentContainer;
        private Container(Container container)
        {
            _parentContainer = container;
            _constructors = new ConcurrentDictionary<string, ConstructorBase>();
            _registrationOptions = new List<IRegistrationOptionWithCopy>();

            var registrationContainer = new RegistrationContainer(this);

            //Копируем все регистрации и создаем все по новой, если синглтон, это не тот синглтон, что был раньше
            IRegistrationOptionWithCopy[] copy;

            lock (container._registrationOptions)
            {
                copy = container._registrationOptions.Select(x => x.Copy(registrationContainer)).ToArray();
            }

            foreach (var option in copy)
            {
                option.Complete();
            }

            RegisterServiceTypesAndScope();
        }

        public Container()
        {
            _registrationOptions = new List<IRegistrationOptionWithCopy>();
            _constructors = new ConcurrentDictionary<string, ConstructorBase>();
            RegisterServiceTypesAndScope();
        }

        #region IContainer
        public object Resolve(string name, object[] args)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var key = GetRegistrationKey(name);

            if (_constructors.TryGetValue(key, out ConstructorBase constructor))
            {
                try
                {
                    return constructor.Construct(args);
                }
                catch (Exception ex)
                {
                    throw new CantResolveContainerException($"Can't resolve dependency with name \"{name}\"", ex);
                }
            }

            throw new CantResolveContainerException($"Dependency \"{name}\" is not registered");
        }

        public object Resolve(Type type, params object[] args)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var key = GetRegistrationKey(type);

            if (_constructors.TryGetValue(key, out ConstructorBase constructor))
            {
                try
                {
                    return constructor.Construct(args);
                }
                catch (Exception ex)
                {
                    throw new CantResolveContainerException($"Can't resolve type {type.FullName}", ex);
                }
            }

            throw new CantResolveContainerException($"Dependency with type \"{type.FullName}\" is not registered");
        }

        public T Resolve<T>(params object[] args)
        {
            return (T)Resolve(typeof(T), args);
        }

        public T Resolve<T>(string name, params object[] args)
        {
            return (T)Resolve(name, args);
        }

        #endregion

        private void RegisterServiceTypesAndScope()
        {
            //Регистрируем типы по умолчанию
            var registration = new Registration(new RegistrationContainer(this));

            //Зависимость регистрации
            registration.Register<IRegistration>()
                .AsSingleton(x => registration)
                .Complete();

            //Зависимость для текущего контейнера
            registration.Register<IContainer>()
                .AsSingleton(x => this)
                .Complete();

            //Зависимость для создания копии скоупа на время жизни объекта
            registration.Register<ILifetimeScope>()
                .As(x => new LifetimeScope(new Container(this)))
                .Complete();

            //Для работы с тред скоупами
            registration.Register<IThreadScope>()
                .As<ThreadScope>()
                .AsSingleton()
                .Complete();

            //Именованые скоупы, должны быть доступны из любых по вложенности скоупов, так что делаем так
            registration.Register<INamedScope>()
                .As(x => _parentContainer?.Resolve<INamedScope>() as NamedScope ?? new NamedScope(x))
                .AsSingleton()
                .Complete();
        }

        private string GetRegistrationKey(string name)
        {
            return $"n:{name}";
        }

        private string GetRegistrationKey(Type type)
        {
            return $"t:{type.FullName}";
        }

        #region Вспомогательные классы для регистрации, чтобы получить доступ к полям внутри и запечатать интерфейс для мира снаружи
        private class RegistrationOptions : IRegistrationOptionWithCopy
        {
            IRegistrationContainer _container;
            string _name;
            Type _asType;
            Type _forType;
            Func<IContainer, object[], object> _constr;
            bool _isSingleton;

            public RegistrationOptions(Type forType, IRegistrationContainer container)
            {
                _container = container;
                _forType = forType;
            }

            #region IRegistrationOptions
            public IRegistrationOption WithName(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException($"Argument {nameof(name)} cannot be all spaces");
                }

                _name = name;
                return this;
            }
            public IRegistrationOption As(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                if (type.IsInterface)
                {
                    throw new ArgumentException($"Argument {type.FullName} cant be an interface type");
                }

                if (type.IsAbstract)
                {
                    throw new ArgumentException($"Argument {type.FullName} cant be an abstract type");
                }

                if (!_forType.IsAssignableFrom(type))
                {
                    throw new ArgumentException($"Type {_forType.FullName} is not assingable from {type.FullName}");
                }

                if (_constr == null && !type.IsPrimitive)
                {
                    var constructors = type.GetConstructors().Where(x => x.IsPublic);
                    if (constructors.Count() == 0)
                    {
                        throw new ArgumentException($"Argument {type.FullName} has to be a type with at least one public constructor");
                    }
                }

                _asType = type;
                return this;
            }
            public IRegistrationOption As<T>()
            {
                return As(typeof(T));
            }
            public IRegistrationOption As<T>(Func<IContainer, object[], T> constr)
            {
                if (constr == null)
                {
                    throw new ArgumentNullException(nameof(constr));
                }

                As<T>();
                _constr = (c, a) => constr(c, a);
                return this;
            }
            public IRegistrationOption As<T>(Func<IContainer, T> constr)
            {
                if (constr == null)
                {
                    throw new ArgumentNullException(nameof(constr));
                }

                return As<T>((c, a) => constr(c));
            }
            public IRegistrationOption As<T>(Func<T> constr)
            {
                if (constr == null)
                {
                    throw new ArgumentNullException(nameof(constr));
                }

                return As<T>((c, a) => constr());
            }
            public IRegistrationOption AsSingleton()
            {
                As(_asType ?? _forType);
                _isSingleton = true;
                return this;
            }
            public IRegistrationOption AsSingleton<T>(Func<IContainer, object[], T> constr)
            {
                if (constr == null)
                {
                    throw new ArgumentNullException(nameof(constr));
                }

                //Тут почти дублирование, но только потому-что у стандартных типов типа int рефлексия не дает конструкторов
                _constr = (c, a) => constr(c, a);

                try
                {
                    As(typeof(T));
                }
                catch (Exception ex)
                {
                    _constr = null;
                    throw ex;
                }

                _isSingleton = true;

                return this;
            }
            public IRegistrationOption AsSingleton<T>(Func<IContainer, T> constr)
            {
                if (constr == null)
                {
                    throw new ArgumentNullException(nameof(constr));
                }

                return AsSingleton((c, a) => constr(c));
            }
            public IRegistrationOption AsSingleton<T>(Func<T> constr)
            {
                if (constr == null)
                {
                    throw new ArgumentNullException(nameof(constr));
                }

                return AsSingleton((c, a) => constr());
            }
            private void CheckRegistration()
            {
                if (_forType.IsInterface && _asType == null)
                {
                    throw new CantRegisterContainerException("Cant complete registration with no call As() method for interface types");
                }

                if (_forType.IsAbstract && _asType == null)
                {
                    throw new CantRegisterContainerException("Cant complete registration with no call As() method for abstract types");
                }
            }
            public void Complete()
            {
                CheckRegistration();

                ConstructorBase constructor;

                // Можно было бы в методе AsSingleton вернуть новый тип, перекопировав параметры, который бы в
                // методе Complete регистрировал SingleInstanceConstructor, но лень, потому if =))

                if (_isSingleton)
                {
                    constructor = new SingleInstanceConstructor(_asType ?? _forType, _container, _constr);
                }
                else
                {
                    constructor = new MultipleInstanceConstructor(_asType ?? _forType, _container, _constr);
                }

                if (string.IsNullOrEmpty(_name))
                {
                    _container.RegisterConstructor(_forType, constructor, this);
                }
                else
                {
                    _container.RegisterConstructor(_name, constructor, this);
                }
            }
            public IRegistrationOptionWithCopy Copy(IRegistrationContainer container)
            {
                if (container == null)
                {
                    throw new ArgumentNullException(nameof(container));
                }

                var copy = new RegistrationOptions(_forType, container);
                copy._name = _name;
                copy._asType = _asType;
                copy._constr = _constr;
                copy._isSingleton = _isSingleton;
                return copy;
            }
            #endregion
        }

        private class Registration : IRegistration
        {
            private IRegistrationContainer _container;

            public Registration(IRegistrationContainer container)
            {
                _container = container;
            }

            public IRegistrationOption Register<T>()
            {
                return Register(typeof(T));
            }

            public IRegistrationOption Register(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                return new RegistrationOptions(type, _container);
            }
        }

        private class RegistrationContainer : IRegistrationContainer
        {
            Container _container;

            public RegistrationContainer(Container container)
            {
                _container = container;
            }

            public bool CanResolve(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                var key = _container.GetRegistrationKey(type);
                return _container._constructors.ContainsKey(key);
            }

            public object Resolve(string name, params object[] args)
            {
                return _container.Resolve(name, args);
            }

            public object Resolve(Type type, params object[] args)
            {
                return _container.Resolve(type, args);
            }

            public T Resolve<T>(params object[] args)
            {
                return _container.Resolve<T>(args);
            }

            public T Resolve<T>(string name, params object[] args)
            {
                return _container.Resolve<T>(name, args);
            }

            public void RegisterConstructor(string name, ConstructorBase constructor, IRegistrationOptionWithCopy options) //Оставим виртуальным, для наследников
            {
                var key = _container.GetRegistrationKey(name);
                _container._constructors[key] = constructor;

                lock (_container._registrationOptions)
                {
                    _container._registrationOptions.Add(options);
                }
            }

            public void RegisterConstructor(Type type, ConstructorBase constructor, IRegistrationOptionWithCopy options) //Оставим виртуальным, для наследников
            {
                var key = _container.GetRegistrationKey(type);
                _container._constructors[key] = constructor;

                lock (_container._registrationOptions)
                {
                    _container._registrationOptions.Add(options);
                }
            }
        }
        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _constructors.Clear();
                    _registrationOptions.Clear();
                }

                _parentContainer = null;
                _registrationOptions = null;
                _constructors = null;
                _disposedValue = true;
            }
        }

        ~Container()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
