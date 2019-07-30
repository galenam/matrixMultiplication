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

        public static bool IsMatrixSquare(int[,] matrix)
        {
            if (matrix == null) return false;
            var sourceDimension = MathExtensions.GetMatrixSize(matrix);
            return sourceDimension.height == sourceDimension.width;
        }

        // метод Штрассена умножает только квадратные матрицы
        public static int[,] CreateSquareMatrixPower2(int[,] values)
        {
            if (!IsMatrixSquare(values))
            {
                throw new ArgumentException("Matrix is not square");
            }
            var sourceDimension = MathExtensions.GetMatrixSize(values);
            var dimension = IsPowerOfTwo(sourceDimension.height) ? sourceDimension.height : GetNearestGreater2Power(sourceDimension.height);
            var resultMatrix = new int[dimension, dimension];
            for (var i = 0; i < sourceDimension.height; i++)
            {
                for (var j = 0; j < sourceDimension.height; j++)
                {
                    resultMatrix[i, j] = values[i, j];
                }
            }
            return resultMatrix;
        }

        public static int[,] CutNonsignificant0(int[,] source, int fHeight)
        {
            if (!IsMatrixSquare(source) || fHeight < 1 || fHeight > source.GetLength(0))
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

        private static int[,] OperationMatrixInternal((int iBegin, int iEnd, int jBegin, int jEnd) indexesFirst,
        (int iBegin, int iEnd, int jBegin, int jEnd) indexesSecond, MatrixOperation operation, int[,] dataFirst, int[,] dataSecond = null)
        {
            var result = new int[indexesFirst.iEnd - indexesFirst.iBegin, indexesFirst.jEnd - indexesFirst.jBegin];
            if (dataSecond == null)
            {
                dataSecond = dataFirst;
            }
            var sourceDimension = MathExtensions.GetMatrixSize(result);
            for (var i = 0; i < sourceDimension.height; i++)
            {
                for (var j = 0; j < sourceDimension.width; j++)
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

        public static (int height, int width) GetMatrixSize(int[,] matrix)
        {
            if (matrix == null)
            {
                return (0, 0);
            }
            return (matrix.GetLength(0), matrix.GetLength(1));
        }

        public static int[,] CreateMatrixPart(int[,] v1, PartOfMatrix part)
        {
            if (v1 == null)
            {
                return new int[0, 0];
            }
            var sourceDimension = MathExtensions.GetMatrixSize(v1);
            var matrix = v1;
            if (!IsPowerOfTwo(sourceDimension.height))
            {
                matrix = CreateSquareMatrixPower2(v1);
            }

            var indexes = MathExtensions.GetIndexes(part, sourceDimension.height);
            var data = new int[indexes.iEnd - indexes.iBegin, indexes.jEnd - indexes.jBegin];
            var sourceDimensionData = MathExtensions.GetMatrixSize(data);
            for (var i = 0; i < sourceDimensionData.height; i++)
            {
                for (var j = 0; j < sourceDimensionData.width; j++)
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
                var sourceDimensionFirst = MathExtensions.GetMatrixSize(dataFirst);
                var sourceDimensionSecond = MathExtensions.GetMatrixSize(dataSecond);
                result = OperationMatrixInternal((0, sourceDimensionFirst.height, 0, sourceDimensionFirst.width),
                (0, sourceDimensionSecond.height, 0, sourceDimensionSecond.width), operation, dataFirst, dataSecond);
            }
            return result;
        }

        public static bool IsEqualDimension(int[,] first, int[,] second)
        {
            if (first == null && second == null) return true;
            var sourceDimension1 = MathExtensions.GetMatrixSize(first);
            var sourceDimension2 = MathExtensions.GetMatrixSize(second);

            if (IsMatrixSquare(first) && IsMatrixSquare(second) && sourceDimension1.height == sourceDimension2.height)
                return true;
            return false;
        }

        // матрицы д.б. квадратными и одного размера
        public static int[,] Join(int[,] c11, int[,] c12, int[,] c21, int[,] c22)
        {
            if (!IsEqualDimension(c11, c12) || !IsEqualDimension(c12, c21) || !IsEqualDimension(c21, c22))
            {
                throw new ArgumentException("Matrix should be square and equal");
            }
            var sourceDimension11 = MathExtensions.GetMatrixSize(c11);

            var result = new int[sourceDimension11.width + c12.GetLength(1), c21.GetLength(0) + c22.GetLength(0)];
            for (var i = 0; i < sourceDimension11.height; i++)
            {
                for (var j = 0; j < sourceDimension11.height; j++)
                {
                    result[i, j] = c11[i, j];
                    result[i + sourceDimension11.height, j] = c21[i, j];
                    result[i, j + sourceDimension11.width] = c12[i, j];
                    result[i + sourceDimension11.width, j + sourceDimension11.height] = c22[i, j];
                }
            }
            return result;
        }

    }
}