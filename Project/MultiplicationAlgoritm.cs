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
                firstPower2 = MathExtensions.CreateSquareMatrixPower2(first);
                secondPower2 = MathExtensions.CreateSquareMatrixPower2(second);
            }

            var newHeigth = firstPower2.GetLength(0);
            var newWidth = firstPower2.GetLength(1);
            var values = new int[newHeigth, newWidth];

            /*  todo: идея : разделить матрицы на 4 подматрицы с размером n/2 * n/2
            вычислить 10 подматриц с операциями сложения-вычитания firstPower2[0, 0] + firstPower2[1, 1]
            если размер матрицы = 1, то выполнить скалярное умножение
            иначе рекурсивно делить матрицы на подматрицы дальше
            */
            var matrixPartFirst = new MatrixPart(firstPower2);
            var matrixPartSecond = new MatrixPart(secondPower2);
            //s1 = b12-b22
            matrixPartSecond.Operation = MatrixOperation.Subtraction;
            matrixPartSecond.PartFirst = PartOfMatrix.RightTop;
            matrixPartSecond.PartSecond = PartOfMatrix.RightBottom;
            var s1 = MathExtensions.OperationsMatrix(matrixPartSecond);
            //s2=a11+a12
            matrixPartFirst.Operation = MatrixOperation.Summation;
            matrixPartFirst.PartFirst = PartOfMatrix.LeftTop;
            matrixPartFirst.PartSecond = PartOfMatrix.RightTop;
            var s2 = MathExtensions.OperationsMatrix(matrixPartFirst);
            //s3=a21+a22
            matrixPartFirst.Operation = MatrixOperation.Summation;
            matrixPartFirst.PartFirst = PartOfMatrix.LeftBottom;
            matrixPartFirst.PartSecond = PartOfMatrix.RightBottom;
            var s3 = MathExtensions.OperationsMatrix(matrixPartFirst);
            //s4=b21-b11
            matrixPartSecond.Operation = MatrixOperation.Subtraction;
            matrixPartSecond.PartFirst = PartOfMatrix.LeftBottom;
            matrixPartSecond.PartSecond = PartOfMatrix.LeftTop;
            var s4 = MathExtensions.OperationsMatrix(matrixPartSecond);
            //s5=a11+a22
            matrixPartFirst.Operation = MatrixOperation.Summation;
            matrixPartFirst.PartFirst = PartOfMatrix.LeftTop;
            matrixPartFirst.PartSecond = PartOfMatrix.RightBottom;
            var s5 = MathExtensions.OperationsMatrix(matrixPartFirst);
            //s6=b11+b22
            matrixPartSecond.Operation = MatrixOperation.Summation;
            matrixPartSecond.PartFirst = PartOfMatrix.LeftTop;
            matrixPartSecond.PartSecond = PartOfMatrix.RightBottom;
            var s6 = MathExtensions.OperationsMatrix(matrixPartSecond);
            //s7=a12-a22
            matrixPartFirst.Operation = MatrixOperation.Subtraction;
            matrixPartFirst.PartFirst = PartOfMatrix.RightTop;
            matrixPartFirst.PartSecond = PartOfMatrix.RightBottom;
            var s7 = MathExtensions.OperationsMatrix(matrixPartFirst);
            //s8=b21+b22
            matrixPartSecond.Operation = MatrixOperation.Summation;
            matrixPartSecond.PartFirst = PartOfMatrix.LeftBottom;
            matrixPartSecond.PartSecond = PartOfMatrix.RightBottom;
            var s8 = MathExtensions.OperationsMatrix(matrixPartSecond);
            //s9=a11-a21
            matrixPartFirst.Operation = MatrixOperation.Subtraction;
            matrixPartFirst.PartFirst = PartOfMatrix.LeftTop;
            matrixPartFirst.PartSecond = PartOfMatrix.LeftBottom;
            var s9 = MathExtensions.OperationsMatrix(matrixPartFirst);
            //s10=b11+b12
            matrixPartSecond.Operation = MatrixOperation.Summation;
            matrixPartSecond.PartFirst = PartOfMatrix.LeftTop;
            matrixPartSecond.PartSecond = PartOfMatrix.RightTop;
            var s10 = MathExtensions.OperationsMatrix(matrixPartSecond);
            // выделить подматрицы без операций : b11, a11, a22, b22

            var a11 = MathExtensions.CreateMatrixPart(firstPower2, PartOfMatrix.LeftTop);
            var a22 = MathExtensions.CreateMatrixPart(firstPower2, PartOfMatrix.RightBottom);
            var b11 = MathExtensions.CreateMatrixPart(secondPower2, PartOfMatrix.LeftTop);
            var b22 = MathExtensions.CreateMatrixPart(secondPower2, PartOfMatrix.RightBottom);

            int[,] p1, p2, p3, p4, p5, p6, p7;

            if (IsSmallEnough(firstPower2.GetLength(0)))
            {
                p1 = Classic(a11, s1);
                p2 = Classic(s2, b22);
                p3 = Classic(s3, b11);
                p4 = Classic(a22, s4);
                p5 = Classic(s5, s6);
                p6 = Classic(s7, s8);
                p7 = Classic(s9, s10);
            }
            else
            {
                p1 = Strassen(a11, s1);
                p2 = Strassen(s2, b22);
                p3 = Strassen(s3, b11);
                p4 = Strassen(a22, s4);
                p5 = Strassen(s5, s6);
                p6 = Strassen(s7, s8);
                p7 = Strassen(s9, s10);
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