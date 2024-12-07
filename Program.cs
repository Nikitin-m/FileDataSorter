using System.Diagnostics;
using FileDataSorter;

var stopwatch = Stopwatch.StartNew();
const long sizeInBytes = 1024 * 1024 * 1024; // 1gb
var fileName = "f_1073741824.txt";//new FileGenerator().Generate(sizeInBytes * 20);
stopwatch.Stop();
Console.WriteLine($"{fileName} generation took {stopwatch.ElapsedMilliseconds} ms");
