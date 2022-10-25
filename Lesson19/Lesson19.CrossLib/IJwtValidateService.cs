using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.CrossLib
{
    public interface IJwtValidateService
    {
        public bool Check(string token);
    }
}
