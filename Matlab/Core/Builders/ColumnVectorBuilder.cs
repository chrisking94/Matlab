using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Matlab.Core.Builders
{
    public class ColumnVectorBuilder : VectorBuilderBase<VectorC>
    {
        public override VectorC BuildLike(double[] vals, VectorC like)
        {
            return new VectorC(Vector<double>.Build.DenseOfArray(vals));
        }

        public VectorC DenseOfArray(double[] vals)
        {
            return new VectorC(Vector<double>.Build.DenseOfArray(vals));
        }

        public VectorC Dense(int n)
        {
            return new VectorC(Vector<double>.Build.Dense(n));
        }

        public VectorC DenseOfVector(Vector<double> vec)
        {
            return new VectorC(Vector<double>.Build.DenseOfVector(vec));
        }

        public VectorC Dense(double[] items)
        {
            return new VectorC(Vector<double>.Build.Dense(items));
        }
    }
}
