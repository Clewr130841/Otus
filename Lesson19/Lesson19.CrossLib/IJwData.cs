using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.CrossLib
{
    public interface IJwtData
    {
        string Token { get; }
        string RefreshToken { get; }
    }
}
