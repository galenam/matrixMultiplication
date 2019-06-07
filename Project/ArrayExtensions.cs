namespace MatrixMultipling.Project
{
    public static class ArrayExtensions
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
    }
}