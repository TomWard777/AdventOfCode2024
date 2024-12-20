namespace AdventOfCode2024;

public class Day19
{
    private string[] _input;
    private string[] _towels;
    private Dictionary<string, long> _patternDictionary;

    public Day19()
    {
        _input = FileParser.ReadInputFromFile("Day19.txt").ToArray();
        //_input = FileParser.ReadInputFromFile("Test19.txt").ToArray();

        _towels = _input.First()
        .Split(", ", StringSplitOptions.RemoveEmptyEntries)
        .ToArray();

        _patternDictionary = new Dictionary<string, long>();
    }

    public void Run()
    {
        var patterns = _input.Skip(2).ToArray();
        var possiblePatterns = patterns.Where(IsPossible).ToArray();

        long total = 0;
        
        foreach (var pattern in possiblePatterns)
        {
            total += NumberOfWaysToMake(pattern);
        }

        Console.WriteLine("\nNumber of possible patterns = " + possiblePatterns.Length);
        Console.WriteLine("Total ways to make patterns = " + total);
    }

    public long NumberOfWaysToMake(string pattern)
    {
        if(_patternDictionary.ContainsKey(pattern))
        {
            return _patternDictionary[pattern];
        }

        var numberOfWays = NumberOfWaysToMakeInternal(pattern);
        _patternDictionary.Add(pattern, numberOfWays);

        return numberOfWays;
    }


    private long NumberOfWaysToMakeInternal(string pattern)
    {
        if (!IsPossible(pattern))
        {
            return 0;
        }

        if (string.IsNullOrEmpty(pattern))
        {
            return 1;
        }

        long total = 0;

        foreach (var towel in _towels)
        {
            if (pattern.StartsWith(towel))
            {
                var remainingPattern = pattern.Substring(towel.Length);

                total += NumberOfWaysToMake(remainingPattern);
            }
        }

        return total;
    }

    private bool IsPossible(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return true;
        }

        foreach (var towel in _towels)
        {
            if (pattern.StartsWith(towel))
            {
                var remainingPattern = pattern.Substring(towel.Length);

                if (IsPossible(remainingPattern))
                {
                    return true;
                }
            }
        }

        return false;
    }
}