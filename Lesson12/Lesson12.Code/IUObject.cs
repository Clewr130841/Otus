using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lesson12.Code
{
    public interface IUObject
    {
        public T GetProperty<T>(string name);

        public void SetProperty<T>(string name, T value);
    }
}
