using MathNet.Numerics.LinearAlgebra;
using Matlab.Core.Builders;
using Matlab.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Matlab.Core
{
    /// <summary>
    /// The base class of VectorR and VectorC.
    /// </summary>
    /// <typeparam name="TMathNetVec"></typeparam>
    /// <typeparam name="TConcreteVec">VectorR or VectorRow</typeparam>
    public abstract class VectorBase<TMathNetVec, TConcreteVec>
        where TMathNetVec : Vector<double>
        where TConcreteVec : VectorBase<TMathNetVec, TConcreteVec>
    {
        public int Count => this.vec.Count;

        internal TMathNetVec Vec => this.vec;

        protected readonly TMathNetVec vec;

        protected abstract VectorBuilderBase<TConcreteVec> build { get; }

        protected VectorBase(TMathNetVec vec)
        {
            this.vec = vec;
        }

        #region References
        /// <summary>
        /// Get a reference at specified location.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="i">matlab index, starts from 1</param>
        /// <returns></returns>
        public VectorPointRef<TMathNetVec, TConcreteVec> @ref(int i)
        {
            return new VectorPointRef<TMathNetVec, TConcreteVec>(this, i);
        }

        /// <summary>
        /// Get a reference by given <paramref name="indices"/>.
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        public VectorScatterRef<TMathNetVec, TConcreteVec> @ref(IEnumerable<double> indices)
        {
            return new VectorScatterRef<TMathNetVec, TConcreteVec>(this, indices);
        }
        #endregion

        #region Extra Utils
        /// <summary>
        /// Unpack to a double float number if <paramref name="vec"/> contains only 1 single element. Otherwise throw an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="emptyValue">The value returned when <paramref name="vec"/> is empty.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public double ToDouble(double? emptyValue = null)
        {
            if (vec.Count == 1)
            {
                return vec[0];
            }
            else if (vec.Count == 0 && emptyValue != null)
            {
                return (double)emptyValue;
            }
            throw new InvalidOperationException($"'{nameof(vec)}' does not contain only 1 element.");
        }
        #endregion

        #region Operators
        /// <summary>
        /// Matlab [.*].
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public TConcreteVec MDP(TConcreteVec other)
        {
            return this.build.CreateMatlabVector(this.vec.PointwiseMultiply(other.vec));
        }

        public static TConcreteVec operator +(VectorBase<TMathNetVec, TConcreteVec> vec1, double value)
        {
            return vec1.build.CreateMatlabVector(vec1.vec + value);
        }

        public static TConcreteVec operator +(double value, VectorBase<TMathNetVec, TConcreteVec> vec1)
        {
            return vec1.build.CreateMatlabVector(value + vec1.vec);
        }

        public static TConcreteVec operator +(VectorBase<TMathNetVec, TConcreteVec> vec1, VectorBase<TMathNetVec, TConcreteVec> vec2)
        {
            return vec1.build.CreateMatlabVector(vec1.vec + vec2.vec);
        }

        public static TConcreteVec operator -(VectorBase<TMathNetVec, TConcreteVec> vec1, double value)
        {
            return vec1.build.CreateMatlabVector(vec1.vec - value);
        }

        public static TConcreteVec operator -(double value, VectorBase<TMathNetVec, TConcreteVec> vec1)
        {
            return vec1.build.CreateMatlabVector(value - vec1.vec);
        }

        public static TConcreteVec operator -(VectorBase<TMathNetVec, TConcreteVec> vec1, VectorBase<TMathNetVec, TConcreteVec> vec2)
        {
            return vec1.build.CreateMatlabVector(vec1.vec - vec2.vec);
        }

        public static TConcreteVec operator *(VectorBase<TMathNetVec, TConcreteVec> vec1, double value)
        {
            return vec1.build.CreateMatlabVector(vec1.vec * value);
        }

        public static TConcreteVec operator *(double value, VectorBase<TMathNetVec, TConcreteVec> vec1)
        {
            return vec1.build.CreateMatlabVector(value * vec1.vec);
        }

        public static TConcreteVec operator /(VectorBase<TMathNetVec, TConcreteVec> vec1, double value)
        {
            return vec1.build.CreateMatlabVector(vec1.vec / value);
        }

        public static TConcreteVec operator /(double value, VectorBase<TMathNetVec, TConcreteVec> vec1)
        {
            return vec1.build.CreateMatlabVector(value / vec1.vec);
        }

        public static TConcreteVec operator %(VectorBase<TMathNetVec, TConcreteVec> vec1, double value)
        {
            return vec1.build.CreateMatlabVector(vec1.vec % value);
        }

        public static TConcreteVec operator %(double value, VectorBase<TMathNetVec, TConcreteVec> vec1)
        {
            return vec1.build.CreateMatlabVector(value % vec1.vec);
        }

        public bool Equals(IEnumerable<double> other)
        {
            var otherList = other.ToList();
            if (otherList.Count == this.vec.Count)
            {
                for (var i = 0; i < this.vec.Count; ++i)
                {
                    if (this.vec[i] != otherList[i]) return false;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Private Utils
        internal TConcreteVec CreateConcreteVector(double[] vals)
        {
            return this.build.BuildLike(vals, this as TConcreteVec);
        }

        /// <summary>
        /// Apply <paramref name="pointCalcFunc"/> to each point.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="pointCalcFunc"></param>
        /// <param name="bIgnoreNan">true: Do not apply <paramref name="pointCalcFunc"/> to double.NAN point. Keep the point be double.NAN in the returned vector.</param>
        /// <returns></returns>
        private double[] PointWiseApply(Func<double, double> pointCalcFunc, bool bIgnoreNan)
        {
            var res = new double[vec.Count];
            for (var i = 0; i < vec.Count; ++i)
            {
                if (bIgnoreNan)
                {
                    var d = vec[i];
                    res[i] = double.IsNaN(d) ? d : pointCalcFunc(d);
                }
                else
                {
                    res[i] = pointCalcFunc(vec[i]);
                }
            }

            return res;
        }
        #endregion

        public override string ToString()
        {
            return this.vec.ToString();
        }
    }
}
