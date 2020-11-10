using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matlab.Utils
{
    public class VectorPointRef
    {
        public double Val
        {
            get => this.vec[i];
            set => this.vec[i] = value;
        }

        private readonly IList<double> vec;

        private readonly int i;

        /// <summary>
        /// Create a vector point ref.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="i">matlab index, starts from 1</param>
        public VectorPointRef(IList<double> vec, int i)
        {
            this.vec = vec;
            this.i = i - 1;
        }

        public static implicit operator double(VectorPointRef vpr)
        {
            return vpr.Val;
        }
    }
}
