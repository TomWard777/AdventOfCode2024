using System.Diagnostics;

namespace AdventOfCode2024;

class Program
{
    static void Main()
    {
        var timer = new Stopwatch();
        timer.Start();

        var day = new Day24();

        day.Run();

        timer.Stop();

        Console.WriteLine("Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff"));
    }
}
