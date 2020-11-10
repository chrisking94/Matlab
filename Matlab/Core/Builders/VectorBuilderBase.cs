using System;
using System.Collections.Generic;
using System.Text;

namespace Matlab.Core.Builders
{
    public abstract class VectorBuilderBase<TVector>
    {
        public abstract TVector BuildLike(double[] vals, TVector like);
    }
}
