using Lesson5.Code.Commands;
using Lesson5.Code;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;

namespace Lesson5.Tests
{
    public class RotateCommandTests
    {
        const int DIRECTIONS_NUMBER = 360;

        [Test]
        [TestCase(1u, 5, 6u)] // Обычный поворот в +
        [TestCase(2u, -5, 357u)] // Обычный поворот в -
        [TestCase(2u, 370, 12u)] // Переполнение в +
        [TestCase(0u, -370, 350u)] // Переполнение в -
        [TestCase(0u, 0, 0u)] // Проверка при 0
        public void RotateTest(uint initRotation, int angularVelocity, uint resultRotation)
        {
            var mock = new Mock<IRotatable>();
            mock.SetupAllProperties();
            mock.Setup(x => x.AngularVelocity).Returns(angularVelocity);
            mock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);

            var rotatable = mock.Object;
            rotatable.Direction = initRotation;

            var command = new RotateCommand(rotatable);

            Assert.DoesNotThrow(() => command.Execute());
            Assert.That(rotatable.Direction == resultRotation);
        }

        [Test]
        public void SetDirectionTest()
        {
            var mock = new Mock<IRotatable>();
            mock.SetupAllProperties();
            mock.SetupGet(x => x.Direction).Throws<Exception>();
            mock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);

            var command = new RotateCommand(mock.Object);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void GetDirectionTest()
        {
            var mock = new Mock<IRotatable>();
            mock.SetupAllProperties();
            mock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);
            mock.SetupGet(x => x.Direction).Throws<Exception>();

            var command = new RotateCommand(mock.Object);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void NullReferenceTest()
        {
            Assert.Throws<ArgumentNullException>(()=> new RotateCommand(null));
        }

        [Test]
        public void GetAngularVelocityTest()
        {
            var mock = new Mock<IRotatable>();
            mock.SetupAllProperties();
            mock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);
            mock.SetupGet(x => x.AngularVelocity).Throws<Exception>();

            var command = new RotateCommand(mock.Object);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void GetDirectionsNumberTest()
        {
            var mock = new Mock<IRotatable>();
            mock.SetupAllProperties();
            mock.Setup(x => x.DirectionsNumber).Throws<Exception>();
            var command = new RotateCommand(mock.Object);

            Assert.Throws<Exception>(() => command.Execute());
        }

    }
}
