using System.Text;

namespace AdventOfCode2023;

public class Day9Part2
{
    private readonly Dictionary<char, int> _charToIntDictionary;

    public Day9Part2()
    {
        _charToIntDictionary = GetCharToIntDictionary();
    }

    public void Run()
    {
        var inputList = FileParser.ReadInputFromFile("Day9.txt");

        if (inputList.Count() != 1)
        {
            throw new Exception("Input not read correctly");
        }

        var input = inputList.First();
        //var input = "2333133121414131402";

        var arr = ConvertInputToBigArray(input);

        //DrawFileArray(arr);

        RunCompression(ref arr);

        //DrawFileArray(arr);

        Console.WriteLine("RESULT = " + GetChecksumValue(arr));
    }

    public long GetChecksumValue(long[] arr)
    {
        long total = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            var val = arr[i] > -1 ? arr[i] : 0;
            total += val * i;
        }

        return total;
    }

    public void RunCompression(ref long[] arr)
    {
        var hasChanged = true;

        while (hasChanged)
        {
            hasChanged = RunCompressionStep(ref arr);
        }
    }

    public bool RunCompressionStep(ref long[] arr)
    {
        var firstSpaceIndex = Array.IndexOf(arr, -1);

        var k = arr.Length - 1;

        while (arr[k] == -1)
        {
            k--;
        }

        if (k + 1 == firstSpaceIndex)
        {
            return false;
        }
        else
        {
            arr[firstSpaceIndex] = arr[k];
            arr[k] = -1;
            return true;
        }
    }

    public (int, int)[] ConvertInputToPairs(string input)
    {
        var list = new List<(int, int)>();

        for (int i = 0; i < input.Length / 2 + 1; i++)
        {
            var fileBlocks = _charToIntDictionary[input[2 * i]];
            list.Add((i, fileBlocks));

            if (2 * i + 1 < input.Length)
            {
                var spaceBlocks = _charToIntDictionary[input[2 * i + 1]];
                list.Add((-1, spaceBlocks));
            }
        }

        return list.ToArray();
    }

    public string DrawFileArray((int, int)[] pairs)
    {
        // This function is just for the test case.
        var sb = new StringBuilder();

        foreach (var pair in pairs)
        {
            string str = ".";

            if (pair.Item1 > -1)
            {
                str = pair.Item1.ToString();
            }

            for (int i = 0; i < pair.Item2; i++)
            {
                sb.Append(str);
            }
        }

        return sb.ToString();
    }

    private Dictionary<char, int> GetCharToIntDictionary()
    {
        return new Dictionary<char, int>
        {
            { '0', 0 },
            { '1', 1 },
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 }
        };
    }
}
