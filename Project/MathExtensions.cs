using System;

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
    }
}