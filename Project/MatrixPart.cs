using System;
using MatrixMultipling.Project.Enums;

namespace MatrixMultipling.Project
{
    public class MatrixPart
    {
        public int[,] Data { get; }
        public MatrixPart(int[,] v1, PartOfMatrix part)
        {
            Data = MathExtensions.CreateMatrixPart(v1, part);
        }

        public MatrixPart(int[,] v1)
        {
            Data = v1;
        }

        // todo сделать метод, который возвращает tuple с высотой и шириной матрицы, использовать его везде
        public static MatrixPart operator +(MatrixPart first, MatrixPart second)
        {
            return Operation(first, second, MatrixOperation.Summation);
        }

        public static MatrixPart operator -(MatrixPart first, MatrixPart second)
        {
            return Operation(first, second, MatrixOperation.Subtraction);
        }

        private static MatrixPart Operation(MatrixPart first, MatrixPart second, MatrixOperation operation)
        {
            if (second == null || first == null)
            {
                throw new ArgumentException("Matrix shouldn't be null");
            }
            var mSizeFirst = MathExtensions.GetMatrixSize(first.Data);
            var mSizeSecond = MathExtensions.GetMatrixSize(second.Data);
            if (mSizeFirst.height != mSizeFirst.width || mSizeFirst.height != mSizeSecond.height || mSizeSecond.height != mSizeSecond.width)
            {
                throw new ArgumentException("Matrix should be square");
            }
            var result = new int[mSizeFirst.height, mSizeFirst.height];
            for (var i = 0; i < mSizeFirst.height; i++)
            {
                for (var j = 0; j < mSizeFirst.height; j++)
                {
                    if (operation == MatrixOperation.Summation)
                    {
                        result[i, j] = first.Data[i, j] + second.Data[i, j];
                    }
                    else
                    {
                        result[i, j] = first.Data[i, j] - second.Data[i, j];
                    }
                }
            }
            return new MatrixPart(result);
        }

    }
}