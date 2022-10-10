using Lesson5.Code.Commands;
using Lesson7.Code;
using Lesson7.Code.Commands;
using Moq;

namespace Lession7.Tests
{
    public class CheckFuelCommandTest
    {
        public Mock<IFuelCheckTarget> GetMock(float initFuel = 1, float burnFuel = 1)
        {
            var mock = new Mock<IFuelCheckTarget>();
            mock.SetupAllProperties();
            mock.Setup(x => x.FuelLevel).Returns(initFuel);
            mock.Setup(x => x.FuelNeedToBurn).Returns(burnFuel);

            return mock;
        }

        [Test]
        [TestCase(5, 5)]
        [TestCase(5, 10)]
        public void CheckFuel(float initFuel, float burnFuel)
        {
            var command = new CheckFuelCommand(GetMock(initFuel, burnFuel).Object);

            if (initFuel < burnFuel)
            {
                Assert.Throws<CommandException>(() => command.Execute());
            }
            else
            {
                Assert.DoesNotThrow(() => command.Execute());
            }
        }

        [Test]
        public void NullReferenceTest()
        {
            Assert.Throws<ArgumentNullException>(() => new CheckFuelCommand(null));
        }

        [Test]
        public void CheckFuelLevelGet()
        {
            var mock = GetMock();
            mock.SetupGet(x => x.FuelLevel).Throws(new Exception());
            var command = new CheckFuelCommand(mock.Object);
            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void CheckFuelNeedToBurnGet()
        {
            var mock = GetMock();
            mock.SetupGet(x => x.FuelNeedToBurn).Throws(new Exception());
            var command = new CheckFuelCommand(mock.Object);
            Assert.Throws<Exception>(() => command.Execute());
        }
    }
}