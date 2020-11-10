using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matlab.Core;

namespace Matlab.Utils
{
    /// <summary>
    /// Reference to a matrix point.
    /// </summary>
    public struct MatrixPointRef
    {
        public double Val
        {
            get => this.mat.Mat[iRow, iCol];
            set => this.mat.Mat[iRow, iCol] = value;
        }

        private readonly Matrix mat;

        private readonly int iRow;

        private readonly int iCol;

        /// <summary>
        /// Create a point ref.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="iRow">matlab index, starts from 1</param>
        /// <param name="iCol">matlab index, starts from 1</param>
        internal MatrixPointRef(Matrix mat, int iRow, int iCol)
        {
            this.mat = mat;
            this.iRow = iRow - 1;
            this.iCol = iCol - 1;
        }

        public static implicit operator double(MatrixPointRef pointRef)
        {
            return pointRef.Val;
        }
    }
}
