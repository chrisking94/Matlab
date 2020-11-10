using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Matlab.Core;
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
    public struct VectorScatterRef<TMathNetVec, TConcreteVec>
        where TMathNetVec : Vector<double>
        where TConcreteVec : VectorBase<TMathNetVec, TConcreteVec>
    {
        public TConcreteVec Val
        {
            get
            {
                var vec = this.vec;
                return this.vec.CreateConcreteVector(this.indices.Select(i => vec.Vec[i]).ToArray());
            }
            set
            {
                // Check.
                if (value.Count != this.indices.Length) throw new Exception();
                for(var i = 0; i < this.indices.Length; ++i)
                {
                    this.vec.Vec[indices[i]] = value.Vec[i];
                }
            }
        }

        public double FillVal
        {
            set
            {
                for (var i = 0; i < this.indices.Length; ++i)
                {
                    this.vec.Vec[indices[i]] = value;
                }
            }
        }

        private readonly VectorBase<TMathNetVec, TConcreteVec> vec;

        private readonly int[] indices;

        /// <summary>
        /// Matlab index.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="indices"></param>
        public VectorScatterRef(VectorBase<TMathNetVec, TConcreteVec> vec, IEnumerable<double> indices)
        {
            // Check
            NumericTool.CheckIndices(indices);
            this.vec = vec;
            this.indices = indices.Select(i => (int)i - 1).ToArray();
        }
    }
}
