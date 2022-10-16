using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public interface IRegistrationContainer : IContainer
    {
        bool CanResolve(Type type);
        void RegisterConstructor(Type type, ConstructorBase constructor, IRegistrationOptionWithCopy registrationOptions);
        void RegisterConstructor(string name, ConstructorBase constructor, IRegistrationOptionWithCopy registrationOptions);
    }
}
