using System.Diagnostics;

namespace AdventOfCode2023;

class Program
{
    static void Main()
    {
        var timer = new Stopwatch();
        timer.Start();

        var day = new Day8();

        day.Run();

        timer.Stop();

        Console.WriteLine("Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff"));
    }
}
