using MathNet.Numerics.LinearAlgebra;
using Matlab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matlab.Utils
{
    /// <summary>
    /// Reference to a vector item.
    /// </summary>
    /// <typeparam name="TMathNetVec"></typeparam>
    /// <typeparam name="TConcreteVec"></typeparam>
    public struct VectorPointRef<TMathNetVec, TConcreteVec> 
        where TMathNetVec : Vector<double>
        where TConcreteVec : VectorBase<TMathNetVec, TConcreteVec>
    {
        public double Val
        {
            get => this.vec.Vec[i];
            set => this.vec.Vec[i] = value;
        }

        private readonly VectorBase<TMathNetVec, TConcreteVec> vec;

        private readonly int i;

        /// <summary>
        /// Create a vector point ref.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="i">matlab index, starts from 1</param>
        internal VectorPointRef(VectorBase<TMathNetVec, TConcreteVec> vec, int i)
        {
            this.vec = vec;
            this.i = i - 1;
        }

        public static implicit operator double(VectorPointRef<TMathNetVec, TConcreteVec> vpr)
        {
            return vpr.Val;
        }
    }
}
