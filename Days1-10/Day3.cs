using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Day3
{
    public void Run()
    {
        var list = FileParser.ReadInputFromFile("Day3.txt").ToList();
        var testInput = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

        var bigString = string.Join(string.Empty, list);
        var total = GetTotalFromString(ParseActiveString(bigString));

        Console.WriteLine("RESULT = " + total);
    }

    public string ParseActiveString(string input)
    {
        var sw = new StringWriter();
        var active = true;

        for (int i = 0; i < input.Length; i++)
        {
            var segment = input.Substring(i, Math.Min(7, input.Length - i));

            if (segment.IndexOf("don't()") > -1)
            {
                active = false;
            }
            else if (segment.IndexOf("do()") == 0)
            {
                active = true;
            }

            if (active)
            {
                sw.Write(input[i]);
                Console.Write(input[i]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(input[i]);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        Console.WriteLine("\n");

        sw.Flush();
        return sw.ToString();
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