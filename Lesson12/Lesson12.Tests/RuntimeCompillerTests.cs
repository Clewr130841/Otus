using Lesson12.Code.Compilation;
using System.Reflection;

namespace Lesson12.Tests
{
    public class RuntimeCompillerTests
    {
        const string TEST_PROP = "TestValue";

        IRuntimeCompiler compiler;
        [SetUp]
        public void Setup()
        {
            compiler = new RuntimeCompiler();
        }

        [Test]
        public void CompilationTests()
        {
            Assert.DoesNotThrow(() =>
            {
                var asm = compiler.CompileToAssembly(@$"
public class TestClass
{{
    public int {TEST_PROP} {{ get; private set; }}
    public TestClass(int testValue)
    {{
        this.{TEST_PROP} = testValue;
    }}
}}");
                var type = asm.DefinedTypes.First();
                var testValue = 2;
                var instance = Activator.CreateInstance(type, testValue);
                var prop = type.GetProperties().First(x => x.Name == TEST_PROP);
                var instanceValue = prop.GetValue(instance);

                Assert.That(instanceValue.Equals(testValue));
            });

            Assert.Throws<ArgumentNullException>(() => compiler.CompileToAssembly(null));
            Assert.Throws<ArgumentNullException>(() => compiler.CompileToAssembly(String.Empty, null));
        }
    }
}