using System.Diagnostics;
using FileDataSorter;

var stopwatch = Stopwatch.StartNew();
const long sizeInBytes = 1024L * 1024 * 1024; // 1gb
var fileName = new FileGenerator().Generate(sizeInBytes);
stopwatch.Stop();
Console.WriteLine($"{fileName} generation took {stopwatch.Elapsed} ms");

stopwatch.Reset();
stopwatch.Start();
const int maxRowsCount = 1_500_000; // almost 100 mb
new FileSorterBoosted().Sort(fileName, maxRowsCount);
stopwatch.Stop();
Console.WriteLine($"{fileName} sorting took {stopwatch.Elapsed} ms");

// stopwatch.Reset();
// stopwatch.Start();
// new InMemoryFileSorter().Sort(fileName);
// stopwatch.Stop();
// Console.WriteLine($"{fileName} sorting in memory took {stopwatch.Elapsed}");

// stopwatch.Reset();
// stopwatch.Start();
// const long maxBatchFileSizeInBytes = 1024L * 1024 * 1024 * 1; // 100 mb
// new FileSorter().Sort(fileName, maxBatchFileSizeInBytes);
// stopwatch.Stop();
// Console.WriteLine($"{fileName} sorting took {stopwatch.Elapsed} ms");