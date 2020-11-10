using MathNet.Numerics.LinearAlgebra;
using Matlab.Utils;

namespace Matlab.Utils
{
    public class MatrixColRef
    {
        /// <summary>
        /// Column vector.
        /// </summary>
        public Vector<double> Val
        {
            get => this.mat.Column(iCol);
            set => this.mat.SetColumn(iCol, value);
        }

        private readonly Matrix<double> mat;

        private readonly int iCol;

        /// <summary>
        /// Matlab index.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="iCol"></param>
        public MatrixColRef(Matrix<double> mat, int iCol)
        {
            this.mat = mat;
            this.iCol = iCol - 1;
        }
    }
}
