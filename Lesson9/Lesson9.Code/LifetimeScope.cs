using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Lesson9.Code
{
    public class LifetimeScope : ILifetimeScope, IDisposable
    {
        IContainer _container;
        private bool _disposedValue;

        public LifetimeScope(IContainer baseContainer)
        {
            _container = baseContainer;
        }

        public object Resolve(string name)
        {
            return _container.Resolve(name);
        }

        public object Resolve(string name, params object[] args)
        {
            return _container.Resolve(name, args);
        }

        public object Resolve(Type type, params object[] args)
        {
            return _container.Resolve(type, args);
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
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
