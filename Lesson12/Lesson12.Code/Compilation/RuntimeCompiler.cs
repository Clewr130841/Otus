using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Numerics;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Lesson12.Code.Compilation
{
    public class RuntimeCompiler : IRuntimeCompiler
    {
        public Assembly CompileToAssembly(string sourceCode, params Type[] usedTypes)
        {
            if (sourceCode == null)
            {
                throw new ArgumentNullException(nameof(sourceCode));
            }

            if (usedTypes == null || usedTypes.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(usedTypes));
            }

            var asmName = "_" + Guid.NewGuid().ToString().Replace("-", "");
            var buffer = Compile(sourceCode, asmName, usedTypes.Select(x => x.Assembly).Distinct().ToArray());
            var assembly = Assembly.Load(buffer);

            return assembly;
        }

        private byte[] Compile(string sourceCode, string assemblyName, params Assembly[] assemblies)
        {
            var compilation = GenerateCode(sourceCode, assemblyName, assemblies);
            using (var peStream = new MemoryStream())
            {
                var result = compilation.Emit(peStream);

                if (result.Success)
                {
                    peStream.Seek(0, SeekOrigin.Begin);

                    return peStream.ToArray();
                }
                throw new Exception();
            }
        }

        private IEnumerable<Assembly> GetFullDependencyList(List<Assembly> asms, List<Assembly> fullList)
        {
            var referenced = asms.SelectMany(x => x.GetReferencedAssemblies()).Distinct().Select(x => Assembly.Load(x));
            var foundDependencies = referenced.Where(z => !fullList.Contains(z)).ToList();

            if (foundDependencies.Any())
            {
                fullList.AddRange(foundDependencies);
                return GetFullDependencyList(foundDependencies, fullList);
            }

            return fullList.Distinct();
        }

        private CSharpCompilation GenerateCode(string sourceCode, string assemblyName, params Assembly[] assemblies)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp9);
            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);


            var asms = new List<Assembly>() { typeof(object).Assembly };
            asms.AddRange(assemblies);
            asms = GetFullDependencyList(asms.ToList(), asms).ToList();

            var references = asms.Select(x => MetadataReference.CreateFromFile(x.Location)).ToArray();

            return CSharpCompilation.Create($"{assemblyName}.dll",
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel:
#if DEBUG
                    OptimizationLevel.Debug,
#else
                    OptimizationLevel.Release,
#endif
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }
    }
}
