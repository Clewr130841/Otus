using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code.Exceptions
{
    public class ContainerException : Exception
    {
        public ContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
