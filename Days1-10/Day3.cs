using System.Text.RegularExpressions;
using Microsoft.Win32.SafeHandles;

namespace AdventOfCode2023;

public class Day3
{
    public void Run()
    {
        var list = FileParser.ReadInputFromFile("Day3.txt");
        var testInput = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
        var testInput2 = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        ////Console.WriteLine("RESULT = " + GetTotalFromString(testInput));
        
        var total = 0;

        foreach(var line in list)
        {
            total += GetTotalFromString(line);
        }

        Console.WriteLine("DAY 3 RESULT = " + total);
    }

    public int GetTotalFromString(string input)
    {
        Regex rg = new Regex(@"mul\([0-9]+,[0-9]+\)");

        var matches = rg.Matches(input);
        var total = 0;

        foreach (var match in matches)
        {
            total += CalculateResultOfMatch(match.ToString());
        }

        return total;
    }

    public int CalculateResultOfMatch(string match)
    {
        match = match.Substring(4, match.Length - 5);

        var arr = match.Split(',')
        .Select(x => int.Parse(x))
        .ToArray();

        return arr[0] * arr[1];
    }
}