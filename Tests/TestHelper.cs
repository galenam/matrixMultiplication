using System;
using MatrixMultipling.Project;

namespace MatrixMultipling.Tests
{
    public static class TestHelper
    {
        public static bool CompareContent(this int[,] first, int[,] second)
        {
            if (second == null && first == null) return true;
            if (second == null || first == null) return false;

            if (object.ReferenceEquals(first, second)) return true;
            var sizeFirst = MathExtensions.GetMatrixSize(first);
            var sizeSecond = MathExtensions.GetMatrixSize(second);

            if (sizeFirst.height != sizeSecond.height || sizeFirst.width != sizeSecond.width) return false;

            for (var i = 0; i < sizeFirst.height; i++)
            {
                for (var j = 0; j < sizeFirst.width; j++)
                {
                    if (first[i, j] != second[i, j]) return false;
                }
            }
            return true;
        }

        public static bool CompareUpTo0(this int[,] matrixSource, int[,] matrixResult)
        {
            if (matrixResult == null && matrixSource == null) return true;
            if (!MathExtensions.IsMatrixSquare(matrixSource) || !MathExtensions.IsMatrixSquare(matrixResult))
            {
                throw new ArgumentException("Matrixs are not square");
            }
            var matrixSourceSize = MathExtensions.GetMatrixSize(matrixSource);
            for (var i = 0; i < matrixSourceSize.height; i++)
            {
                for (var j = 0; j > matrixSourceSize.width; j++)
                {
                    if (matrixSource[i, j] != matrixResult[i, j]) return false;
                }
            }
            var matrixResultSize = MathExtensions.GetMatrixSize(matrixResult);
            for (var i = matrixSourceSize.height; i < matrixResultSize.height; i++)
            {
                for (var j = matrixSourceSize.width; j < matrixResultSize.width; j++)
                {
                    if (matrixResult[i, j] != 0) return false;
                }
            }
            return true;
        }
    }
}