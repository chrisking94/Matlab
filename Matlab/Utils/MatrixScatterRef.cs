using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matlab.Utils
{
    /// <summary>
    /// Refer matrix by a list of indices.
    /// </summary>
    public class MatrixScatterRef
    {
        /// <summary>
        /// A column vector.
        /// </summary>
        public Matrix<double> Val
        {
            get => Matrix<double>.Build.DenseOfColumnArrays(this.indices.Select(i => mat[i % mat.RowCount, i / mat.RowCount]).ToArray());
            set
            {
                // Check.
                if (value.RowCount != indices.Count || value.ColumnCount != 1) throw new Exception("Invalid value.");
                for(var k = 0; k < indices.Count; ++k)
                {
                    var i = indices[k];
                    mat[i % mat.RowCount, i / mat.RowCount] = value[k, 0];
                }
            }
        }

        public double FillVal
        {
            set => this.indices.ForEach(i => mat[i % mat.RowCount, i / mat.RowCount] = value);
        }

        private readonly Matrix<double> mat;

        /// <summary>
        /// See <see cref="https://www.mathworks.com/company/newsletters/articles/matrix-indexing-in-matlab.html?s_tid=srchtitle"/> Linear Indexing.
        /// </summary>
        private readonly List<int> indices;

        public MatrixScatterRef(Matrix<double> mat, Matrix<double> scatterMat)
        {
            // Check dimension.
            if (mat.RowCount != scatterMat.RowCount || mat.ColumnCount != scatterMat.ColumnCount) throw new Exception("Invalid scatter matrix.");
            this.mat = mat;
            // Make indices.
            this.indices = new List<int>();
            for(var j = 0; j < scatterMat.ColumnCount; ++j)
            {
                for (var i = 0; i < scatterMat.RowCount; ++i)
                {
                    var scatterVal = scatterMat[i, j];
                    if (scatterVal == 0)
                    {
                        // Pass.
                    }
                    else if (scatterVal == 1.0)
                    {
                        indices.Add(j * scatterMat.RowCount + i);
                    }
                    else
                    {
                        throw new Exception("Invalid scatter matrix.");
                    }
                }
            }
        }

        /// <summary>
        /// Cast to a column vector.
        /// </summary>
        /// <param name="scatterRef"></param>
        public static implicit operator Matrix<double>(MatrixScatterRef scatterRef)
        {
            return scatterRef.Val;
        }
    }
}
