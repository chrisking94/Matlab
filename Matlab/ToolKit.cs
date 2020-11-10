using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Matlab.Utils;
using System.Data;
using MathNet.Numerics;
using System.Runtime.CompilerServices;
using Matlab.Core;

namespace Matlab
{
    /// <summary>
    /// Provide functions in C# way corresponding to MATLAB ones.
    /// <para>You might use this ToolKit by code: <code>using static Matlab.ToolKit;</code></para>
    /// </summary>
    public static class ToolKit
    {
        #region Basic math tools
        public const double Inf = double.PositiveInfinity;

        public const double Big = 1e12;

        public static double exp(double d) => Math.Exp(d);

        public static double power(double d, double n) => Math.Pow(d, n);

        public static double log(double d) => Math.Log(d);

        public static double sqrt(double d) => Math.Sqrt(d);

        public static double abs(double d) => Math.Abs(d);

        public static double floor(double d) => Math.Floor(d);

        public static int max(int d1, int d2) => Math.Max(d1, d2);
        #endregion

        #region Column Vector
        public static VectorC exp(VectorC vec) => vec.Vec.PointwiseExp();

        public static double sum(VectorC vec) => vec.Vec.Sum();

        public static int length(VectorC vec) => vec.Vec.Count;

        public static double norm(VectorC vec) => vec.Vec.Norm(2);

        public static double min(VectorC vec) => vec.Vec.Min();

        public static double max(VectorC vec) => vec.Vec.Max();

        public static VectorC floor(VectorC vec) => vec.Vec.PointwiseFloor();

        /// <summary>
        /// Returns a column vector of the linear indices of the result.
        /// <see cref="https://www.mathworks.com/help/matlab/ref/find.html?s_tid=srchtitle"/>
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static VectorC find(VectorC vec)
        {
            var noneZeroIndices = new List<double>();
            for (var i = 0; i < vec.Vec.Count; ++i)
            {
                if (vec.Vec[i] != 0)
                {
                    var index = i + 1;
                    noneZeroIndices.Add(index);
                }
            }
            return VectorC.Build.DenseOfArray(noneZeroIndices.ToArray());
        }
        #endregion

        #region Row Vector
        public static VectorR exp(VectorR vec) => vec.Vec.PointwiseExp();

        public static double sum(VectorR vec) => vec.Vec.Sum();

        public static int length(VectorR vec) => vec.Vec.Count;

        public static double norm(VectorR vec) => vec.Vec.Norm(2);

        public static double min(VectorR vec) => vec.Vec.Min();

        public static double max(VectorR vec) => vec.Vec.Max();

        /// <summary>
        /// Returns a row vector of the linear indices of the result.
        /// <see cref="https://www.mathworks.com/help/matlab/ref/find.html?s_tid=srchtitle"/>
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static VectorR find(VectorR vec)
        {
            var noneZeroIndices = new List<double>();
                for (var i = 0; i < vec.Vec.Count; ++i)
                {
                    if (vec.Vec[i] != 0)
                    {
                        var index = i + 1;
                        noneZeroIndices.Add(index);
                    }
                }
            return VectorR.Build.DenseOfArray(noneZeroIndices.ToArray());
        }
        #endregion

        #region Matrix
        public static (int m, int n) size(Matrix mat) => (mat.Mat.RowCount, mat.Mat.ColumnCount);

        public static int size(Matrix mat, int dim)
        {
            switch (dim)
            {
                case 1:
                    return mat.Mat.RowCount;
                case 2:
                    return mat.Mat.ColumnCount;
            }
            throw new ArgumentOutOfRangeException();
        }

        public static int length(Matrix mat) => Math.Max(mat.Mat.RowCount, mat.Mat.ColumnCount);

        /// <summary>
        /// <see cref="https://www.mathworks.com/help/matlab/ref/sum.html#d122e1264126"/>
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="dim"></param>
        /// <returns></returns>
        public static Matrix sum(Matrix mat, int dim)
        {
            switch (dim)
            {
                case 1:  // row vector containing the sum of each column.
                    return Matrix.Build.DenseOfRowVectors(mat.Mat.ColumnSums());
                case 2:  // column vector containing the sum of each row.
                    return Matrix.Build.DenseOfColumnVectors(mat.Mat.RowSums());
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Point-wise isinf.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix isinf(Matrix mat) => mat.PointWiseApply(d => double.IsNaN(d) ? double.NaN : (double.IsInfinity(d) ? 1 : 0));

        public static bool isempty(Matrix mat) => mat.Mat.RowCount == 0 || mat.Mat.ColumnCount == 0;

        /// <summary>
        /// Returns a column vector of the linear indices of the result.
        /// <see cref="https://www.mathworks.com/help/matlab/ref/find.html?s_tid=srchtitle"/>
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix find(Matrix mat)
        {
            var noneZeroIndices = new List<double>();
            for (var j = 0; j < mat.Mat.ColumnCount; ++j)
            {
                for (var i = 0; i < mat.Mat.RowCount; ++i)
                {
                    if (mat.Mat[i, j] != 0)
                    {
                        var index = j * mat.Mat.RowCount + i + 1;
                        noneZeroIndices.Add(index);
                    }
                }
            }
            return Matrix.Build.DenseOfColumnArrays(noneZeroIndices.ToArray());
        }

        /// <summary>
        /// Row vector containing the maximum value of each column.
        /// <see cref="https://www.mathworks.com/help/matlab/ref/max.html?s_tid=srchtitle"/>
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static VectorR max(Matrix mat) => mat.ColumnWiseApply(c => c.Max());

        /// <summary>
        /// Row vector containing the sum of each column.
        /// <see cref="https://www.mathworks.com/help/matlab/ref/sum.html?s_tid=srchtitle"/>
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static VectorR sum(Matrix mat) => mat.ColumnWiseApply(c => c.Sum());

        /// <summary>
        /// Row vector containing the minimum value of each column.
        /// <see cref="https://www.mathworks.com/help/matlab/ref/min.html?s_tid=srchtitle"/>
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static VectorR min(Matrix mat) => mat.ColumnWiseApply(c => c.Min());
        #endregion

        #region Constructors
        public static Matrix zeros(int m, int n) => Matrix.Build.Dense(m, n);

        public static Matrix zeros((int m, int n) dim) => zeros(dim.m, dim.n);

        /// <summary>
        /// Create [m x m] square matrix.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix zeros(int m) => zeros(m, m);

        public static Matrix ones(int m, int n) => Matrix.Build.Dense(m, n, 1);

        public static Matrix matrix(int m, int n, double value) => Matrix.Build.Dense(m, n, value);

        /// <summary>
        /// [m x m] squre matrix.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix ones(int m) => ones(m, m);

        /// <summary>
        /// Create a row vector. The placeHolder should be '1' constantly.
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static VectorR zeros_r(int placeHolder, int n)
        {
            if (placeHolder != 1) throw new Exception();
            var vec = VectorR.Build.Dense(n);
            return vec;
        }

        /// <summary>
        /// Create a column vector. The <paramref name="placeHolder"/> should be '1' constantly.
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static VectorC zeros_c(int n, int placeHolder)
        {
            if (placeHolder != 1) throw new Exception();
            var vec = VectorC.Build.Dense(n);
            return vec;
        }
        #endregion

        #region Other utils
        public static void printf(string format)
        {
            Console.Write(format);
        }

        public static void fprintf(string format)
        {
            Console.WriteLine(format);
        }

        public static void disp(object obj)
        {
            Console.WriteLine(obj);
        }
        #endregion
    }
}
