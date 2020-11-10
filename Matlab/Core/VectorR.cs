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

        protected override VectorBuilderBase<VectorR> build => Build;

        internal VectorR(Matrix<double> mat) : base(mat.Row(0))
        {
            if (mat.RowCount != 1) throw new Exception($"'{nameof(mat)}' is not a row vector.");
        }

        internal VectorR(IEnumerable<double> vs) : base(Vector<double>.Build.DenseOfEnumerable(vs))
        {
        }

        #region Operators
        public static implicit operator VectorR(Vector<double> vec)
        {
            return new VectorR(vec);
        }

        public static double operator *(VectorR vec1, VectorC vec2)
        {
            return vec1.vec * vec2.Vec;
        }
        #endregion

        #region Private Utils
        #endregion
    }
}
