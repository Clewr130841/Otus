using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public class NamedScope : INamedScope
    {
        IContainer _parentContainer;
        ConcurrentDictionary<string, IContainer> _containers;
        private bool disposedValue;

        public NamedScope(IContainer parentContainer)
        {
            _parentContainer = parentContainer;
            _containers = new ConcurrentDictionary<string, IContainer>();
        }

        public IContainer WithName(string name)
        {
            return _containers.GetOrAdd(name, (_) => _parentContainer.Resolve<ILifetimeScope>());
        }

        public void Remove(string name)
        {
            _containers.TryRemove(name, out _);
        }
    }
}
