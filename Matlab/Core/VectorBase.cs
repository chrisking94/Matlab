using MathNet.Numerics.LinearAlgebra;
using Matlab.Core.Builders;
using Matlab.Utils;
using System;
using System.Collections.Generic;
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
        where TMathNetVec : IList<double>
        where TConcreteVec : VectorBase<TMathNetVec, TConcreteVec>
    {
        public int Count => this.vec.Count;

        internal TMathNetVec Vec => this.vec;

        protected readonly TMathNetVec vec;

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
        public VectorPointRef @ref(int i)
        {
            return new VectorPointRef(vec, i);
        }

        public VectorScatterRef @ref(IEnumerable<double> indices)
        {
            return new VectorScatterRef(vec, indices);
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
        public abstract TConcreteVec MDP(TConcreteVec other);

        /// <summary>
        /// Matlab [.*].
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public TConcreteVec MDP(TConcreteVec vec, double val)
        {
            var resVals = vec.PointWiseApply(d => d * val, true);

            return this.CreateConcreteVector(resVals);
        }
        #endregion

        #region Private Utils
        protected abstract TConcreteVec CreateConcreteVector(double[] vals);

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
