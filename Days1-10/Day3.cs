using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Day3
{
    public void Run()
    {
        var list = FileParser.ReadInputFromFile("Day3.txt");
        var testInput = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

        var total = 0;
        var total2 = 0;
        var total3 = 0;

        foreach (var line in list)
        {
            total += GetTotalFromString(line);
            var x = GetTotalFromString(GetActiveVersionOfString(line));
            total2 += x;
            Console.WriteLine(x);
            total3 += GetTotalFromString(GetInactiveVersionOfString(line));
        }

        Console.WriteLine(GetActiveVersionOfString(testInput));
        Console.WriteLine("TEST =   " + GetInactiveVersionOfString(testInput));
        Console.WriteLine("RESULT 1 = " + total);
        Console.WriteLine("RESULT 2 = " + total2);
        Console.WriteLine("RESULT 2 again = " + (total-total3));
    }

        public string GetInactiveVersionOfString(string input)
    {
        var inactiveParts = new List<string>();

        var parts = input.Split("don't()", StringSplitOptions.None);

        // if(input.IndexOf("don't()") == 0)
        // {
        //     Console.WriteLine("AAAA " + parts[0] + " BBBB");
        // }

        for (int i = 1; i < parts.Length; i++)
        {
            var part = parts[i];
            var index = part.IndexOf("do()");

            if (index > -1)
            {
                var sub = part.Substring(0, index+1);
                inactiveParts.Add(sub);
            }
            else
            {
                inactiveParts.Add(part);
            }
        }

        return string.Join("/////", inactiveParts);
    }

    public string GetActiveVersionOfString(string input)
    {
        var activeParts = new List<string>();

        var parts = input.Split("don't()", StringSplitOptions.None);

        activeParts.Add(parts[0]);

        // if(input.IndexOf("don't()") == 0)
        // {
        //     Console.WriteLine("AAAA " + parts[0] + " BBBB");
        // }

        for (int i = 1; i < parts.Length; i++)
        {
            var part = parts[i];
            var index = part.IndexOf("do()");

            if (index > -1)
            {
                var activeSubstring = part.Substring(index);
                activeParts.Add(activeSubstring);
            }
        }

        return string.Join("/////", activeParts);
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