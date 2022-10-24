using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public class ExceptionWithCode : Exception
    {
        public int Code { get; private set; }
        public ExceptionWithCode(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
