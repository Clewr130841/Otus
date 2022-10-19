using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public interface IRegistration
    {
        IRegistrationOption Register<T>();
        IRegistrationOption Register(Type type);
        IRegistration RegisterModule(IContainerModule module);

        void Unregister<T>();
        void Unregister<T>(string name);
        void Unregister(Type t);
        void Unregister(Type t, string name);
    }
}
