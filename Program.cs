using System.Diagnostics;
using System.Globalization;
using System.Text;
using FileDataSorter;

var stopwatch = Stopwatch.StartNew();
const long sizeInBytes = 1024 * 1024 * 1024; // 1gb
var fileName = new FileGenerator().Generate(sizeInBytes); //"f_1073741824.txt";
stopwatch.Stop();
Console.WriteLine($"{fileName} generation took {stopwatch.ElapsedMilliseconds} ms");

// stopwatch.Reset();
// stopwatch.Start();
// new InMemoryFileSorter().Sort(fileName);
// stopwatch.Stop();
// Console.WriteLine($"{fileName} sorting took {stopwatch.ElapsedMilliseconds} ms");

stopwatch.Reset();
stopwatch.Start();
new FileSorter().Sort(fileName, 1024 * 1024 * 100); //100mb
stopwatch.Stop();
Console.WriteLine($"{fileName} sorting took {stopwatch.ElapsedMilliseconds} ms");

internal class ParsedLine : IComparable<ParsedLine>
{
    public string ReaderLine { get; }
    public int Index { get; }
    public string Str { get; }
    public int Number { get; }

    public ParsedLine(string line)
    {
        ReaderLine = line;
        var span = ReaderLine.AsSpan();
        Index = span.IndexOf('.');
        Number = int.Parse(span.Slice(0, Index));
        Str = span.Slice(Index + 2).ToString();
    }

    public int CompareTo(ParsedLine? other)
    {
        var stringComparison = string.Compare(Str, other.Str, StringComparison.Ordinal);
        if (stringComparison != 0) 
            return stringComparison;
        return Number.CompareTo(other.Number);
    }
}

internal class FileSorter
{
    private List<string> _batchFileNames;
    
    public void Sort(string fileName, long maxBatchSize = 1024 * 1024 * 1024) // 1gb
    {
        _batchFileNames = new List<string>();
        SplitFileIntoBatchFiles(fileName, maxBatchSize);
        MergeFiles(fileName);
    }

    private void SplitFileIntoBatchFiles(string fileName, long maxBatchSize)
    {
        var parsedLines = new List<ParsedLine>();
        using var reader = File.OpenText(fileName);
        var currentBatchSize = 0L;
        while (true)
        {
            var line = reader.ReadLine();
            var lineIsEmpty = string.IsNullOrEmpty(line);
            
            if (lineIsEmpty && currentBatchSize == 0)
                break;
                        
            if (lineIsEmpty || currentBatchSize >= maxBatchSize)
            {
                var batchFileName = $"batch{_batchFileNames.Count + 1}_{fileName}";
                _batchFileNames.Add(batchFileName);
                CreateSortedBatch(batchFileName, parsedLines);
                parsedLines.Clear();
                currentBatchSize = 0;
                continue;
            }

            var parsedLine = new ParsedLine(line!);
            parsedLines.Add(parsedLine);
            currentBatchSize += Encoding.UTF8.GetBytes(line!).Length + Environment.NewLine.Length;
        }
    }

    private void CreateSortedBatch(string fileName, List<ParsedLine> parsedLines)
    {
        using var writer = new StreamWriter(fileName);
        var parsedLinesArray = parsedLines.ToArray();
        Array.Sort(parsedLinesArray);
        foreach (var parsedLine in parsedLinesArray)
        {
            writer.WriteLine(parsedLine.ReaderLine);
        }
    }
    
    private void MergeFiles(string fileName)
    {
        using var writer = new StreamWriter(fileName);
        var readers = _batchFileNames.Select(x => new StreamReader(x)).ToArray();
        var readerLines = readers
            .ToDictionary(x=> new ParsedLine(x.ReadLine()), x=> x);

        while (readerLines.Count > 0)
        {
            var lines = readerLines.Keys.ToArray();
            Array.Sort(lines);
            var parsedLine = lines[0];
            var reader = readerLines[parsedLine];
            
            writer.WriteLine(lines[0].ReaderLine);
            readerLines.Remove(parsedLine);
            
            if (!reader.EndOfStream)
                readerLines.Add(new ParsedLine(reader.ReadLine()), reader);
            else 
                readerLines.Remove(parsedLine);
        }
    }

    private void RemoveBatchFiles()
    {
        foreach (var batchFileName in _batchFileNames)
        {
            if (File.Exists(batchFileName))
            {
                File.Delete(batchFileName);
            }
        }
    }
}