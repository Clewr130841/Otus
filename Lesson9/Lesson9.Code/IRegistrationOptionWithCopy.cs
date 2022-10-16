using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public interface IRegistrationOptionWithCopy : IRegistrationOption
    {
        IRegistrationOptionWithCopy Copy(IRegistrationContainer container);
    }
}
