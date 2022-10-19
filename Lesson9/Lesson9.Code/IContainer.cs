using System;

namespace Lesson9.Code
{
    public interface IContainer
    {
        /// Резолв по дженерик типу
        bool CanResolve<T>();
        bool CanResolve<T>(string name);
        bool CanResolve(Type type);
        bool CanResolve(Type type, string name);

        object Resolve(Type type, string name, params object[] args);
        object Resolve(Type type, params object[] args);

        T Resolve<T>(params object[] args);
        T Resolve<T>(string name, params object[] args);
    }
}