namespace FileDataSorter;

internal sealed class FileSorterBoosted
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
    
    
    public void Sort(string fileName, int maxRowsCount)
    {
        var fileNames = SplitFile(fileName, maxRowsCount);
        MergeFiles(fileNames);
    }

    private List<string> SplitFile(string fileName, int maxRowsCount)
    {
        var fileNames = new List<string>();
        var parsedLines = new ParsedLine[maxRowsCount];
        using var reader = File.OpenText(fileName);
        var counter = 0;
        while (true)
        {
            var line = reader.ReadLine();
            var endOfStream = string.IsNullOrEmpty(line);

            if (endOfStream)
            {
                Array.Resize(ref parsedLines, counter);
                maxRowsCount = counter;
            }

            if (counter == maxRowsCount)
            {
                var batchFileName = $"batch{fileNames.Count + 1}.txt";
                fileNames.Add(batchFileName);
                Array.Sort(parsedLines);
                WriteLines(batchFileName, ref parsedLines);
                Array.Clear(parsedLines);
                counter = 0;
            }
            
            if (endOfStream)
                break;
            
            var parsedLine = new ParsedLine(line!);
            parsedLines[counter] = parsedLine;
            counter++;
        }
        
        return fileNames;
    }

    private void WriteLines(string fileName, ref readonly ParsedLine[] parsedLines)
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