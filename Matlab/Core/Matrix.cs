using MathNet.Numerics.LinearAlgebra;
using Matlab.Core.Builders;
using Matlab.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matlab.Core
{
    /// <summary>
    /// A wrapper for <see cref="Matrix{T}"/>.
    /// </summary>
    public partial class Matrix
    {
        public static readonly MatrixBuilder Build = new MatrixBuilder();

        internal Matrix<double> Mat => this.mat;

        public int RowCount => this.mat.RowCount;

        public int ColumnCount => this.mat.ColumnCount;

        private readonly Matrix<double> mat;

        public Matrix(Matrix<double> mat)
        {
            this.mat = mat;
        }

        /// <summary>
        /// Get a reference at the given <paramref name="iRow"/> and <paramref name="iCol"/> in this matrix.
        /// </summary>
        /// <param name="iRow"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public MatrixPointRef @ref(int iRow, int iCol)
        {
            return new MatrixPointRef(this, iRow, iCol);
        }

        /// <summary>
        /// Point reference for a column vector.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="iRow"></param>
        /// <returns></returns>
        public MatrixPointRef @ref(int iRow)
        {
            if (mat.ColumnCount != 1) throw new Exception($"'{nameof(mat)}' is not a column vector");
            return new MatrixPointRef(this, iRow, 1);
        }

        /// <summary>
        /// Get row reference.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="iRow"></param>
        /// <param name="colRep"></param>
        /// <returns></returns>
        public MatrixRowRef @ref(int iRow, char colRep)
        {
            if (colRep != ':') throw new Exception($"{nameof(colRep)} should be ':' constantly.");
            return new MatrixRowRef(this, iRow);
        }

        /// <summary>
        /// Get column reference.
        /// </summary>
        /// <param name="colRep"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public MatrixColRef @ref(char colRep, int iCol)
        {
            if (colRep != ':') throw new Exception($"{nameof(colRep)} should be ':' constantly.");
            return new MatrixColRef(this, iCol);
        }

        /// <summary>
        /// Slice reference.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="rowSlice"></param>
        /// <param name="colSlice"></param>
        /// <returns></returns>
        public MatrixSliceRef @ref(ValueTuple<int, int> rowSlice, ValueTuple<int, int> colSlice)
        {
            return new MatrixSliceRef(this, rowSlice, colSlice);
        }

        /// <summary>
        /// Slice reference.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="rowSlice"></param>
        /// <param name="colSlice"></param>
        /// <returns></returns>
        public MatrixSliceRef @ref(int row, ValueTuple<int, int> colSlice)
        {
            return new MatrixSliceRef(this, (row, row), colSlice);
        }

        /// <summary>
        /// Slice reference.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="rowIndices"></param>
        /// <param name="colIndices"></param>
        /// <returns></returns>
        public MatrixSliceRef @ref(IEnumerable<double> rowIndices, IEnumerable<double> colIndices)
        {
            return new MatrixSliceRef(this, rowIndices, colIndices);
        }

        /// <summary>
        /// Slice reference. All columns.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="rowIndices"></param>
        /// <param name="colRep">Should be ':' constantly.</param>
        /// <returns></returns>
        public MatrixSliceRef @ref(IEnumerable<double> rowIndices, char colRep)
        {
            if (colRep != ':') throw new Exception($"'{nameof(colRep)}' should be ':' constantly");
            return new MatrixSliceRef(this, rowIndices, Enumerable.Range(1, mat.ColumnCount).Select(i => (double)i));
        }

        /// <summary>
        /// Slice reference. All rows.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="colIndices"></param>
        /// <param name="rowRep">Should be ':' constantly.</param>
        /// <returns></returns>
        public MatrixSliceRef @ref(char rowRep, IEnumerable<double> colIndices)
        {
            if (rowRep != ':') throw new Exception($"'{nameof(rowRep)}' should be ':' constantly");
            return new MatrixSliceRef(this, Enumerable.Range(1, mat.RowCount).Select(i => (double)i), colIndices);
        }

        /// <summary>
        /// Scatter reference.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="scatterMatrix"></param>
        /// <returns></returns>
        public MatrixScatterRef @ref(Matrix<double> scatterMatrix)
        {
            return new MatrixScatterRef(this, scatterMatrix);
        }

        /// <summary>
        /// Perform point-wise calculation.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="pointCalcFunc"></param>
        /// <param name="bIgnoreNan">true: Do not apply <paramref name="pointCalcFunc"/> to double.NAN point. Keep the point be double.NAN in the returned matrix.</param>
        /// <returns></returns>
        public Matrix<double> PointWiseApply(Func<double, double> pointCalcFunc, bool bIgnoreNan = false)
        {
            var res = Matrix<double>.Build.Dense(mat.RowCount, mat.ColumnCount);

            for (var i = 0; i < mat.RowCount; ++i)
            {
                for (var j = 0; j < mat.ColumnCount; ++j)
                {
                    if (bIgnoreNan)
                    {
                        var d = mat[i, j];
                        res[i, j] = double.IsNaN(d) ? d : pointCalcFunc(d);
                    }
                    else
                    {
                        res[i, j] = pointCalcFunc(mat[i, j]);
                    }
                }
            }

            return res;
        }


        /// <summary>
        /// Apply <paramref name="columnCalcFunc"/>> to each column then return a row vector.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="columnCalcFunc"></param>
        /// <returns></returns>
        public VectorR ColumnWiseApply(Func<Vector<double>, double> columnCalcFunc)
        {
            var res = VectorR.Build.Dense(mat.ColumnCount);
            for (var j = 0; j < mat.ColumnCount; ++j)
            {
                res.Vec[j] = columnCalcFunc(mat.Column(j));
            }
            return res;
        }

        /// <summary>
        /// Point-wise NOT operation.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public Matrix<double> Not()
        {
            return this.PointWiseApply(d => double.IsNaN(d) ? double.NaN : (d == 0 ? 1 : 0));
        }

        /// <summary>
        /// To row vector.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public VectorR ToRowVector()
        {
            // Check.
            if (mat.RowCount != 1) throw new InvalidCastException($"'{nameof(mat)}' is not a row vector.");
            return mat.Row(0);
        }

        /// <summary>
        /// To column vector.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public Vector<double> ToColumnVector()
        {
            // Check.
            if (mat.ColumnCount != 1) throw new InvalidCastException($"'{nameof(mat)}' is not a column vector.");
            return mat.Column(0);
        }

        /// <summary>
        /// Unpack to a double float number if <paramref name="mat"/> contains only 1 single element. Otherwise throw an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public double ToDouble()
        {
            if (mat.RowCount == 1 && mat.ColumnCount == 1)
            {
                return mat[0, 0];
            }
            throw new InvalidOperationException($"'{nameof(mat)}' does not contain only 1 element.");
        }

        #region Operators
        public static Matrix operator ==(Matrix mat, double value)
        {
            return mat.PointWiseApply(d => d == value ? 1 : 0, true);
        }

        public static Matrix operator !=(Matrix mat, double value)
        {
            return mat.PointWiseApply(d => d != value ? 1 : 0, true);
        }

        public static Matrix operator +(Matrix mat, double value)
        {
            return mat.mat + value;
        }

        public static Matrix operator +(Matrix mat1, Matrix mat2)
        {
            return mat1.mat + mat2.mat;
        }

        public static Matrix operator -(Matrix mat, double value)
        {
            return mat.mat - value;
        }

        public static Matrix operator -(Matrix mat1, Matrix mat2)
        {
            return mat1.mat - mat2.mat;
        }

        public static Matrix operator *(Matrix mat, double value)
        {
            return mat.mat * value;
        }

        public static VectorC operator *(Matrix mat, VectorC vec)
        {
            return mat.mat * vec.Vec;
        }

        public static VectorR operator *(Matrix mat, VectorR vec)
        {
            return mat.ColumnWiseApply(col => col * vec.Vec);
        }

        public static Matrix operator /(Matrix mat, double value)
        {
            return mat.mat / value;
        }

        public static Matrix operator %(Matrix mat, double value)
        {
            return mat.mat % value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static implicit operator Matrix(Matrix<double> mat)
        {
            return new Matrix(mat);
        }

        public static implicit operator VectorC(Matrix mat)
        {
            return mat.ToColumnVector();
        }

        public static implicit operator VectorR(Matrix mat)
        {
            return mat.ToRowVector();
        }
        #endregion
    }
}
