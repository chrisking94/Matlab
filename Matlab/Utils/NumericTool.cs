using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matlab.Utils
{
    /// <summary>
    /// Some tools for operating <see cref="double"/>.
    /// </summary>
    public static class NumericTool
    {
        public static bool IsInteger(double d)
        {
            return d == (long)d;
        }

        public static int AsInteger(this double d)
        {
            if (IsInteger(d)) return (int)d;
            throw new InvalidCastException($"'{d}' is not a integer!");
        }

        /// <summary>
        /// Items inside <paramref name="indices"/> should all be integers.
        /// </summary>
        /// <param name="indices"></param>
        public static void CheckIndices(IEnumerable<double> indices)
        {
            var noneInts = indices.Where(d => !NumericTool.IsInteger(d)).ToArray();
            if (noneInts.Length > 0)
            {
                throw new ArgumentException($"Invalid index(ices) {string.Join("/", noneInts)}. Expect integer.");
            }
        }
    }
}
