namespace FileDataSorter;

internal sealed class FileSorter
{
    private readonly record struct ParsedLineReader(StreamReader Reader, ParsedLine ParsedLine)
        : IComparable<ParsedLineReader>
    {
        public int CompareTo(ParsedLineReader other)
        {
            var lineComparison = ParsedLine.CompareTo(other.ParsedLine);
            if (lineComparison != 0)
                return lineComparison;
            return Reader.Equals(other.Reader) ? 0 : 1;
        }
    }
    
    
    public void Sort(string fileName, long maxBatchSize)
    {
        var fileNames = SplitFile(fileName, maxBatchSize);
        MergeFiles(fileNames);
    }

    private List<string> SplitFile(string fileName, long maxBatchFileSize)
    {
        var fileNames = new List<string>();
        var parsedLines = new List<ParsedLine>();
        using var reader = File.OpenText(fileName);
        var currentBatchSize = 0L;
        while (true)
        {
            var line = reader.ReadLine();
            var lineIsEmpty = string.IsNullOrEmpty(line);
  
            if (currentBatchSize >= maxBatchFileSize || lineIsEmpty)
            {
                var batchFileName = $"batch{fileNames.Count + 1}.txt";
                fileNames.Add(batchFileName);
                parsedLines.Sort();
                WriteLines(batchFileName, ref parsedLines);
                parsedLines.Clear();
                currentBatchSize = 0;
            }

            if (!lineIsEmpty)
            {
                var parsedLine = new ParsedLine(line!);
                parsedLines.Add(parsedLine);
                currentBatchSize += line!.Length + Environment.NewLine.Length;
            }
            else
            {
                break;
            }

        }
        
        return fileNames;
    }

    private void WriteLines(string fileName, ref readonly List<ParsedLine> parsedLines)
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
                .Select(x => new ParsedLineReader(x, new ParsedLine(x.ReadLine())))
                .ToList();
            
            while (parsedLineReaders.Count > 0)
            {
                parsedLineReaders.Sort();
                var readerLine = parsedLineReaders[0];
                writer.WriteLine(readerLine.ParsedLine.ReaderLine);
            
                var line = readerLine.Reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    parsedLineReaders.RemoveAt(0);
                }
                else
                {
                    parsedLineReaders[0] = new ParsedLineReader(parsedLineReaders[0].Reader, new ParsedLine(line));
                }
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