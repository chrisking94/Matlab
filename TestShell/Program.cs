using System;

namespace TestShell
{
    class Program
    {
        static void Main(string[] args)
        {
            new Matlab.Core.Tests.MatrixTests().refTest();
        }
    }
}
