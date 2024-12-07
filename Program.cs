using System.Diagnostics;
using System.Globalization;
using FileDataSorter;

var stopwatch = Stopwatch.StartNew();
const long sizeInBytes = 1024 * 1024 * 1024; // 1gb
var fileName = "f_1073741824.txt";//new FileGenerator().Generate(sizeInBytes);
stopwatch.Stop();
Console.WriteLine($"{fileName} generation took {stopwatch.ElapsedMilliseconds} ms");


stopwatch.Reset();
stopwatch.Start();
new InMemoryFileSorter().Sort(fileName);
stopwatch.Stop();
Console.WriteLine($"{fileName} sorting took {stopwatch.ElapsedMilliseconds} ms");

internal class InMemoryFileSorter
{
    public void Sort(string fileName)
    {
        var dict = new Dictionary<string, List<int>>();
        using var reader = File.OpenText(fileName);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null)
                continue;


            var index = line.IndexOf('.');
            // var spanLine
            // var span = line.AsSpan();
            // span.in
            // var inta = int.Parse(span.Slice(0, index), NumberStyles.Integer);
            var number = int.Parse(line.Substring(0, index));
            var str = line.Substring(index + 2);
            
            if (dict.TryGetValue(str, out var value))
                value.Add(number);
            else 
                dict.Add(str, new List<int> { number });
        }

        using var writer = new StreamWriter($"sorted_{fileName}");
        
        foreach (var strNumber in dict.OrderBy(x=> x.Key))
        {
            var numbers = strNumber.Value.ToArray();
            Array.Sort(numbers);
            foreach (var number in numbers)
            {
                writer.WriteLine($"{number}. {strNumber.Key}");
            }
        }
    }
}