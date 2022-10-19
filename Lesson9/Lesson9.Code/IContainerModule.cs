using Lesson9.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public interface IContainerModule
    {
        void RegisterModule(IRegistration registration);
    }
}
