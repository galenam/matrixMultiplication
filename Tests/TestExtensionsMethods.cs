using System.Collections.Generic;
using MatrixMultipling.Project;
using NUnit.Framework;

namespace MatrixMultipling.Tests
{
    [TestFixture]
    public class TestExtensionsMethods
    {
        [SetUp]
        public void Setup()
        {
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

        // todo : сделать все вспомогательные методы в Project internal и добавить тесты в проект Project
        [TestCaseSource(nameof(TestMatrixs))]
        public void TestCreateSquareMatrixPower2(int[,] matrixSource)
        {
            var matrixResult = MathExtensions.CreateSquareMatrixPower2(matrixSource);
            Assert.NotNull(matrixResult);
            Assert.AreEqual(matrixResult.GetLength(0), matrixResult.GetLength(1));
            Assert.AreEqual(matrixResult.GetLength(0), MathExtensions.GetNearestGreater2Power(matrixResult.GetLength(0)));
            Assert.AreEqual(MathExtensions.CompareUpTo0(matrixSource, matrixResult), true);
        }
    }
}