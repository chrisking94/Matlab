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
    public class MatrixRowRef
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
                    this.mat[iRow, j] = value;
                }
            }
        }

        /// <summary>
        /// The row represented by a vector.
        /// </summary>
        /// <param name="source"></param>
        public VectorR Val
        {
            get
            {
                var vec = VectorR.Build.Dense(mat.Row(iRow));
                return vec;
            }
            set
            {
                if (this.mat.ColumnCount != value.Count) throw new Exception();
                this.mat.SetRow(iRow, value.Transpose.Vec);
            }
        }

        /// <summary>
        /// Available only when the matrix has 1 single column.
        /// </summary>
        public double RowVal
        {
            get
            {
                if (this.mat.ColumnCount != 1) throw new Exception();
                return this.mat[iRow, 0];
            }
            set
            {
                if (this.mat.ColumnCount != 1) throw new Exception();
                this.mat[iRow, 0] = value;
            }
        }

        private readonly Matrix<double> mat;

        private readonly int iRow;

        /// <summary>
        /// Crete a row ref.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="iRow">matlab index, starts from 1</param>
        public MatrixRowRef(Matrix<double> mat, int iRow)
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
