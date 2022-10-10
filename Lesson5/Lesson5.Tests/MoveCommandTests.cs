using Lesson5.Code;
using Lesson5.Code.Commands;
using Moq;
using System.Numerics;

namespace Lesson5.Tests
{
    public class MoveCommandTests
    {
        [Test]
        public void MoveTest()
        {
            var mock = new Mock<IMovable>();
            mock.SetupAllProperties();
            mock.Setup(x => x.Velocity).Returns(new Vector2(-7, 3));
            var movable = mock.Object;
            movable.Position = new Vector2(12, 5);

            var command = new MoveCommand(movable);

            Assert.DoesNotThrow(() => command.Execute());
            Assert.That(movable.Position == new Vector2(5, 8));
        }

        [Test]
        public void NullReferenceTest()
        {
            Assert.Throws<ArgumentNullException>(() => new MoveCommand(null));
        }

        [Test]
        public void PositionCantReadTest()
        {
            var mock = new Mock<IMovable>();
            mock.SetupAllProperties();
            mock.Setup(x => x.Position).Throws(new Exception());
            var movable = mock.Object;
            var command = new MoveCommand(movable);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void VelocityCantReadTest()
        {
            var mock = new Mock<IMovable>();
            mock.SetupAllProperties();
            mock.Setup(x => x.Velocity).Throws(new Exception());
            var movable = mock.Object;
            var command = new MoveCommand(movable);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void PositionCantWriteTest()
        {
            var mock = new Mock<IMovable>();
            mock.SetupAllProperties();
            mock.SetupSet(x => x.Position).Throws(new Exception());
            var movable = mock.Object;
            var command = new MoveCommand(movable);

            Assert.Throws<Exception>(() => command.Execute());
        }
    }
}