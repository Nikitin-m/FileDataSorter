using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmark;

[MemoryDiagnoser]
public class StringSizeBenchmark
{
    public static string example =
        "211520. AAAEWDIJWWFVQTDVIEFWTFNXMMAKLRXOLSBRJWCBQIKBQRNAIQYHOPJTCVMGIFEWPBGLTVFXJSPYVDDNSLIHNSXVUKCTRJJQS";
    
    [Benchmark]
    public int EncodingWithoutAllocation()
    {
        var str = Encoding.UTF8.GetBytes(example).Length + Environment.NewLine.Length;
        return str;
    }
    
    [Benchmark]
    public int EncodingNewLineAllocation()
    {
        var str = Encoding.UTF8.GetBytes(example + Environment.NewLine).Length;
        return str;
    }
    
    [Benchmark]
    public int Length()
    {
        var str = example.Length + Environment.NewLine.Length;
        return str;
    }
}