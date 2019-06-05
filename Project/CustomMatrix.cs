using System;

namespace MatrixMultipling.Project
{
    public class CustomMatrix : IEquatable<CustomMatrix>
    {
        public int Width
        {
            get
            {
                return Values != null ? Values.GetLength(1) : 0;
            }
        }

        public int Height
        {
            get
            {
                return Values != null ? Values.GetLength(0) : 0;
            }
        }
        public int[,] Values { get; private set; }

        public CustomMatrix(int[,] values)
        {
            if (values == null || values.GetLength(0) == 0 || values.GetLength(1) == 0)
            {
                throw new ArgumentException("Error data");
            }
            Values = new int[values.GetLength(0), values.GetLength(1)];
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    Values[i, j] = values[i, j];
                }
            }
        }

        public static CustomMatrix operator *(CustomMatrix first, CustomMatrix second)
        {
            if (first.Values == null || second.Values == null || first.Width != second.Height)
            {
                throw new ArgumentException("Matrixs are not consistent");
            }
            var values = new int[first.Height, second.Width];

            for (var j = 0; j < second.Width; j++)
            {
                for (var i = 0; i < first.Height; i++)
                {
                    for (var r = 0; r < first.Width; r++)
                    {
                        values[i, j] += first.Values[i, r] * second.Values[r, j];
                    }
                }
            }

            return new CustomMatrix(values);
        }

        public bool Equals(CustomMatrix other)
        {
            if (other == null) return false;
            if (object.ReferenceEquals(this, other)) return true;
            if (Width != other.Width || Height != other.Height) return false;
            if (Values == null && other.Values == null) return true;

            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    if (Values[i, j] != other.Values[i, j]) return false;
                }
            }
            return true;
        }
    }
}