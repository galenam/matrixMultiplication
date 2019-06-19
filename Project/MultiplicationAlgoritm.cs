using System;

namespace MatrixMultipling.Project
{
    public static class MultiplicationAlgoritm
    {
        public static int[,] Strassen(int[,] first, int[,] second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentException("Matrixs are not consistent");
            }
            var fWidth = first != null ? first.GetLength(1) : 0;
            var sHeight = second != null ? second.GetLength(0) : 0;
            if (fWidth != sHeight)
            {
                throw new ArgumentException("Matrixs are not consistent");
            }
            var fHeight = first != null ? first.GetLength(0) : 0;
            var sWidth = second != null ? second.GetLength(1) : 0;

            if (fHeight != fWidth || sHeight != sWidth)
            {
                throw new ArgumentException("Matrixs are not square");
            }
            int[,] firstPower2, secondPower2;            

            if (MathExtensions.IsPowerOfTwo(fHeight))
            {
                firstPower2 = first;
                secondPower2 = second;
            }
            else
            {
                // todo : сделать матрицу квадратной и заполнить недостающее 0
                firstPower2 = MathExtensions.CreateSquareMatrixPower2(first);
                secondPower2 = MathExtensions.CreateSquareMatrixPower2(second);
            }
            // todo: исправитьfirst, second на firstPower2, secondPower2
            var values = new int[fHeight, sWidth];

            var p1 = (first[0, 0] + first[1, 1]) * (second[0, 0] + second[1, 1]);
            var p2 = (first[1, 0] + first[1, 1]) * second[0, 0];
            var p3 = first[0, 0] * (second[0, 1] - second[1, 1]);
            var p4 = first[1, 1] * (second[1, 0] - second[0, 0]);
            var p5 = (first[0, 0] + first[0, 1]) * second[1, 1];
            var p6 = (first[1, 0] - first[0, 0]) * (second[0, 0] + second[0, 1]);
            var p7 = (first[0, 1] - first[1, 1]) * (second[1, 0] + second[1, 1]);

            var c00 = p1 + p4 - p5 + p7;
            var c01 = p3 + p5;
            var c10 = p2 + p4;
            var c11 = p1 - p2 + p3 + p6;

            throw new NotImplementedException();
        }

        public static int[,] Classic(int[,] first, int[,] second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentException("Matrixs are not consistent");
            }
            var fWidth = first != null ? first.GetLength(1) : 0;
            var sHeight = second != null ? second.GetLength(0) : 0;
            if (fWidth != sHeight)
            {
                throw new ArgumentException("Matrixs are not consistent");
            }
            var fHeight = first != null ? first.GetLength(0) : 0;
            var sWidth = second != null ? second.GetLength(1) : 0;
            var values = new int[fHeight, sWidth];

            for (var j = 0; j < sWidth; j++)
            {
                for (var i = 0; i < fHeight; i++)
                {
                    for (var r = 0; r < fWidth; r++)
                    {
                        values[i, j] += first[i, r] * second[r, j];
                    }
                }
            }

            return values;
        }
    }
}