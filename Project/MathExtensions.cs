using System;
using System.Collections.Generic;
using System.Linq;
using MatrixMultipling.Project.Enums;

namespace MatrixMultipling.Project
{
    public static class MathExtensions
    {
        public static bool IsPowerOfTwo(int x)
        {
            return (x > 1) && ((x & (x - 1)) == 0);
        }

        public static int GetNearestGreater2Power(int value)
        {
            if (IsPowerOfTwo(value)) return value;
            if (value == 1) return 2;
            return (int)Math.Pow(2, ((int)Math.Ceiling(Math.Log(value, 2))));
        }

        // метод Штрассена умножает только квадратные матрицы
        public static int[,] CreateSquareMatrixPower2(int[,] values)
        {
            if (values == null || values.GetLength(0) != values.GetLength(1))
            {
                throw new ArgumentException("Matrix is not square");
            }
            var sourceDimension = values.GetLength(0);
            var dimension = IsPowerOfTwo(sourceDimension) ? sourceDimension : GetNearestGreater2Power(sourceDimension);
            var resultMatrix = new int[dimension, dimension];
            for (var i = 0; i < sourceDimension; i++)
            {
                for (var j = 0; j < sourceDimension; j++)
                {
                    resultMatrix[i, j] = values[i, j];
                }
            }
            return resultMatrix;
        }

        public static int[,] CutNonsignificant0(int[,] source, int fHeight)
        {
            if (source == null || fHeight < 1 || fHeight > source.GetLength(0) || source.GetLength(0) != source.GetLength(1))
            {
                throw new ArgumentException("Incorrect entries");
            }
            var result = new int[fHeight, fHeight];
            for (var i = 0; i < fHeight; i++)
            {
                for (var j = 0; j < fHeight; j++)
                {
                    result[i, j] = source[i, j];
                }
            }
            return result;
        }

        public static (int iBegin, int iEnd, int jBegin, int jEnd) GetIndexes(PartOfMatrix part, int dimension)
        {
            int iBegin = 0, iEnd = 0, jBegin = 0, jEnd = 0;
            if (!IsPowerOfTwo(dimension))
            {
                return (iBegin: iBegin, iEnd: iEnd, jBegin: jBegin, jEnd: jEnd);
            }

            if (part == PartOfMatrix.LeftTop)
            {
                iBegin = 0;
                iEnd = dimension / 2;
                jBegin = 0;
                jEnd = dimension / 2;
            }
            if (part == PartOfMatrix.LeftBottom)
            {
                iBegin = dimension / 2;
                iEnd = dimension;
                jBegin = 0;
                jEnd = dimension / 2;
            }
            if (part == PartOfMatrix.RightTop)
            {
                iBegin = 0;
                iEnd = dimension / 2;
                jBegin = dimension / 2;
                jEnd = dimension;
            }
            if (part == PartOfMatrix.RightBottom)
            {
                iBegin = dimension / 2;
                iEnd = dimension;
                jBegin = dimension / 2;
                jEnd = dimension;
            }
            return (iBegin: iBegin, iEnd: iEnd, jBegin: jBegin, jEnd: jEnd);
        }

        public static int[,] OperationsMatrix(MatrixPart mPart)
        {
            if (mPart == null || mPart.Data == null) return new int[0, 0];

            var indexesFirst = GetIndexes(mPart.PartFirst, mPart.Data.GetLength(0));
            var indexesSecond = GetIndexes(mPart.PartSecond, mPart.Data.GetLength(0));
            var result = OperationMatrixInternal(indexesFirst, indexesSecond, mPart.Operation, mPart.Data);
            return result;
        }

        private static int[,] OperationMatrixInternal((int iBegin, int iEnd, int jBegin, int jEnd) indexesFirst,
        (int iBegin, int iEnd, int jBegin, int jEnd) indexesSecond, MatrixOperation operation, int[,] dataFirst, int[,] dataSecond = null)
        {
            var result = new int[indexesFirst.iEnd - indexesFirst.iBegin, indexesFirst.jEnd - indexesFirst.jBegin];
            if (dataSecond == null)
            {
                dataSecond = dataFirst;
            }
            for (var i = 0; i < result.GetLength(0); i++)
            {
                for (var j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = dataFirst[indexesFirst.iBegin + i, indexesFirst.jBegin + j];
                    if (operation == MatrixOperation.Subtraction)
                    {
                        result[i, j] -= dataSecond[indexesSecond.iBegin + i, indexesSecond.jBegin + j];
                    }
                    if (operation == MatrixOperation.Summation)
                    {
                        result[i, j] += dataSecond[indexesSecond.iBegin + i, indexesSecond.jBegin + j];
                    }
                }
            }
            return result;
        }

        public static int[,] CreateMatrixPart(int[,] v1, PartOfMatrix part)
        {
            if (v1 == null || v1.GetLength(0) == 0)
            {
                return new int[0, 0];
            }
            var matrix = v1;
            if (!IsPowerOfTwo(v1.GetLength(0)))
            {
                matrix = CreateSquareMatrixPower2(v1);
            }
            var indexes = MathExtensions.GetIndexes(part, matrix.GetLength(0));
            var data = new int[indexes.iEnd - indexes.iBegin, indexes.jEnd - indexes.jBegin];
            for (var i = 0; i < data.GetLength(0); i++)
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = matrix[indexes.iBegin + i, indexes.jBegin + j];
                }
            }
            return data;
        }

        public static int[,] OperationsMatrix(Stack<int[,]> datas, Stack<MatrixOperation> operations)
        {
            if (datas == null || !datas.Any() || operations == null || !operations.Any() || operations.Count != datas.Count - 1
            || operations.Count < 1 || datas.Count < 2)
            {
                return new int[0, 0];
            }
            int[,] result = null;
            foreach (var operation in operations)
            {
                var dataFirst = result == null ? datas.Pop() : result;
                var dataSecond = datas.Pop();
                result = OperationMatrixInternal((0, dataFirst.GetLength(0), 0, dataFirst.GetLength(1)),
                (0, dataSecond.GetLength(0), 0, dataSecond.GetLength(1)), operation, dataFirst, dataSecond);
            }
            return result;
        }

        private static bool IsEqualDimension(int[,] first, int[,] second)
        {
            if (first == null && second == null) return true;
            if (first.GetLength(0) != first.GetLength(1) || second.GetLength(0) != second.GetLength(1) ||
            first.GetLength(0) != second.GetLength(0))
                return false;
            return true;
        }

        // матрицы д.б. квадратными и одного размера
        public static int[,] Join(Stack<int[,]> matrixStack)
        {
            if (matrixStack == null || matrixStack.Count != 4)
            {
                throw new ArgumentException("Stack length should be 4");
            }
            var c11 = matrixStack.Pop();
            var c12 = matrixStack.Pop();
            var c21 = matrixStack.Pop();
            var c22 = matrixStack.Pop();

            if (!IsEqualDimension(c11, c12) || !IsEqualDimension(c12, c21) || !IsEqualDimension(c21, c22))
            {
                throw new ArgumentException("Matrix should be square and equal");
            }

            var result = new int[c11.GetLength(1) + c12.GetLength(1), c21.GetLength(0) + c22.GetLength(0)];
            for (var i = 0; i < c11.GetLength(0); i++)
            {
                for (var j = 0; j < c11.GetLength(1); j++)
                {
                    result[i, j] = c11[i, j];
                    result[i + c11.GetLength(1), j] = c21[i, j];
                    result[i, j + c11.GetLength(0)] = c12[i, j];
                    result[i + c11.GetLength(0), j + c11.GetLength(1)] = c22[i, j];
                }
            }
            return result;
        }

    }
}