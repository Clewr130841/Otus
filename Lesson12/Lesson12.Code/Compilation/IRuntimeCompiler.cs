using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lesson12.Code.Compilation
{
    public interface IRuntimeCompiler
    {
        Assembly CompileToAssembly(string sourceCode, params Type[] usedTypes);
    }
}
