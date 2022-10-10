using Lesson7.Code;
using Lesson7.Code.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lession7.Tests
{
    public class ChangeVelocityCommandTests
    {
        public Mock<IChangeVelocityTarget> GetMock(Vector2 initVelocity, Vector2 newVelocity)
        {
            var mock = new Mock<IChangeVelocityTarget>();
            mock.SetupAllProperties();
            mock.Object.Velocity = initVelocity;
            mock.Setup(x => x.NewVelocity).Returns(newVelocity);

            return mock;
        }

        [Test]
        public void TestChangeVelocity()
        {
            var mock = GetMock(Vector2.Zero, Vector2.One);

            mock.VerifySet(x => x.Velocity = It.IsAny<Vector2>());

            var command = new ChangeVelocityCommand(mock.Object);
            command.Execute();

            mock.VerifyAll();
        }

        [Test]
        public void NullReferenceTest()
        {
            Assert.Throws<ArgumentNullException>(() => new ChangeVelocityCommand(null));
        }
    }
}
