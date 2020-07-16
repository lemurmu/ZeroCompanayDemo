using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;

namespace TestBenchmarkDotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TestContext>();
            Console.ReadLine();
        }
    }
}
