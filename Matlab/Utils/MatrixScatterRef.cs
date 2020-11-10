using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Matlab.Core;

namespace Matlab.Utils
{
    /// <summary>
    /// Refer matrix by a list of indices.
    /// </summary>
    public struct MatrixScatterRef
    {
        /// <summary>
        /// A column vector.
        /// </summary>
        public Matrix Val
        {
            get
            {
                var mat = this.mat;
                return Matrix.Build.DenseOfColumnArrays(this.indices.Select(i => mat.Mat[i % mat.RowCount, i / mat.RowCount]).ToArray());
            }
            set
            {
                // Check.
                if (value.RowCount != indices.Count || value.ColumnCount != 1) throw new Exception("Invalid value.");
                for(var k = 0; k < indices.Count; ++k)
                {
                    var i = indices[k];
                    mat.Mat[i % mat.RowCount, i / mat.RowCount] = value.Mat[k, 0];
                }
            }
        }

        public double FillVal
        {
            set
            {
                var mat = this.mat;
                this.indices.ForEach(i => mat.Mat[i % mat.RowCount, i / mat.RowCount] = value);
            }
        }

        private readonly Matrix mat;

        /// <summary>
        /// See <see cref="https://www.mathworks.com/company/newsletters/articles/matrix-indexing-in-matlab.html?s_tid=srchtitle"/> Linear Indexing.
        /// </summary>
        private readonly List<int> indices;

        internal MatrixScatterRef(Matrix mat, Matrix scatterMat)
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
                    var scatterVal = scatterMat.Mat[i, j];
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
        public static implicit operator Matrix(MatrixScatterRef scatterRef)
        {
            return scatterRef.Val;
        }
    }
}
