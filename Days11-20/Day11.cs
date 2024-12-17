using System.IO.Compression;
using System.Text;

namespace AdventOfCode2024;

public class Day11
{
    public void Run()
    {
        // Real input
        var input = "6571 0 5851763 526746 23 69822 9 989";

        // Test input
        //var input = "125 17";

        var dict = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .ToDictionary(str => str, _ => (long)1);

        Console.WriteLine("Original input: " + input);

        for (var i = 0; i < 75; i++)
        {
            dict = BlinkDictionary(dict);
        }

        foreach (var key in dict.Keys)
        {
            Console.WriteLine("Key = " + key);
            Console.WriteLine("Count = " + dict[key]);
        }

        Console.WriteLine("\nRESULT = " + dict.Values.Select(x => (long)x).Sum());
    }

    public Dictionary<string, long> BlinkDictionary(Dictionary<string, long> dict)
    {
        var outputDict = new Dictionary<string, long>();

        foreach (var key in dict.Keys)
        {
            var multiplier = dict[key];

            var result = Blink(key);
            var stones = result.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (string stone in stones)
            {
                UpdateDictionary(outputDict, stone, multiplier);
            }
        }

        return outputDict;
    }

    public void UpdateDictionary(Dictionary<string, long> dict, string newKey, long val)
    {
        if (dict.ContainsKey(newKey))
        {
            dict[newKey] = dict[newKey] + val;
        }
        else
        {
            dict.Add(newKey, val);
        }
    }

    public string Blink(string input)
    {
        var arr = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();

        foreach (var numberString in arr)
        {
            sb.Append(ProcessSingleNumberString(numberString));
            sb.Append(" ");
        }

        return sb.ToString();
    }

    public string ProcessSingleNumberString(string str)
    {
        if (str == "0")
        {
            return "1";
        }

        var len = str.Length;

        if (len % 2 == 0)
        {
            var tail = str.Substring(len / 2, len / 2);
            return str.Substring(0, len / 2) + " " + ProcessTail(tail);
        }

        var n = 2024 * long.Parse(str);
        return n.ToString();
    }

    public string ProcessTail(string str)
    {
        var trimmed = str.TrimStart('0');

        if (trimmed.Length == 0)
        {
            return "0";
        }

        return trimmed;
    }
}
