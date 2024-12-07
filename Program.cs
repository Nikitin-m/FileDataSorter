using System.Diagnostics;
using System.Globalization;
using System.Text;
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


stopwatch.Reset();
stopwatch.Start();
new FileSorter().Sort(fileName);
stopwatch.Stop();
Console.WriteLine($"{fileName} sorting took {stopwatch.ElapsedMilliseconds} ms");


internal class FileSorter
{
    public void Sort(string fileName, long maxBatchSize = 1024 * 1024 * 1024) // 1gb
    {
        var dict = new Dictionary<string, List<int>>();
        using var reader = File.OpenText(fileName);
        var currentBatchSize = 0L;
        var batchNames = new List<string>();
        while (true)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                if (currentBatchSize > 0)
                    maxBatchSize = 0; //to create batch file
                else break;
            }
            
            var span = line.AsSpan();
            var index = span.IndexOf('.');
            var number = int.Parse(span.Slice(0, index));
            var str = span.Slice(index + 2).ToString();
            
            if (dict.TryGetValue(str, out var value))
                value.Add(number);
            else 
                dict.Add(str, new List<int> { number });
            
            currentBatchSize += Encoding.UTF8.GetBytes(line).Length;
            
            if (currentBatchSize >= maxBatchSize)
            {
                var batchFileName = $"batch{batchNames.Count + 1}_{fileName}";
                batchNames.Add(batchFileName);
                CreateSortedBatch(batchFileName, dict);
                dict.Clear();
                currentBatchSize = 0;
            }
        }

        MergeFiles(fileName, batchNames);
    }

    private void CreateSortedBatch(string fileName, Dictionary<string, List<int>> dict)
    {
        using var writer = new StreamWriter(fileName);
        foreach (var strNumber in dict.OrderBy(x=> x.Key))
        {
            var numbers = strNumber.Value.ToArray();
            Array.Sort(numbers);
            foreach (var number in numbers)
            {
                writer.Write(number);
                writer.Write(". ");
                writer.WriteLine(strNumber.Key);
            }
        }
    }
    
    class Line
    {
        public Line(string line)
        {
            
        }
        var span = line.AsSpan();
        var index = span.IndexOf('.');
        var number = int.Parse(span.Slice(0, index));
        var str = span.Slice(index + 2).ToString();
    }
    
    private void MergeFiles(string fileName, List<string> fileNames)
    {

        using var writer = new StreamWriter(fileName);

        var readers = fileNames.Select(x => new StreamReader(x)).ToArray();
        var lines = readers
            .Select(x => new
            {
                Reader = x,
                Line = x.ReadLine()
            })
            .ToDictionary(x=> x.Reader, x=> x.Line);

        while (lines.Count > 0)
        {
            
        }
        
        var dict = new Dictionary<StreamReader, string?>(readers.Length);
        foreach (var reader in readers)
        {
            dict[reader] = reader.ReadToEnd();
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                dict.Add(reader, reader.ReadLine());
                continue;
            }
            
            var span = line.AsSpan();
            var index = span.IndexOf('.');
            var number = int.Parse(span.Slice(0, index));
            var str = span.Slice(index + 2).ToString();
            
            if (dict.TryGetValue(str, out var value))
                value.Add(number);
            else 
                dict.Add(str, new List<int> { number });
        }
        
        foreach (var strNumber in dict.OrderBy(x=> x.Key))
        {
            var numbers = strNumber.Value.ToArray();
            Array.Sort(numbers);
            writer.Write(numbers[0]);
            writer.Write(". ");
            writer.WriteLine(strNumber.Key);
        }

        foreach (var streamReader in readers)
        {
            streamReader.Dispose();
        }
    }
}