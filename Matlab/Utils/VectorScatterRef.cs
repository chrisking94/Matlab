using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Matlab.Utils
{
    /// <summary>
    /// Scatter reference.
    /// </summary>
    public class VectorScatterRef
    {
        public Vector<double> Val
        {
            get => Vector<double>.Build.DenseOfEnumerable(this.indices.Select(i => this.vec[i]));
            set
            {
                // Check.
                if (value.Count != this.indices.Length) throw new Exception();
                for(var i = 0; i < this.indices.Length; ++i)
                {
                    this.vec[indices[i]] = value[i];
                }
            }
        }

        public double FillVal
        {
            set
            {
                for (var i = 0; i < this.indices.Length; ++i)
                {
                    this.vec[indices[i]] = value;
                }
            }
        }

        private readonly IList<double> vec;

        private readonly int[] indices;

        /// <summary>
        /// Matlab index.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="indices"></param>
        public VectorScatterRef(IList<double> vec, IEnumerable<double> indices)
        {
            // Check
            NumericTool.CheckIndices(indices);
            this.vec = vec;
            this.indices = indices.Select(i => (int)i - 1).ToArray();
        }
    }
}
