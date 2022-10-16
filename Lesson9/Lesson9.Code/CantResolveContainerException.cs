using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code
{
    public class CantResolveContainerException : ContainerException
    {
        public CantResolveContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CantResolveContainerException(string message) : base(message, null)
        {
        }
    }
}
