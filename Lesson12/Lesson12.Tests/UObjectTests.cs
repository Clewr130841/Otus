using Lesson12.Code;
using System.Numerics;

namespace Lesson12.Tests
{
    public class UObjectTests
    {
        [Test]
        public void CompilationTests()
        {
            var uObject = new UObject();

            uObject.SetProperty("TestProp", 1);

            Assert.That(uObject.GetProperty<int>("TestProp") == 1);

            Assert.Throws<InvalidCastException>(() => uObject.GetProperty<Vector2>("TestProp"));

            Assert.That(uObject.GetProperty<int>("TestProp2") == 0);

            Assert.That(uObject.GetProperty<UObjectTests>("TestProp3") == null);


            Assert.Throws<ArgumentNullException>(() => uObject.GetProperty<int>(null));
            Assert.Throws<ArgumentNullException>(() => uObject.SetProperty(null, 1));
        }
    }
}
