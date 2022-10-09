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
            var gamePropsMock = new Mock<IGameProperties>();
            gamePropsMock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);
            var gameProps = gamePropsMock.Object;

            var rotatableMock = new Mock<IRotatable>();
            rotatableMock.SetupAllProperties();
            rotatableMock.Setup(x => x.AngularVelocity).Returns(angularVelocity);

            var rotatable = rotatableMock.Object;
            rotatable.Direction = initRotation;

            var command = new RotateCommand(rotatable, gameProps);
            command.Execute();

            Assert.That(rotatable.Direction == resultRotation);
        }

        [Test]
        public void SetDirectionTest()
        {
            var gamePropsMock = new Mock<IGameProperties>();
            gamePropsMock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);
            var gameProps = gamePropsMock.Object;

            var rotatableMock = new Mock<IRotatable>();
            rotatableMock.SetupAllProperties();
            rotatableMock.SetupGet(x => x.Direction).Throws<Exception>();

            var rotatable = rotatableMock.Object;

            var command = new RotateCommand(rotatable, gameProps);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void GetDirectionTest()
        {
            var gamePropsMock = new Mock<IGameProperties>();
            gamePropsMock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);
            var gameProps = gamePropsMock.Object;

            var rotatableMock = new Mock<IRotatable>();
            rotatableMock.SetupAllProperties();
            rotatableMock.SetupGet(x => x.Direction).Throws<Exception>();

            var rotatable = rotatableMock.Object;

            var command = new RotateCommand(rotatable, gameProps);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void GetAngularVelocityTest()
        {
            var gamePropsMock = new Mock<IGameProperties>();
            gamePropsMock.Setup(x => x.DirectionsNumber).Returns(DIRECTIONS_NUMBER);
            var gameProps = gamePropsMock.Object;

            var rotatableMock = new Mock<IRotatable>();
            rotatableMock.SetupAllProperties();
            rotatableMock.SetupGet(x => x.AngularVelocity).Throws<Exception>();

            var rotatable = rotatableMock.Object;

            var command = new RotateCommand(rotatable, gameProps);

            Assert.Throws<Exception>(() => command.Execute());
        }

        [Test]
        public void GetDirectionsNumberTest()
        {
            var gamePropsMock = new Mock<IGameProperties>();
            gamePropsMock.SetupAllProperties();
            gamePropsMock.SetupGet(x => x.DirectionsNumber).Throws<Exception>();
            var gameProps = gamePropsMock.Object;

            var rotatableMock = new Mock<IRotatable>();
            rotatableMock.SetupAllProperties();

            var rotatable = rotatableMock.Object;

            var command = new RotateCommand(rotatable, gameProps);

            Assert.Throws<Exception>(() => command.Execute());
        }

    }
}
