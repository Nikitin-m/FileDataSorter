using System.Diagnostics;
using System.Text;
using FileDataSorter;

var stopwatch = Stopwatch.StartNew();
const long sizeInBytes = 1024 * 1024 * 1024; // 1gb
const long maxBatchSizeInBytes = 1024 * 1024 * 100; // 100 mb
var fileName = "generatedFile.txt";//new FileGenerator().Generate(sizeInBytes);
stopwatch.Stop();
Console.WriteLine($"{fileName} generation took {stopwatch.ElapsedMilliseconds} ms");

// stopwatch.Reset();
// stopwatch.Start();
// new InMemoryFileSorter().Sort(fileName);
// stopwatch.Stop();
// Console.WriteLine($"{fileName} sorting took {stopwatch.ElapsedMilliseconds} ms");

stopwatch.Reset();
stopwatch.Start();
new FileSorter().Sort(fileName, maxBatchSizeInBytes);
stopwatch.Stop();
Console.WriteLine($"{fileName} sorting took {stopwatch.ElapsedMilliseconds} ms");

internal sealed class ParsedLine : IComparable<ParsedLine>
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

    public int CompareTo(ParsedLine other)
    {
        var stringComparison = string.Compare(Str, other.Str, StringComparison.Ordinal);
        if (stringComparison != 0) 
            return stringComparison;
        return Number.CompareTo(other.Number);
    }
}


internal sealed class ParsedLineReader : IComparable<ParsedLineReader>
{
    public ParsedLine ParsedLine { get; private set; }
    public StreamReader? Reader { get; }

    public ParsedLineReader(StreamReader? streamReader)
    {
        Reader = streamReader;
        TryReadNextLine();
    }

    public bool TryReadNextLine()
    {
        var line = Reader?.ReadLine();
        if (string.IsNullOrEmpty(line))
            return false;
        
        ParsedLine = new ParsedLine(line);
        return true;
    }

    public int CompareTo(ParsedLineReader other)
    {
        return ParsedLine.CompareTo(other.ParsedLine);
    }
}

internal sealed class FileSorter
{
    public void Sort(string fileName, long maxBatchSize)
    {
        var fileNames = SplitFile(fileName, maxBatchSize);
        MergeFiles(fileNames);
    }

    private List<string> SplitFile(string fileName, long maxBatchSize)
    {
        var fileNames = new List<string>();
        var parsedLines = new List<ParsedLine>();
        using var reader = File.OpenText(fileName);
        var currentBatchSize = 0L;
        while (true)
        {
            var line = reader.ReadLine();
            var lineIsEmpty = string.IsNullOrEmpty(line);
  
            if (lineIsEmpty || currentBatchSize >= maxBatchSize)
            {
                var batchFileName = $"batch{fileNames.Count + 1}.txt";
                fileNames.Add(batchFileName);
                parsedLines.Sort();
                WriteLines(batchFileName, parsedLines);
                parsedLines.Clear();
                currentBatchSize = 0;
            }

            if (!lineIsEmpty)
            {
                var parsedLine = new ParsedLine(line!);
                parsedLines.Add(parsedLine);
                currentBatchSize += Encoding.UTF8.GetBytes(line!).Length + Environment.NewLine.Length;
            }
            else
            {
                break;
            }

        }
        
        return fileNames;
    }

    private void WriteLines(string fileName, List<ParsedLine> parsedLines)
    {
        using var writer = new StreamWriter(fileName);
        foreach (var parsedLine in parsedLines)
        {
            writer.WriteLine(parsedLine.ReaderLine);
        }
    }
    
    private void MergeFiles(List<string> fileNames)
    {
        using var writer = new StreamWriter("sortedfile.txt");
        var readers = fileNames.Select(x => new StreamReader(x)).ToArray();
        try
        {
            var parsedLineReaders = readers
                .Select(x => new ParsedLineReader(x))
                .ToList();
        
            while (parsedLineReaders.Count > 0)
            {
                parsedLineReaders.Sort();
                var readerLine = parsedLineReaders[0];
                writer.WriteLine(readerLine.ParsedLine.ReaderLine);
                if (!readerLine.TryReadNextLine())
                    parsedLineReaders.RemoveAt(0);
            }
        }
        finally
        {
            foreach (var streamReader in readers) 
                streamReader.Dispose();
        }
    }

    private void RemoveFiles(List<string> fileNames)
    {
        foreach (var fileName in fileNames)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}