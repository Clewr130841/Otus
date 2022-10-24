using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson12.Code.Exceptions
{
    public abstract class SelfHandlingException : Exception
    {
        public abstract void Handle();
    }
}
