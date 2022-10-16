using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson12.Code
{
    public class UObject : IUObject
    {
        Dictionary<string, object> _properties;

        public UObject()
        {
            _properties = new Dictionary<string, object>();
        }

        public T GetProperty<T>(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_properties.TryGetValue(name, out var value))
            {
                return (T)value;
            }
            return default(T);
        }

        public void SetProperty<T>(string name, T value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _properties[name] = value;
        }
    }
}
