using MathNet.Numerics.LinearAlgebra;
using Matlab.Core;
using Matlab.Utils;

namespace Matlab.Utils
{
    public struct MatrixColRef
    {
        /// <summary>
        /// Column vector.
        /// </summary>
        public VectorC Val
        {
            get => this.mat.Mat.Column(iCol);
            set => this.mat.Mat.SetColumn(iCol, value.Vec);
        }

        private readonly Matrix mat;

        private readonly int iCol;

        /// <summary>
        /// Matlab index.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="iCol"></param>
        internal MatrixColRef(Matrix mat, int iCol)
        {
            this.mat = mat;
            this.iCol = iCol - 1;
        }
    }
}
