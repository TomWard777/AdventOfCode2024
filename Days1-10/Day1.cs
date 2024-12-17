namespace AdventOfCode2024;

public class Day1
{
    public void Run()
    {
        var input = FileParser.ReadInputFromFile("Day1.txt");

        var list1 = new List<int>();
        var list2 = new List<int>();

        foreach (var line in input)
        {
            var pair = GetValues(line);
            list1.Add(pair[0]);
            list2.Add(pair[1]);
        }

        list1.Sort();
        list2.Sort();

        var arr1 = list1.ToArray();
        var arr2 = list2.ToArray();
        var len = arr1.Length;

        var diffsum = 0;

        for (int i = 0; i < len; i++)
        {
            diffsum += Math.Abs(arr1[i] - arr2[i]);
        }

        var result = 0;

        foreach (var n in list1)
        {
            result += n * GetSimilarityScore(n, list2);
        }

        Console.WriteLine("Sum of differences of sorted lists = " + diffsum);
        Console.WriteLine("\nFinal result = " + result);
    }

    public int GetSimilarityScore(int number, List<int> list)
    {
        var ct = 0;

        foreach (var n in list)
        {
            if (n == number)
            {
                ct++;
            }
        }

        return ct;
    }

    public int[] GetValues(string line)
    {
        return line
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(t => int.Parse(t))
        .ToArray();
    }
}