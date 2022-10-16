using Lesson9.Code.Exceptions;
using Lesson9.Code.Resolvers;
using Lesson9.Code.Scopes;
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
        ConcurrentDictionary<string, ResolverBase> _resolvers;
        private bool _disposedValue;
        Container _parentContainer;
        private Container(Container container)
        {
            _parentContainer = container;
            _resolvers = new ConcurrentDictionary<string, ResolverBase>();
            RegisterServiceTypesAndScope();
        }

        public Container()
        {
            _resolvers = new ConcurrentDictionary<string, ResolverBase>();
            RegisterServiceTypesAndScope();
        }

        public bool CanResolve<T>()
        {
            return CanResolve(typeof(T));
        }

        public bool CanResolve(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var key = GetResolverKey(type);
            return CanResolveInner(key);
        }

        public bool CanResolve(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var key = GetResolverKey(name);
            return CanResolveInner(key);
        }

        #region IContainer
        public object Resolve(string name, object[] args)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var key = GetResolverKey(name);


            var resolver = GetResolver(key);

            if (resolver != null)
            {
                try
                {
                    return resolver.Resolve(args, this);
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

            var key = GetResolverKey(type);
            var resolver = GetResolver(key);

            if (resolver != null)
            {
                try
                {
                    return resolver.Resolve(args, this);
                }
                catch (Exception ex)
                {
                    throw new CantResolveContainerException($"Can't resolve type {type.FullName}", ex);
                }
            }

            throw new CantResolveContainerException($"Dependency with type \"{type.FullName}\" is not registered");
        }

        private ResolverBase GetResolver(string key)
        {
            if (_resolvers.TryGetValue(key, out ResolverBase constructor))
            {
                return constructor;
            }
            else if (_parentContainer != null)
            {
                return _parentContainer.GetResolver(key);
            }
            return null;
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
            var registration = new Registration(this);

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

            //Именованые скоупы
            registration.Register<INamedScope>()
                .As(c => new NamedScope(c))
                .AsSingleton()
                .Complete();
        }

        private string GetResolverKey(string name)
        {
            return $"n:{name}";
        }

        private string GetResolverKey(Type type)
        {
            return $"t:{type.FullName}";
        }

        private void RegisterResolver(string name, ResolverBase constructor) //Оставим виртуальным, для наследников
        {
            var key = GetResolverKey(name);
            _resolvers[key] = constructor;
        }

        private void RegisterResolver(Type type, ResolverBase constructor) //Оставим виртуальным, для наследников
        {
            var key = GetResolverKey(type);
            _resolvers[key] = constructor;
        }

        private void UnregisterResolver(string name) //Оставим виртуальным, для наследников
        {
            var key = GetResolverKey(name);
            _resolvers.TryRemove(key, out _);
        }

        private void UnregisterResolver(Type type) //Оставим виртуальным, для наследников
        {
            var key = GetResolverKey(type);
            _resolvers.TryRemove(key, out _);
        }

        private bool CanResolveInner(string key)
        {
            return _resolvers.ContainsKey(key) || (_parentContainer?.CanResolveInner(key) ?? false);
        }

        #region Вспомогательные классы для регистрации, чтобы получить доступ к полям внутри и запечатать интерфейс для мира снаружи
        private class RegistrationOptions : IRegistrationOption
        {
            Container _container;
            string _name;
            Type _asType;
            Type _forType;
            Func<IContainer, object[], object> _constr;
            bool _isSingleton;

            public RegistrationOptions(Type forType, Container container)
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

                ResolverBase resolver;

                // Можно было бы в методе AsSingleton вернуть новый тип, перекопировав параметры, который бы в
                // методе Complete регистрировал SingleInstanceConstructor, но лень, потому if =))

                if (_isSingleton)
                {
                    resolver = new SingleInstanceResolver(_asType ?? _forType, _constr);
                }
                else
                {
                    resolver = new MultipleInstanceResolver(_asType ?? _forType, _constr);
                }

                if (string.IsNullOrEmpty(_name))
                {
                    _container.RegisterResolver(_forType, resolver);
                }
                else
                {
                    _container.RegisterResolver(_name, resolver);
                }
            }
            #endregion
        }

        private class Registration : IRegistration
        {
            private Container _container;

            public Registration(Container container)
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

            public void Unregister<T>()
            {
                _container.UnregisterResolver(typeof(T));
            }

            public void Unregister(Type t)
            {
                _container.UnregisterResolver(t);
            }

            public void Unregister(string name)
            {
                _container.UnregisterResolver(name);
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
                    _resolvers.Clear();
                }

                _parentContainer = null;
                _resolvers = null;
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
