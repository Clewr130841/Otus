﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lesson9.Code
{
    public class ThreadScope : IThreadScope
    {
        IContainer _parentContainer;
        ThreadLocal<IContainer> _threadLocal;
        private bool _disposedValue;

        public ThreadScope(IContainer parentContainer)
        {
            _threadLocal = new ThreadLocal<IContainer>(() => parentContainer.Resolve<ILifetimeScope>());
        }

        public object Resolve(string name)
        {
            return _threadLocal.Value.Resolve(name);
        }

        public object Resolve(string name, params object[] args)
        {
            return _threadLocal.Value.Resolve(name, args);
        }

        public object Resolve(Type type, params object[] args)
        {
            return _threadLocal.Value.Resolve(type, args);
        }

        public T Resolve<T>()
        {
            return _threadLocal.Value.Resolve<T>();
        }

        public T Resolve<T>(params object[] args)
        {
            return _threadLocal.Value.Resolve<T>(args);
        }

        public T Resolve<T>(string name, params object[] args)
        {
            return Resolve<T>(name, args);
        }
    }
}
