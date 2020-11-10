using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matlab.Utils;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Matlab.Core.Builders;

namespace Matlab.Core
{
    /// <summary>
    /// Row vector. See also column vector <seealso cref="VectorC"/>.
    /// </summary>
    public partial class VectorR : VectorBase<Vector<double>, VectorR>
    {
        public static readonly RowVectorBuilder Build = new RowVectorBuilder();

        public VectorC Transpose => VectorC.Build.DenseOfVector(this.vec);

        internal VectorR(Matrix<double> mat) : base(mat.Row(0))
        {
            if (mat.RowCount != 1) throw new Exception($"'{nameof(mat)}' is not a row vector.");
        }

        internal VectorR(IEnumerable<double> vs) : base(Vector<double>.Build.DenseOfEnumerable(vs))
        {
        }

        #region Operators
        public override VectorR MDP(VectorR other)
        {
            return new VectorR(this.vec.PointwiseMultiply(other.vec));
        }

        public static VectorR operator -(VectorR opr1, double d)
        {
            return new VectorR(opr1.vec - d);
        }

        public static implicit operator VectorR(Vector<double> vec)
        {
            return new VectorR(vec);
        }
        #endregion

        #region Private Utils
        protected override VectorR CreateConcreteVector(double[] vals)
        {
            return Build.BuildLike(vals, this);
        }
        #endregion
    }
}
