using MatrixMultipling.Project.Enums;

namespace MatrixMultipling.Project
{
    public class MatrixPart
    {
        public int[,] Data { get; }
        public PartOfMatrix PartFirst { get; set; }
        public PartOfMatrix PartSecond { get; set; }

        public MatrixOperation Operation { get; set; }

        public MatrixPart(int[,] v1)
        {
            this.Data = v1;
        }
    }
}