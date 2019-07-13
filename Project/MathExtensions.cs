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

        public static bool CompareContent(this int[,] first, int[,] second)
        {
            if (second == null && first == null) return true;
            if (second == null || first == null) return false;

            if (object.ReferenceEquals(first, second)) return true;

            var firstWidth = first.GetLength(1);
            var secondWidth = second.GetLength(1);
            var firstHeight = first.GetLength(0);
            var secondHeight = second.GetLength(0);

            if (firstWidth != secondWidth || firstHeight != secondHeight) return false;

            for (var i = 0; i < firstHeight; i++)
            {
                for (var j = 0; j < firstWidth; j++)
                {
                    if (first[i, j] != second[i, j]) return false;
                }
            }
            return true;
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

        public static object CompareUpTo0(int[,] matrixSource, int[,] matrixResult)
        {
            if (matrixResult == null && matrixSource == null) return true;
            if (matrixSource.GetLength(0) != matrixSource.GetLength(1) || matrixResult.GetLength(0) != matrixResult.GetLength(1))
            {
                throw new ArgumentException("Matrixs are not square");
            }
            for (var i = 0; i < matrixSource.GetLength(0); i++)
            {
                for (var j = 0; j > matrixSource.GetLength(0); j++)
                {
                    if (matrixSource[i, j] != matrixResult[i, j]) return false;
                }
            }

            for (var i = matrixSource.GetLength(0); i < matrixResult.GetLength(0); i++)
            {
                for (var j = matrixSource.GetLength(1); j < matrixResult.GetLength(1); j++)
                {
                    if (matrixResult[i, j] != 0) return false;
                }
            }
            return true;
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
    }
}