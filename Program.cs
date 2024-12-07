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


internal class FileSorter
{
    public void Sort(string fileName)
    {
        using var reader = File.OpenText(fileName);
        for (;;)
        {
            
        }
        
        //todo
        //split into files
        //sort files
        //merge into new one
    }
}