using System;
using System.Collections.Generic;
using MatrixMultipling.Project.Enums;

namespace MatrixMultipling.Project
{
    public static class MultiplicationAlgoritm
    {
        public const int SmallMatrixDimension = 2;

        public static bool IsSmallEnough(int dimension) => dimension <= SmallMatrixDimension;
        public static int[,] Strassen(int[,] first, int[,] second)
        {
            if (!MathExtensions.IsEqualDimension(first, second))
            {
                throw new ArgumentException("Matrixs are not square");
            }
            var sourceDimension1 = MathExtensions.GetMatrixSize(first);

            int[,] firstPower2, secondPower2;
            var isPowerOf2 = MathExtensions.IsPowerOfTwo(sourceDimension1.height);

            if (isPowerOf2)
            {
                firstPower2 = first;
                secondPower2 = second;
            }
            else
            {
                firstPower2 = MathExtensions.CreateSquareMatrixPower2(first);
                secondPower2 = MathExtensions.CreateSquareMatrixPower2(second);
            }
            var result = StrassenInner(firstPower2, secondPower2);
            if (isPowerOf2)
            {
                return result;
            }
            return MathExtensions.CutNonsignificant0(result, sourceDimension1.height);
        }

        private static int[,] StrassenInner(int[,] firstPower2, int[,] secondPower2)
        {
            var sizes = MathExtensions.GetMatrixSize(firstPower2);
            var values = new int[sizes.height, sizes.width];

            /*  todo: идея : разделить матрицы на 4 подматрицы с размером n/2 * n/2
            вычислить 10 подматриц с операциями сложения-вычитания firstPower2[0, 0] + firstPower2[1, 1]
            если размер матрицы = 1, то выполнить скалярное умножение
            иначе рекурсивно делить матрицы на подматрицы дальше
            */

            var a11 = new MatrixPart(firstPower2, PartOfMatrix.LeftTop);
            var a12 = new MatrixPart(firstPower2, PartOfMatrix.RightTop);
            var a21 = new MatrixPart(firstPower2, PartOfMatrix.LeftBottom);
            var a22 = new MatrixPart(firstPower2, PartOfMatrix.RightBottom);

            var b11 = new MatrixPart(secondPower2, PartOfMatrix.LeftTop);
            var b12 = new MatrixPart(secondPower2, PartOfMatrix.RightTop);
            var b21 = new MatrixPart(secondPower2, PartOfMatrix.LeftBottom);
            var b22 = new MatrixPart(secondPower2, PartOfMatrix.RightBottom);

            var s1 = b12 - b22;
            var s2 = a11 + a12;
            var s3 = a21 + a22;
            var s4 = b21 - b11;
            var s5 = a11 + a22;
            var s6 = b11 + b22;
            var s7 = a12 - a22;
            var s8 = b21 + b22;
            var s9 = a11 - a21;
            var s10 = b11 + b12;

            var p1 = ChooseMultiplicationAlgorithm(a11, s1);
            var p2 = ChooseMultiplicationAlgorithm(s2, b22);
            var p3 = ChooseMultiplicationAlgorithm(s3, b11);
            var p4 = ChooseMultiplicationAlgorithm(a22, s4);
            var p5 = ChooseMultiplicationAlgorithm(s5, s6);
            var p6 = ChooseMultiplicationAlgorithm(s7, s8);
            var p7 = ChooseMultiplicationAlgorithm(s9, s10);

            var c11 = p5 + p4 - p2 + p6;
            var c12 = p1 + p2;
            var c21 = p3 + p4;
            var c22 = p5 + p1 - p3 - p7;

            var result = MathExtensions.Join(c11.Data, c12.Data, c21.Data, c22.Data);
            return result;
        }

        private static MatrixPart ChooseMultiplicationAlgorithm(MatrixPart first, MatrixPart second)
        {
            int[,] result;
            if (first.Data.GetLength(0) == 1)
            {
                result = Classic(first.Data, second.Data);
            }
            else
            {
                result = StrassenInner(first.Data, second.Data);
            }

            return new MatrixPart(result);
        }

        public static int[,] Classic(int[,] first, int[,] second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentException("Matrixs are not consistent");
            }
            var sizeFirst = MathExtensions.GetMatrixSize(first);
            var sizeSecond = MathExtensions.GetMatrixSize(second);

            if (sizeFirst.width != sizeSecond.height)
            {
                throw new ArgumentException("Matrixs are not consistent");
            }
            var values = new int[sizeFirst.height, sizeSecond.width];

            for (var j = 0; j < sizeSecond.width; j++)
            {
                for (var i = 0; i < sizeFirst.height; i++)
                {
                    for (var r = 0; r < sizeFirst.width; r++)
                    {
                        values[i, j] += first[i, r] * second[r, j];
                    }
                }
            }

            return values;
        }
    }
}