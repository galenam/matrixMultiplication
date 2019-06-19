using NUnit.Framework;
using MatrixMultipling.Project;
using System;
using System.Collections.Generic;
using System.Collections;

namespace MatrixMultipling.Tests
{
    /*
     */
    [TestFixture]
    public class CommonTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestExceptionMultipliation()
        {
            var m1Data = new int[5, 4] { { 1, 2, 3, 4 }, { 5, 3, 2, 4 }, { 5, 4, 2, 2 }, { 34, 5, 5, 5 }, { 6, 7, 2, 5 } };
            var m2Data = new int[,] { { 1, 2 }, { 3, 4 } };

            Assert.Throws<ArgumentException>(() => MultiplicationAlgoritm.Classic(m1Data, m2Data));
        }

        [Test]
        public void TestClassicMultiplication()
        {
            var m1Data = new int[3, 2] { { 1, 2 }, { 3, 4 }, { 5, 3 } };
            var m2Data = new int[2, 3] { { 1, 2, 3 }, { 3, 4, 6 } };

            var resultData = MultiplicationAlgoritm.Classic(m1Data, m2Data);
            var correctData = new int[,] { { 7, 10, 15 }, { 15, 22, 33 }, { 14, 22, 33 } };
            Assert.That(resultData.CompareContent(correctData));
        }

        [Test]
        public void TestClassicMultiplicationResultNonSquare()
        {
            var m1Data = new int[4, 2] { { 1, 2 }, { 3, 4 }, { 5, 3 }, { 7, 9 } };
            var m2Data = new int[2, 3] { { 1, 2, 3 }, { 3, 4, 6 } };

            var resultData = MultiplicationAlgoritm.Classic(m1Data, m2Data);

            var correctData = new int[,] { { 7, 10, 15 }, { 15, 22, 33 }, { 14, 22, 33 }, { 34, 50, 75 } };
            Assert.That(resultData.CompareContent(correctData));
        }

        [Test]
        public void TestExceptionMultipliationStrassenNonConsistent()
        {
            var m1Data = new int[5, 4] { { 1, 2, 3, 4 }, { 5, 3, 2, 4 }, { 5, 4, 2, 2 }, { 34, 5, 5, 5 }, { 6, 7, 2, 5 } };
            var m2Data = new int[,] { { 1, 2 }, { 3, 4 } };

            Assert.Throws<ArgumentException>(() => MultiplicationAlgoritm.Strassen(m1Data, m2Data));
        }
        [Test]
        public void TestExceptionMultipliationStrassenNonSquare()
        {
            var m1Data = new int[4, 2] { { 1, 2 }, { 3, 4 }, { 5, 3 }, { 7, 9 } };
            var m2Data = new int[2, 3] { { 1, 2, 3 }, { 3, 4, 6 } };

            Assert.Throws<ArgumentException>(() => MultiplicationAlgoritm.Strassen(m1Data, m2Data));
        }

        [TestCase(0, ExpectedResult = false)]
        [TestCase(1, ExpectedResult = false)]
        [TestCase(2, ExpectedResult = true)]
        [TestCase(5, ExpectedResult = false)]
        [TestCase(128, ExpectedResult = true)]
        public bool TestPowerOf2(int value)
        {
            return MathExtensions.IsPowerOfTwo(value);
        }

        [TestCase(1, ExpectedResult = 2)]
        [TestCase(2, ExpectedResult = 2)]
        [TestCase(5, ExpectedResult = 8)]
        [TestCase(127, ExpectedResult = 128)]
        [TestCase(128, ExpectedResult = 128)]
        public int TestNearestGreater2Power(int value)
        {
            return MathExtensions.GetNearestGreater2Power(value);
        }
        static IEnumerable<int[,]> TestMatrixs()
        {
            yield return new int[4, 4] { { 1, 2, 4, 5 }, { 3, 4, 5, 6 }, { 5, 3, 1, 7 }, { 7, 9, 0, 5 } };
            yield return new int[3, 3] { { 1, 2, 4 }, { 3, 4, 5 }, { 5, 3, 1 } };
        }

        [TestCaseSource(nameof(TestMatrixs))]
        public void TestCreateSquareMatrixPower2(int[,] matrixSource)
        {
            var matrixResult = MathExtensions.CreateSquareMatrixPower2(matrixSource);
            Assert.NotNull(matrixResult);
            Assert.AreEqual(matrixResult.GetLength(0), matrixResult.GetLength(1));
            Assert.AreEqual(matrixResult.GetLength(0), MathExtensions.GetNearestGreater2Power(matrixResult.GetLength(0)));
            Assert.AreEqual(MathExtensions.CompareUpTo0(matrixSource, matrixResult), true);
        }

        [Test]
        public void TestStrassenMultiplicationNon2Degree()
        {
            var m1Data = new int[3, 3] { { 1, 2, 7 }, { 3, 4, 8 }, { 5, 3, 3 } };
            var m2Data = new int[3, 3] { { 1, 2, 3 }, { 3, 4, 6 }, { 3, 9, 4 } };
            var resultData = MultiplicationAlgoritm.Strassen(m1Data, m2Data);

            var correctData = new int[,] { { 28, 73, 43 }, { 39, 94, 65 }, { 23, 49, 45 } };
            Assert.That(resultData.Equals(correctData));
        }

        // todo : тест на матрицу с 8 элементами
        [Test]
        public void TestStrassenMultiplication2Degree()
        {
            var m1Data = new int[4, 4] { { 1, 2, 7, 9 }, { 3, 4, 8, 0 }, { 5, 3, 3, 6 }, { 3, 4, 6, 5 } };
            var m2Data = new int[4, 4] { { 1, 2, 3, 7 }, { 3, 4, 6, 1 }, { 3, 9, 4, 4 }, { 4, 6, 3, 7 } };
            var resultData = MultiplicationAlgoritm.Strassen(m1Data, m2Data);

            var correctData = new int[,] { { 64, 127, 70, 100 }, { 39, 94, 65, 57 }, { 47, 85, 63, 92 }, { 53, 106, 72, 84 } };
            Assert.That(resultData.Equals(correctData));
        }

    }
}