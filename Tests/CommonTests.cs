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
        public void TestException()
        {
            var m1Data = new int[21] { 1, 2, 3, 4, 5, 3, 2, 4, 5, 4, 2, 2, 34, 5, 5, 5, 6, 7, 2, 5, 3 };
            Assert.Throws(typeof(ArgumentException), () => new CustomMatrix(4, m1Data));
        }
        [Test]
        public void TestFillArray()
        {
            var m1Data = new int[6] { 1, 2, 3, 4, 5, 3 };
            var m1 = new CustomMatrix(3, m1Data);
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
            var m1Data = new int[20] { 1, 2, 3, 4, 5, 3, 2, 4, 5, 4, 2, 2, 34, 5, 5, 5, 6, 7, 2, 5 };
            var m2Data = new int[4] { 1, 2, 3, 4 };

            var m1 = new CustomMatrix(4, m1Data);
            var m2 = new CustomMatrix(2, m1Data);
            Assert.Throws(typeof(ArgumentException), () => m1.Multiple(m2));
        }

        [Test]
        public void TestClassicMultiplication()
        {
            var m1Data = new int[6] { 1, 2, 3, 4, 5, 3 };
            var m2Data = new int[6] { 1, 2, 3, 3, 4, 6 };

            var m1 = new CustomMatrix(3, m1Data);
            var m2 = new CustomMatrix(2, m1Data);
            var m3 = m1.Multiple(m2);

            var correctData = new CustomMatrix(3, new int[] { 7, 10, 15, 15, 22, 33, 14, 22, 33 });
            Assert.That(m3.Equals(correctData));
            /*
            7 10 15
            15 22 33
            14 22 33                        
             */
        }
    }
}