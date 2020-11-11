using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matlab.Core.Builders
{
    public class MatrixBuilder
    {
        public Matrix DenseOfRowVectors(Vector<double> vectors)
        {
            return new Matrix(Matrix<double>.Build.DenseOfRowVectors(vectors));
        }

        public Matrix DenseOfColumnVectors(Vector<double> vectors)
        {
            return new Matrix(Matrix<double>.Build.DenseOfColumnVectors(vectors));
        }

        public Matrix DenseOfColumnArrays(params double[][] vs)
        {
            return new Matrix(Matrix<double>.Build.DenseOfColumnArrays(vs));
        }

        public Matrix DenseOfRowArrays(params double[][] vs)
        {
            return new Matrix(Matrix<double>.Build.DenseOfRowArrays(vs));
        }

        public Matrix Dense(int m, int n)
        {
            return new Matrix(Matrix<double>.Build.Dense(m, n));
        }

        public Matrix Dense(int m, int n, double v)
        {
            return new Matrix(Matrix<double>.Build.Dense(m, n, v));
        }
    }
}
