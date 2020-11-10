using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matlab.Utils;
using Matlab.Core;

namespace Matlab.Utils
{
    public struct MatrixRowRef
    {
        /// <summary>
        /// Fill row with given value.
        /// </summary>
        /// <param name="value"></param>
        public double FillVal
        {
            set
            {
                for (var j = 0; j < mat.ColumnCount; ++j)
                {
                    this.mat.Mat[iRow, j] = value;
                }
            }
        }

        /// <summary>
        /// The row represented by a vector.
        /// </summary>
        /// <param name="source"></param>
        public VectorR Val
        {
            get => mat.Mat.Row(iRow);
            set
            {
                if (this.mat.ColumnCount != value.Count) throw new Exception();
                this.mat.Mat.SetRow(iRow, value.Vec);
            }
        }

        /// <summary>
        /// Available only when the matrix has 1 element.
        /// </summary>
        public double SingleVal
        {
            get
            {
                if (this.mat.ColumnCount != 1) throw new Exception();
                return this.mat.Mat[iRow, 0];
            }
            set
            {
                if (this.mat.ColumnCount != 1) throw new Exception();
                this.mat.Mat[iRow, 0] = value;
            }
        }

        private readonly Matrix mat;

        private readonly int iRow;

        /// <summary>
        /// Crete a row ref.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="iRow">matlab index, starts from 1</param>
        internal MatrixRowRef(Matrix mat, int iRow)
        {
            this.mat = mat;
            this.iRow = iRow - 1;  // The index in matlab matrix starts from '1'.
            if (iRow < 0) throw new Exception();
        }

        public static implicit operator VectorR(MatrixRowRef rowRef)
        {
            return rowRef.Val;
        }
    }
}
