using System.Text;

namespace FileDataSorter;

internal sealed class FileGenerator
{
    private readonly Random _random = new();
    private readonly string[] _words;

    public FileGenerator()
    {       
        _words = Enumerable.Range(1, 1000000)
            .Select(_ =>
            {
                var wordLength = Enumerable.Range(5, _random.Next(20, 100));
                var charArray = wordLength.Select(x => (char)_random.Next('A', 'Z')).ToArray();
                var str = new string(charArray);
                return str;
            }).ToArray();
    }
    public string Generate(long sizeBytes)
    {
        var fileName = $"f_{sizeBytes}.txt";
        var currentFileSize = 0L;
        using var writer = new StreamWriter(fileName);
        while (currentFileSize < sizeBytes)
        {
            var line = GetLine();
            writer.WriteLine(line);
            var currentLineSize = Encoding.UTF8.GetBytes(line + Environment.NewLine).LongLength;
            currentFileSize += currentLineSize;
        }
        
        return fileName;
    }

    private string GetLine()
    {
        return $"{GetNumber()}. {GetString()}";
    }

    private int GetNumber()
    {
        return _random.Next(0, 1000000);
    }

    private string GetString()
    {
        return _words[_random.Next(0, _words.Length)];
    }
}