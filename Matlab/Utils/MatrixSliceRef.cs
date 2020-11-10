using MathNet.Numerics.LinearAlgebra;
using Matlab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matlab.Utils
{
    /// <summary>
    /// Referenced by slices.
    /// </summary>
    public class MatrixSliceRef
    {
        public Matrix<double> Val
        {
            get
            {
                var val = Matrix<double>.Build.Dense(this.rowIndices.Length, this.colIndices.Length);
                for(var i = 0; i < this.rowIndices.Length; ++i)
                {
                    var rowIndex = this.rowIndices[i];
                    for(var j = 0; j < this.colIndices.Length; ++j)
                    {
                        var colIndex = this.colIndices[j];
                        val[i, j] = this.mat[rowIndex, colIndex];
                    }
                }
                return val;
            }
            set
            {
                // Check size.
                if (value.RowCount != this.rowIndices.Length || value.ColumnCount != this.colIndices.Length)
                    throw new ArgumentException($"Expect {this.rowIndices.Length}x{this.colIndices.Length} matrix. Got {value.RowCount}x{value.ColumnCount}");
                for (var i = 0; i < this.rowIndices.Length; ++i)
                {
                    var rowIndex = this.rowIndices[i];
                    for (var j = 0; j < this.colIndices.Length; ++j)
                    {
                        var colIndex = this.colIndices[j];
                        this.mat[rowIndex, colIndex] = value[i, j];
                    }
                }
            }
        }

        /// <summary>
        /// Fill all rows with same row vector.
        /// </summary>
        public VectorR FillRow
        {
            set
            {
                // Check.
                if (value.Count != this.colIndices.Length) throw new ArgumentException("Inconsistent vector size.");
                if (this.rowIndices.Length != 1) throw new InvalidOperationException("Slice reference contains none or more than 1 row. Filling operation is forbidden.");
                foreach (var i in this.rowIndices)
                {
                    for (var k = 0; k < this.colIndices.Length; ++k)
                    {
                        var j = this.colIndices[k];
                        this.mat[i, j] = value.Vec[k];
                    }
                }
            }
        }

        private readonly Matrix<double> mat;

        private readonly int[] rowIndices;

        private readonly int[] colIndices;

        public MatrixSliceRef(Matrix<double> mat, ValueTuple<int, int> rowSlice, ValueTuple<int, int> colSlice)
        {
            this.mat = mat;
            this.rowIndices = Enumerable.Range(rowSlice.Item1 - 1, rowSlice.Item2 - rowSlice.Item1 + 1).ToArray();
            this.colIndices = Enumerable.Range(colSlice.Item1 - 1, colSlice.Item2 - colSlice.Item1 + 1).ToArray();
        }

        public MatrixSliceRef(Matrix<double> mat, IEnumerable<double> rowIndices, IEnumerable<double> colIndices)
        {
            // Check.
            NumericTool.CheckIndices(rowIndices);
            NumericTool.CheckIndices(colIndices);
            this.mat = mat;
            this.rowIndices = rowIndices.Select(d => (int)d - 1).ToArray();
            this.colIndices = colIndices.Select(d => (int)d - 1).ToArray();
        }

        public static implicit operator Matrix<double>(MatrixSliceRef sliceRef)
        {
            return sliceRef.Val;
        }
    }
}
