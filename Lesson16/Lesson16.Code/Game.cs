using Lesson12.Code;
using Lesson14.Code;
using Lesson14.Code.Loops;
using Lesson5.Code.Commands;
using Lesson9.Code;
using Lesson9.Code.Scopes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public class Game : IGame
    {
        IContainer _scope;
        IUObjectAdapterFactory _uObjectAdapterFactory;
        Guid _guid;

        public Guid Guid => _guid;
        public IContainer GameScope => _scope;

        public Game(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _guid = Guid.NewGuid();
            _scope = container.Resolve<ILifetimeScope>();
            _uObjectAdapterFactory = _scope.Resolve<IUObjectAdapterFactory>();
        }

        public IUObject FindObject(Guid guid)
        {
            var key = guid.ToString();
            if (_scope.CanResolve<IUObject>(key))
            {
                return _scope.Resolve<IUObject>(key);
            }

            return null;
        }

        /// <summary>
        /// Create a new game object and register it in scope
        /// </summary>
        /// <returns></returns>
        public Guid CreateObject()
        {
            var result = _scope.Resolve<IUObject>();
            var guid = Guid.NewGuid();

            var guidSetter = _uObjectAdapterFactory.Adapt<IGuidSetter>(result, _scope);
            guidSetter.Guid = guid;

            _scope.Resolve<IRegistration>().Register<IUObject>()
                .WithName(guid.ToString())
                .AsSingleton(() => result)
                .Complete();

            return guid;
        }
    }
}
