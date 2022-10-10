using Lesson7.Code;
using Lesson7.Code.Commands;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;

namespace Lession7.Tests
{
    public class BurnFuelCommandTest
    {
        public Mock<IFuelBurnTarget> GetMock(float initFuel = 1, float burnFuel = 1)
        {
            var mock = new Mock<IFuelBurnTarget>();
            mock.SetupAllProperties();
            mock.Setup(x => x.FuelNeedToBurn).Returns(burnFuel);
            mock.Object.FuelLevel = initFuel;

            return mock;
        }

        [Test]
        [TestCase(5, 5)]
        [TestCase(5, 10)]
        public void BurnFuelTests(float initFuel, float burnFuel)
        {
            var mock = GetMock(initFuel, burnFuel);

            var obj = mock.Object;
            obj.FuelLevel = initFuel;

            var command = new BurnFuelCommand(obj);
            command.Execute();

            Assert.That((initFuel - burnFuel) == obj.FuelLevel);
        }

        [Test]
        public void NullReferenceTest()
        {
            Assert.Throws<ArgumentNullException>(() => new BurnFuelCommand(null));
        }

        [Test]
        public void CheckFuelLevelSet()
        {
            var mock = GetMock();
            mock.SetupSet(x => x.FuelLevel).Throws(new Exception());
            var command = new BurnFuelCommand(mock.Object);
            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void CheckFuelLevelGet()
        {
            var mock = GetMock();
            mock.SetupGet(x => x.FuelLevel).Throws(new Exception());
            var command = new BurnFuelCommand(mock.Object);
            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void CheckFuelNeedToBurnGet()
        {
            var mock = GetMock();
            mock.SetupGet(x => x.FuelNeedToBurn).Throws(new Exception());
            var command = new BurnFuelCommand(mock.Object);
            Assert.Throws<Exception>(() => command.Execute());
        }
    }
}