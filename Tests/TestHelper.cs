using System;

namespace MatrixMultipling.Tests
{
    public static class TestHelper
    {
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

        public static bool CompareUpTo0(this int[,] matrixSource, int[,] matrixResult)
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