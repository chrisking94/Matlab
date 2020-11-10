using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matlab.Core.Builders
{
    public abstract class VectorBuilderBase<TVector>
    {
        public abstract TVector BuildLike(double[] vals, TVector like);

        /// <summary>
        /// The <paramref name="vec"/> will be used as a reference.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        internal abstract TVector CreateMatlabVector(Vector<double> vec);
    }
}
