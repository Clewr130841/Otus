using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public interface IRegistration
    {
        IRegistrationOption Register<T>();
        IRegistrationOption Register(Type type);
    }
}
