using NUnit.Framework;
using MatrixMultipling.Project.Model;
using MatrixMultipling.Project;
using MatrixMultipling.Project.Methods;
using System;

namespace MatrixMultipling.Tests
{
    [TestFixture]
    public class CommonTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFillArray()
        {
            var m1Data = new int[3, 2] { { 1, 2 }, { 3, 4 }, { 5, 3 } };
            var m1 = new CustomMatrix(m1Data);
            Assert.That(m1 != null);
            Assert.That(m1.Values != null);
            Assert.That(m1.Values.GetLength(0) == 3);
            Assert.That(m1.Values.GetLength(1) == 2);
            Assert.That(m1.Values[0, 0] == 1);
            Assert.That(m1.Values[0, 1] == 2);
            Assert.That(m1.Values[1, 0] == 3);
            Assert.That(m1.Values[1, 1] == 4);
            Assert.That(m1.Values[2, 0] == 5);
            Assert.That(m1.Values[2, 1] == 3);
        }

        [Test]
        public void TestExceptionMultipliation()
        {
            var m1Data = new int[5, 4] { { 1, 2, 3, 4 }, { 5, 3, 2, 4 }, { 5, 4, 2, 2 }, { 34, 5, 5, 5 }, { 6, 7, 2, 5 } };
            var m2Data = new int[,] { { 1, 2 }, { 3, 4 } };

            var m1 = new CustomMatrix(m1Data);
            var m2 = new CustomMatrix(m1Data);
            Assert.Throws<ArgumentException>(delegate { var customMatrix = m1 * m2; });
        }

        [Test]
        public void TestClassicMultiplication()
        {
            var m1Data = new int[3, 2] { { 1, 2 }, { 3, 4 }, { 5, 3 } };
            var m2Data = new int[2, 3] { { 1, 2, 3 }, { 3, 4, 6 } };

            var m1 = new CustomMatrix(m1Data);
            var m2 = new CustomMatrix(m2Data);
            var m3 = m1 * m2;
            var correctData = new CustomMatrix(new int[,] { { 7, 10, 15 }, { 15, 22, 33 }, { 14, 22, 33 } });
            Assert.That(m3.Equals(correctData));
        }

        [Test]
        public void TestClassicMultiplicationNonSquare()
        {
            var m1Data = new int[4, 2] { { 1, 2 }, { 3, 4 }, { 5, 3 }, { 7, 9 } };
            var m2Data = new int[2, 3] { { 1, 2, 3 }, { 3, 4, 6 } };

            var m1 = new CustomMatrix(m1Data);
            var m2 = new CustomMatrix(m2Data);
            var m3 = m1 * m2;

            var correctData = new CustomMatrix(new int[,] { { 7, 10, 15 }, { 15, 22, 33 }, { 14, 22, 33 }, { 34, 50, 75 } });
            Assert.That(m3.Equals(correctData));
        }
    }
}