using System.Text;

namespace AdventOfCode2023;

public class Day9
{
    private readonly Dictionary<char, int> _charToIntDictionary;

    public Day9()
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

    public long[] ConvertInputToBigArray(string input)
    {
        var isSpace = false;
        var id = 0;
        var list = new List<long>();

        foreach (var ch in input)
        {
            var len = _charToIntDictionary[ch];
            var numberToAdd = isSpace ? -1 : id;

            list.AddRange(Enumerable.Repeat((long)numberToAdd, len));

            id = isSpace ? id : id + 1;
            isSpace = !isSpace;
        }

        return list.ToArray();
    }

    public void DrawFileArray(long[] arr)
    {
        // This function is just for the test case.
        var sb = new StringBuilder();

        foreach (var n in arr)
        {
            var str = n > -1 ? n.ToString() : ".";
            sb.Append(str);
        }

        Console.WriteLine(sb.ToString());
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
