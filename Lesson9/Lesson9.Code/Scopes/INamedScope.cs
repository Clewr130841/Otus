using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson9.Code.Scopes
{
    public interface INamedScope
    {
        IContainer WithName(string name);
    }
}
