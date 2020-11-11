using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matlab.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matlab.Core.Tests
{
    [TestClass()]
    public class MatrixTests
    {
        private Matrix mat = Matrix.Build.DenseOfRowArrays(
            new double[] { 1, 4, 7 },
            new double[] { 2, 5, 8 },
            new double[] { 3, 6, 9 }
            );

        [TestMethod()]
        public void refTest()
        {
            Assert.AreEqual(mat.@ref(2, 2).Val, 5);
            Assert.IsTrue(mat.@ref(2, ':').Val.Equals(new double[] { 2, 5, 8 }));
        }
    }
}