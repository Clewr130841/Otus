using System;

namespace Lesson9.Code
{
    public interface IContainer
    {
        object Resolve(string name, params object[] args);
        object Resolve(Type type, params object[] args);
        T Resolve<T>(params object[] args);
        T Resolve<T>(string name, params object[] args);
    }
}