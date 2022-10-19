using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson9.Code.Scopes
{
    public class ThreadScope : IThreadScope
    {
        ThreadLocal<IContainer> _threadLocal;

        public ThreadScope(IContainer parentContainer)
        {
            _threadLocal = new ThreadLocal<IContainer>(() => parentContainer.Resolve<ILifetimeScope>());
        }

        public bool CanResolve(Type type)
        {
            return _threadLocal.Value.CanResolve(type);
        }

        public bool CanResolve<T>()
        {
            return _threadLocal.Value.CanResolve<T>();
        }

        public bool CanResolve<T>(string name)
        {
            return _threadLocal.Value.CanResolve<T>(name);
        }

        public bool CanResolve(Type type, string name)
        {
            return _threadLocal.Value.CanResolve(type, name);
        }

        public object Resolve(Type type, params object[] args)
        {
            return _threadLocal.Value.Resolve(type, args);
        }

        public T Resolve<T>(params object[] args)
        {
            return _threadLocal.Value.Resolve<T>(args);
        }

        public T Resolve<T>(string name, params object[] args)
        {
            return Resolve<T>(name, args);
        }

        public object Resolve(Type type, string name, params object[] args)
        {
            return Resolve(type, name, args);
        }
    }
}
