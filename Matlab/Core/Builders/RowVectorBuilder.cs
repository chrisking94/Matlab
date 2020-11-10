using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matlab.Core.Builders
{
    public class RowVectorBuilder : VectorBuilderBase<VectorR>
    {
        public override VectorR BuildLike(double[] vals, VectorR like)
        {
            return new VectorR(vals);
        }

        public VectorR Dense(int n)
        {
            return new VectorR(Matrix<double>.Build.Dense(1, n));
        }

        public VectorR Dense(Vector<double> vec)
        {
            return new VectorR(vec);
        }

        public VectorR DenseOfArray(double[] vs)
        {
            return new VectorR(vs);
        }

        public VectorR DenseOfEnumerable(IEnumerable<double> vec)
        {
            return new VectorR(vec);
        }

        internal override VectorR CreateMatlabVector(Vector<double> vec)
        {
            throw new NotImplementedException();
        }
    }
}
