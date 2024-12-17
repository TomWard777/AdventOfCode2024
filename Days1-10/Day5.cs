using System.Data;

namespace AdventOfCode2024;

public class Day5
{
    public void Run()
    {
        var ruleStrings = FileParser.ReadInputFromFile("Day5-1.txt");
        var updateStrings = FileParser.ReadInputFromFile("Day5-2.txt");
        //var ruleStrings = FileParser.ReadInputFromFile("Test5-1.txt");
        //var updateStrings = FileParser.ReadInputFromFile("Test5-2.txt");

        var rules = new List<Rule>();

        foreach (var line in ruleStrings)
        {
            rules.Add(new Rule(line));
        }

        var total1 = 0;
        var total2 = 0;
        var incorrectUpdates = new List<string[]>();

        foreach (var update in updateStrings)
        {
            if (IsUpdateValid(rules, update))
            {
                total1 += GetMiddleNumber(update);
            }
            else
            {
                total2 += GetNumberFromIncorrectUpdate(rules, update);
            }
        }

        Console.WriteLine("RESULT 1 = " + total1);
        Console.WriteLine("RESULT 2 = " + total2);
    }

    public int GetNumberFromIncorrectUpdate(List<Rule> rules, string update)
    {
        var input = update.Split(',');
        var sortedArray = BubbleSort(rules, input);
        return int.Parse(sortedArray[(sortedArray.Length - 1) / 2]);
    }

    public string[] BubbleSort(List<Rule> rules, string[] input)
    {
        var swapsMade = false;

        do
        {
            swapsMade = BubbleSortPass(rules, ref input);
        }
        while (swapsMade);

        return input;
    }

    public bool BubbleSortPass(List<Rule> rules, ref string[] input)
    {
        var wereSwapsMade = false;

        for (int i = 0; i < input.Length - 1; i++)
        {
            if (IsXGreaterThanY(rules, input[i], input[i + 1]))
            {
                var x = input[i];
                input[i] = input[i + 1];
                input[i + 1] = x;
                wereSwapsMade = true;
            }
        }

        return wereSwapsMade;
    }

    public bool IsXGreaterThanY(List<Rule> rules, string x, string y)
    {
        return rules
        .Where(r => r.Left == y && r.Right == x)
        .Any();
    }

    public bool IsUpdateValid(List<Rule> rules, string update)
    {
        var isValid = true;

        foreach (var rule in rules)
        {
            isValid = rule.IsSatisfied(update);

            if (!isValid)
            {
                break;
            }
        }

        return isValid;
    }

    public int GetMiddleNumber(string update)
    {
        var arr = update.Split(',')
        .ToArray();

        return int.Parse(arr[(arr.Length - 1) / 2]);
    }
}

public class Rule
{
    public Rule(string str)
    {
        var parts = str.Split('|');
        Left = parts[0];
        Right = parts[1];
    }

    public string Left { get; set; }
    public string Right { get; set; }

    public bool IsSatisfied(string update)
    {
        var leftIndex = update.IndexOf(Left);

        if (leftIndex == -1)
        {
            return true;
        }

        var rightIndex = update.IndexOf(Right);

        if (rightIndex == -1)
        {
            return true;
        }

        return leftIndex < rightIndex;
    }
}