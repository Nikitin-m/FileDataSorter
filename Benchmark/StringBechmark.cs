using BenchmarkDotNet.Attributes;

namespace Benchmark;

[MemoryDiagnoser]
public class StringBenchmark
{
    public static string example =
        "211520. AAAEWDIJWWFVQTDVIEFWTFNXMMAKLRXOLSBRJWCBQIKBQRNAIQYHOPJTCVMGIFEWPBGLTVFXJSPYVDDNSLIHNSXVUKCTRJJQS";
    
    [Benchmark]
    public string Substring()
    {
        var values = example.Split('.');
        var number = int.Parse(values[0]);
        var str = values[1];
        return str;
    }
    
    [Benchmark]
    public string ByIndex()
    {
        var index = example.IndexOf('.');
        var number = int.Parse(example.Substring(0, index - 1));
        var str = example.Substring(index + 2);
        return str;
    }
}