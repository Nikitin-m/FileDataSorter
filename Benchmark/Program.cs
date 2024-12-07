// See https://aka.ms/new-console-template for more information

using Benchmark;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<StringBenchmark>();
// Console.WriteLine(new StringBenchmark().ByIndex());