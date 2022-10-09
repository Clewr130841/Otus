using Lesson3.Code;
using NUnit.Framework;


namespace Lesson3.Tests
{
    public class QuadraticEquationUnitTest
    {
        SqueareRoot _quadraticEquation;

        [SetUp]
        public void Setup()
        {
            _quadraticEquation = new SqueareRoot();
        }

        /// <summary>
        /// Корней нет
        /// </summary>
        [Test]
        [TestCase(double.Epsilon / 2, 0, 1)]
        [TestCase(1, 0, 1)]
        public void TestHaveNoRoots(double a, double b, double c)
        {
            Assert.IsEmpty(_quadraticEquation.Solve(a, b, c));
        }

        /// <summary>
        /// Один корень кратности 2
        /// </summary>
        [Test]
        [TestCase(1, -2, 1)]
        public void TestEqualRoots(double a, double b, double c)
        {
            var result = _quadraticEquation.Solve(a, b, c);

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], result[1]);
        }

        /// <summary>
        /// Два корня кратности 1
        /// </summary>
        [Test]
        [TestCase(1, 0, -1)]
        public void TestRootsEqualOne(double a, double b, double c)
        {
            var result = _quadraticEquation
                .Solve(a, b, c)
                .OrderBy(x => x)
                .ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], -1);
            Assert.AreEqual(result[1], 1);
        }

        /// <summary>
        /// Обычный случай
        /// </summary>
        [Test]
        [TestCase(4, 6.2, 2)]
        public void TestHaveRoots(double a, double b, double c)
        {
            Assert.AreEqual(2, _quadraticEquation.Solve(a, b, c).Length);
        }

        /// <summary>
        /// Проблемы с аргументами
        /// </summary>
        [Test]
        [TestCase(double.NaN, 0, 0)]
        [TestCase(0, double.NaN, 0)]
        [TestCase(0, 0, double.NaN)]
        [TestCase(double.PositiveInfinity, 0, 0)]
        [TestCase(0, double.PositiveInfinity, 0)]
        [TestCase(0, 0, double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity, 0, 0)]
        [TestCase(0, double.NegativeInfinity, 0)]
        [TestCase(0, 0, double.NegativeInfinity)]
        public void TestArguments(double a, double b, double c)
        {
            Assert.Throws<ArgumentException>(() => _quadraticEquation.Solve(a, b, c));
        }
    }
}