using System;
using System.Collections.Generic;
using MatrixMultipling.Project;
using MatrixMultipling.Project.Enums;
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

        /* не получится сделать все вспомогательные методы в Project internal и добавить тесты в проект Project, т.к. проект project - это
        class library (тип netstandard2.0), а nunit не работает с netstandard2.0 (только netcoreapp)
         */
        [TestCaseSource(nameof(TestMatrixs))]
        public void TestCreateSquareMatrixPower2(int[,] matrixSource)
        {
            var matrixResult = MathExtensions.CreateSquareMatrixPower2(matrixSource);
            Assert.NotNull(matrixResult);
            Assert.AreEqual(matrixResult.GetLength(0), matrixResult.GetLength(1));
            Assert.AreEqual(matrixResult.GetLength(0), MathExtensions.GetNearestGreater2Power(matrixResult.GetLength(0)));
            Assert.AreEqual(MathExtensions.CompareUpTo0(matrixSource, matrixResult), true);
        }

        private static IEnumerable<MatrixPart> TestSourcesPart()
        {
            yield return new MatrixPart(new int[,] { { 2, 9 }, { 3, 9 } }) { Operation = MatrixOperation.Summation, PartFirst = PartOfMatrix.LeftTop, PartSecond = PartOfMatrix.RightBottom };
            yield return new MatrixPart(new int[,] { { 6, 10 }, { 7, 14 } }) { Operation = MatrixOperation.Summation, PartFirst = PartOfMatrix.LeftBottom, PartSecond = PartOfMatrix.RightBottom };
            yield return new MatrixPart(new int[,] { { 3, -2 }, { 5, 1 } }) { Operation = MatrixOperation.Subtraction, PartFirst = PartOfMatrix.RightTop, PartSecond = PartOfMatrix.RightBottom };
            yield return new MatrixPart(new int[,] { { 4, 1 }, { 4, 5 } }) { Operation = MatrixOperation.Subtraction, PartFirst = PartOfMatrix.LeftBottom, PartSecond = PartOfMatrix.LeftTop };
            yield return new MatrixPart(new int[,] { { 5, 7 }, { 8, 10 } }) { Operation = MatrixOperation.Summation, PartFirst = PartOfMatrix.LeftTop, PartSecond = PartOfMatrix.RightTop };
            yield return new MatrixPart(new int[,] { { 6, 10 }, { 7, 14 } }) { Operation = MatrixOperation.Summation, PartFirst = PartOfMatrix.LeftBottom, PartSecond = PartOfMatrix.RightBottom };
        }

        [TestCaseSource(nameof(TestSourcesPart))]
        public void TestMatrixSum(MatrixPart data)
        {
            var mPart = new MatrixPart(new int[,] {
                { 1, 2, 4, 5 },
                { 3, 4, 5, 6 },
                { 5, 3, 1, 7 },
                { 7, 9, 0, 5 } });

            mPart.PartFirst = data.PartFirst;
            mPart.PartSecond = data.PartSecond;
            mPart.Operation = data.Operation;
            var result = MathExtensions.OperationsMatrix(mPart);
            Assert.AreEqual(MathExtensions.CompareContent(result, data.Data), true);
        }

        private static IEnumerable<(PartOfMatrix, int, (int iBegin, int iEnd, int jBegin, int jEnd))> GetSourceAndDataForTestGetIndexes()
        {
            yield return (PartOfMatrix.LeftTop, 8, (0, 4, 0, 4));
            yield return (PartOfMatrix.LeftBottom, 8, (4, 8, 0, 4));
            yield return (PartOfMatrix.RightTop, 8, (0, 4, 4, 8));
            yield return (PartOfMatrix.RightBottom, 8, (4, 8, 4, 8));
        }

        [TestCaseSource(nameof(GetSourceAndDataForTestGetIndexes))]
        public void TestGetIndexes((PartOfMatrix part, int dimension, (int iBegin, int iEnd, int jBegin, int jEnd) correctAnswer) data)
        {
            var result = MathExtensions.GetIndexes(data.part, data.dimension);
            Assert.AreEqual(result.iBegin, data.correctAnswer.iBegin);
            Assert.AreEqual(result.iEnd, data.correctAnswer.iEnd);
            Assert.AreEqual(result.jBegin, data.correctAnswer.jBegin);
            Assert.AreEqual(result.jEnd, data.correctAnswer.jEnd);
        }

        private static IEnumerable<(Stack<int[,]> data, Stack<MatrixOperation> operations, int[,] correctResult)> Source4TestOperationsMatrixStack()
        {
            Stack<int[,]> data1 = new Stack<int[,]>();
            data1.Push(new int[2, 2] { { 1, 2 }, { 4, 5 } });
            data1.Push(new int[2, 2] { { 3, 2 }, { 4, 7 } });
            data1.Push(new int[2, 2] { { 9, 0 }, { 2, 6 } });
            data1.Push(new int[2, 2] { { 7, 8 }, { 3, 0 } });

            var operations1 = new Stack<MatrixOperation>();
            operations1.Push(MatrixOperation.Summation);
            operations1.Push(MatrixOperation.Subtraction);
            operations1.Push(MatrixOperation.Summation);
            yield return (data1, operations1, new int[2, 2] { { 14, 8 }, { 5, 4 } });

            Stack<int[,]> data2 = new Stack<int[,]>();
            data2.Push(new int[2, 2] { { 1, 2 }, { 4, 5 } });
            data2.Push(new int[2, 2] { { 3, 2 }, { 4, 7 } });

            var operations2 = new Stack<MatrixOperation>();
            operations2.Push(MatrixOperation.Summation);
            yield return (data2, operations2, new int[2, 2] { { 4, 4 }, { 8, 12 } });

            Stack<int[,]> data3 = new Stack<int[,]>();
            data3.Push(new int[2, 2] { { 1, 2 }, { 4, 5 } });
            data3.Push(new int[2, 2] { { 3, 2 }, { 4, 7 } });
            data3.Push(new int[2, 2] { { 9, 0 }, { 2, 6 } });
            data3.Push(new int[2, 2] { { 7, 8 }, { 3, 0 } });

            var operations3 = new Stack<MatrixOperation>();
            operations3.Push(MatrixOperation.Subtraction);
            operations3.Push(MatrixOperation.Subtraction);
            operations3.Push(MatrixOperation.Summation);
            yield return (data3, operations3, new int[2, 2] { { 12, 4 }, { -3, -6 } });
        }

        [TestCaseSource(nameof(Source4TestOperationsMatrixStack))]
        public void TestOperationsMatrixStack((Stack<int[,]> data, Stack<MatrixOperation> operations, int[,] correctResult) source)
        {
            var result = MathExtensions.OperationsMatrix(source.data, source.operations);
            Assert.AreEqual(MathExtensions.CompareContent(result, source.correctResult), true);
        }

        public static IEnumerable<(Stack<int[,]> data, int[,] correctAnswer)> Source4TestJoinMatrix()
        {
            var c11 = new int[,] { { 1, 2 }, { 3, 4 } };
            var c12 = new int[,] { { 5, 6 }, { 7, 8 } };
            var c21 = new int[,] { { 9, 10 }, { 11, 12 } };
            var c22 = new int[,] { { 1, 2 }, { 3, 4 } };
            var stackMatrix = new Stack<int[,]>();
            stackMatrix.Push(c22);
            stackMatrix.Push(c21);
            stackMatrix.Push(c12);
            stackMatrix.Push(c11);

            var correctAnswer = new int[,] { { 1, 2, 9, 10 }, { 3, 4, 11, 12 }, { 5, 6, 1, 2 }, { 7, 8, 3, 4 } };

            yield return (stackMatrix, correctAnswer);
        }

        [TestCaseSource(nameof(Source4TestJoinMatrix))]
        public void TestJoinMatrix((Stack<int[,]> data, int[,] correctAnswer) source)
        {
            var result = MathExtensions.Join(source.data);
            Assert.AreEqual(MathExtensions.CompareContent(result, source.correctAnswer), true);
        }
    }
}