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

            //s1 = b12-b22
            var s1 = b12 - b22;
            //s2=a11+a12
            var s2 = a11 + a12;
            //s3=a21+a22
            var s3 = a21 + a22;
            //s4=b21-b11;
            var s4 = b21 - b11;
            //s5=a11+a22
            var s5 = a11 + a22;
            //s6=b11+b22
            var s6 = b11 + b22;
            //s7=a12-a22
            var s7 = a12 - a22;
            //s8=b21+b22
            var s8 = b21 + b22;
            //s9=a11-a21
            var s9 = a11 - a21;
            //s10=b11+b12
            var s10 = b11 + b12;

            int[,] p1, p2, p3, p4, p5, p6, p7;

            if (a11.Data.GetLength(0) == 1)
            {
                p1 = Classic(a11.Data, s1.Data);
            }
            else
            {
                p1 = StrassenInner(a11.Data, s1.Data);
            }
            if (b22.Data.GetLength(0) == 1)
            {
                p2 = Classic(s2.Data, b22.Data);
            }
            else
            {
                p2 = StrassenInner(s2.Data, b22.Data);
            }
            if (b11.Data.GetLength(0) == 1)
            {
                p3 = Classic(s3.Data, b11.Data);
            }
            else
            {
                p3 = StrassenInner(s3.Data, b11.Data);
            }
            if (a22.Data.GetLength(0) == 1)
            {
                p4 = Classic(a22.Data, s4.Data);
            }
            else
            {
                p4 = StrassenInner(a22.Data, s4.Data);
            }
            if (s5.Data.GetLength(0) == 1)
            {
                p5 = Classic(s5.Data, s6.Data);
            }
            else
            {
                p5 = StrassenInner(s5.Data, s6.Data);
            }
            if (s7.Data.GetLength(0) == 1)
            {
                p6 = Classic(s7.Data, s8.Data);
            }
            else
            {
                p6 = StrassenInner(s7.Data, s8.Data);
            }
            if (s9.Data.GetLength(0) == 1)
            {
                p7 = Classic(s9.Data, s10.Data);
            }
            else
            {
                p7 = StrassenInner(s9.Data, s10.Data);
            }

            //var c11 = p5 + p4 - p2 + p6;
            var stactC11 = new Stack<int[,]>();
            stactC11.Push(p6);
            stactC11.Push(p2);
            stactC11.Push(p4);
            stactC11.Push(p5);

            var stackOperationsC11 = new Stack<MatrixOperation>();
            stackOperationsC11.Push(MatrixOperation.Summation);
            stackOperationsC11.Push(MatrixOperation.Subtraction);
            stackOperationsC11.Push(MatrixOperation.Summation);

            int[,] c11 = MathExtensions.OperationsMatrix(stactC11, stackOperationsC11);

            //var c12 = p1 + p2;
            var stactC12 = new Stack<int[,]>();
            stactC12.Push(p1);
            stactC12.Push(p2);

            var stackOperationsC12_21 = new Stack<MatrixOperation>();
            stackOperationsC12_21.Push(MatrixOperation.Summation);

            int[,] c12 = MathExtensions.OperationsMatrix(stactC12, stackOperationsC12_21);

            //var c21 = p3 + p4;
            var stactC21 = new Stack<int[,]>();
            stactC21.Push(p3);
            stactC21.Push(p4);

            int[,] c21 = MathExtensions.OperationsMatrix(stactC21, stackOperationsC12_21);

            //var c11 = p5 + p1 - p3 - p7;
            var stactC22 = new Stack<int[,]>();
            stactC22.Push(p7);
            stactC22.Push(p3);
            stactC22.Push(p1);
            stactC22.Push(p5);

            var stackOperationsC22 = new Stack<MatrixOperation>();
            stackOperationsC22.Push(MatrixOperation.Subtraction);
            stackOperationsC22.Push(MatrixOperation.Subtraction);
            stackOperationsC22.Push(MatrixOperation.Summation);

            int[,] c22 = MathExtensions.OperationsMatrix(stactC22, stackOperationsC22);

            var matrixStack = new Stack<int[,]>();
            matrixStack.Push(c22);
            matrixStack.Push(c21);
            matrixStack.Push(c12);
            matrixStack.Push(c11);
            var result = MathExtensions.Join(matrixStack);
            return result;
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