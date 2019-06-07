using System;

namespace MatrixMultipling.Project
{
    public class MultiplicationAlgoritm
    {
        int[,] first;
        int[,] second;

        public MultiplicationAlgoritm(int[,] m1Data, int[,] m2Data)
        {
            this.first = m1Data;
            this.second = m2Data;
        }

        public int[,] Strassen()
        {
            throw new NotImplementedException();
        }

        public int[,] Classic()
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