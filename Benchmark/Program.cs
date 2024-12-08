// See https://aka.ms/new-console-template for more information

using Benchmark;
using BenchmarkDotNet.Running;

// BenchmarkRunner.Run<StringBenchmark>();
BenchmarkRunner.Run<StringSizeBenchmark>();
// Console.WriteLine(new StringBenchmark().ByIndex());