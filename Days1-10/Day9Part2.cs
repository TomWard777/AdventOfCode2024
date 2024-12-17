using System.Text;
namespace AdventOfCode2024;

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

        var pairs = ConvertInputToPairs(input);

        //DrawFileArray(pairs);
        RunCompression(ref pairs);
        DrawFileArray(pairs);

        Console.WriteLine("RESULT = " + GetChecksumValue(pairs));
    }

    public long GetChecksumValue((int, int)[] pairs)
    {
        var ind = 0;
        long total = 0;

        foreach (var pair in pairs)
        {
            for (int i = 0; i < pair.Item2; i++)
            {
                total += pair.Item1 > -1 ? (long)pair.Item1 * ind : 0;
                ind++;
            }

        }

        return total;
    }



    public void RunCompression(ref (int, int)[] pairs)
    {
        var highestId = pairs.Select(p => p.Item1).Max();

        for (int i = highestId; i > 0; i--)
        {
            RunCompressionStep(ref pairs, i);

            // For test example only
            //DrawFileArray(pairs);
        }
    }

    public void RunCompressionStep(ref (int, int)[] pairs, int id)
    {
        var index = Array.FindIndex(pairs, p => p.Item1 == id);

        if (index == -1)
        {
            throw new Exception($"ID {id} not found");
        }

        var pairToMove = pairs[index];
        var destinationIndex = Array.FindIndex(pairs, p => p.Item1 == -1 && p.Item2 >= pairToMove.Item2);

        if (destinationIndex > -1 && destinationIndex < index)
        {
            var emptySpace = pairs[destinationIndex].Item2;

            pairs[destinationIndex] = pairToMove;
            pairs[index] = (-1, pairToMove.Item2);

            var spaceLeft = emptySpace - pairToMove.Item2;

            if (spaceLeft > 0)
            {
                pairs = InsertIntoArray<(int, int)>(pairs, (-1, spaceLeft), destinationIndex + 1);
            }

            CollectSpaces(ref pairs);
        }
    }

    public void CollectSpaces(ref (int, int)[] pairs)
    {
        for (int i = 0; i < pairs.Length - 1; i++)
        {
            if (pairs[i].Item1 == -1 && pairs[i + 1].Item1 == -1 &&
                pairs[i].Item2 > 0 && pairs[i + 1].Item2 > 0)
            {
                pairs[i] = (-1, pairs[i].Item2 + pairs[i + 1].Item2);
                pairs[i + 1] = (-1, 0);
            }
        }
    }

    public T[] InsertIntoArray<T>(T[] arr, T newEntry, int index)
    {
        var result = new T[arr.Length + 1];

        for (int i = 0; i < index; i++)
        {
            result[i] = arr[i];
        }

        result[index] = newEntry;

        for (int i = index; i < arr.Length; i++)
        {
            result[i + 1] = arr[i];
        }

        return result;
    }

    public T[] ConcatArrays<T>(T[] arr1, T[] arr2)
    {
        var result = new T[arr1.Length + arr2.Length];

        for (int i = 0; i < arr1.Length; i++)
        {
            result[i] = arr1[i];
        }

        for (int i = 0; i < arr2.Length; i++)
        {
            result[i + arr1.Length] = arr2[i];
        }

        return result;
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

    public void DrawFileArray((int, int)[] pairs)
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
