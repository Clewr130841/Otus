using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code.Exceptions
{
    public class CantRegisterContainerException : ContainerException
    {
        public CantRegisterContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CantRegisterContainerException(string message) : base(message, null)
        {

        }
    }
}
