using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson8.Code.ExceptionHandlers
{
    public abstract class HandledExceptionBase : Exception
    {
        public abstract void HandleException();
    }
}
