using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Lesson9.Code.Scopes
{
    public class LifetimeScope : ILifetimeScope, IDisposable
    {
        IContainer _container;
        private bool _disposedValue;

        public LifetimeScope(IContainer baseContainer)
        {
            _container = baseContainer;
        }

        public bool CanResolve<T>()
        {
            return _container.CanResolve<T>();
        }

        public bool CanResolve(Type type)
        {
            return _container.CanResolve(type);
        }

        public bool CanResolve<T>(string name)
        {
            return _container.CanResolve<T>(name);
        }

        public bool CanResolve(Type type, string name)
        {
            return _container.CanResolve(type, name);
        }

        public object Resolve(Type type, string name, params object[] args)
        {
            return _container.Resolve(type, name, args);
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

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _container = null;
                _disposedValue = true;
            }
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        // ~LifetimeScope()
        // {
        //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
