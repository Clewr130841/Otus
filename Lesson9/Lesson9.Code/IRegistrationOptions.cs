using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public interface IRegistrationOption
    {
        IRegistrationOption WithName(string name);
        IRegistrationOption As<T>(Func<IContainer, object[], T> constr);
        IRegistrationOption As<T>(Func<IContainer, T> constr);
        IRegistrationOption As<T>(Func<T> constr);
        IRegistrationOption As(Type type);
        IRegistrationOption As<T>();
        IRegistrationOption AsSingleton<T>(Func<IContainer, object[], T> constr);
        IRegistrationOption AsSingleton<T>(Func<IContainer, T> constr);
        IRegistrationOption AsSingleton<T>(Func<T> constr);
        IRegistrationOption AsSingleton();
        void Complete();
    }
}
