using MathNet.Numerics.LinearAlgebra;
using Matlab.Core.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matlab.Core
{
    /// <summary>
    /// Column vector, see also <seealso cref="VectorR"/>.
    /// </summary>
    public partial class VectorC : VectorBase<Vector<double>, VectorC>
    {
        public static readonly ColumnVectorBuilder Build = new ColumnVectorBuilder();

        protected override VectorBuilderBase<VectorC> build => Build;

        public VectorC(Vector<double> vec) : base(vec)
        {

        }
        #region Operators
        public static implicit operator VectorC(Vector<double> vec)
        {
            return new VectorC(vec);
        }
        #endregion
    }
}
