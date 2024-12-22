namespace AdventOfCode2024;

public class Day22
{
    // 1424 too low
    public void Run()
    {
        var input = FileParser.ReadInputFromFile("Day22.txt").Select(n => long.Parse(n)).ToArray();
        //var input = GetTestInputPart2();

        var arr = GetConsecutiveIntegers(5);
        var diffsArray = ArrayHelper.CartesianProductArray(arr, arr, arr, arr);

        var prices = new List<int>();

        var ct = diffsArray.Length;

        foreach (var diffs in diffsArray)
        {
            var p = GetTotalPrice(input, diffs);
            prices.Add(p);

            ct--;
            if (ct % 1000 == 0)
            {
                Console.Write(ct + ", ");
            }
        }

        Console.WriteLine("\nRESULT = " + prices.Max());
    }

    public int GetTotalPrice(long[] input, int[] diffs)
    {
        var total = 0;

        foreach (var n in input)
        {
            var price = GetPriceResult(n, diffs);

            if (price != -1)
            {
                total += price;
            }

            ///Console.WriteLine($"{n}  {price}");
        }

        return total;
    }

    public int GetPriceResult(long secretNumber, int[] diffs)
    {
        var seq = GetPriceSequenceWithDifferences(secretNumber, 2000).ToArray();
        return LookUpFourDifferences(seq, diffs);
    }

    public int LookUpFourDifferences((int, int)[] seq, int[] diffs)
    {
        if (diffs.Length != 4)
        {
            throw new Exception("Difference sequence should be of length 4");
        }

        for (var i = 3; i < seq.Length; i++)
        {
            var match = true;

            for (var j = 0; j < 4; j++)
            {
                match &= seq[i - j].Item2 == diffs[3 - j];
            }

            if (match == true)
            {
                return seq[i].Item1;
            }
        }

        return -1;
    }

    public IEnumerable<(int, int)> GetPriceSequenceWithDifferences(
        long secretNumber,
        int numberOfIterations)
    {
        var seq = GetNumberSequence(secretNumber, numberOfIterations)
        .Select(n => (int)(n % 10))
        .ToArray();

        for (var i = 1; i < seq.Length - 1; i++)
        {
            yield return (seq[i], seq[i] - seq[i - 1]);
        }
    }

    public IEnumerable<long> GetNumberSequence(
        long secretNumber,
        int numberOfIterations)
    {
        yield return secretNumber;

        for (var i = 0; i < numberOfIterations; i++)
        {
            secretNumber = GenerateNextWithBitShifts(secretNumber);
            yield return secretNumber;
        }
    }

    public long IterateSecretNumber(long secretNumber, int n)
    {
        for (var i = 0; i < n; i++)
        {
            secretNumber = GenerateNextWithBitShifts(secretNumber);
        }

        return secretNumber;
    }

    public long GenerateNextWithBitShifts(long secretNumber)
    {
        // 64 = 2^6.  32 = 2^5.   2048 = 2^11.
        var n = secretNumber << 6;
        secretNumber = Mix(secretNumber, n);
        secretNumber = Prune(secretNumber);

        n = secretNumber >> 5;
        secretNumber = Mix(secretNumber, n);
        secretNumber = Prune(secretNumber);

        n = secretNumber << 11;
        secretNumber = Mix(secretNumber, n);
        secretNumber = Prune(secretNumber);

        return secretNumber;
    }

    public long GenerateNext(long secretNumber)
    {
        // 64 = 2^6.  32 = 2^5.   2048 = 2^11.
        var n = secretNumber * 64;
        secretNumber = Mix(secretNumber, n);
        secretNumber = Prune(secretNumber);

        n = secretNumber / 32;
        secretNumber = Mix(secretNumber, n);
        secretNumber = Prune(secretNumber);

        n = secretNumber * 2048;
        secretNumber = Mix(secretNumber, n);
        secretNumber = Prune(secretNumber);

        return secretNumber;
    }

    public long Mix(long m, long n)
    {
        return m ^ n;
    }

    public long Prune(long n)
    {
        // 16777216 is 2 to the power 24.
        return n % 16777216;
    }

    public long[] GetTestInputPart2()
    {
        return new long[] { 1, 2, 3, 2024 };
    }

    public long[] GetTestInput()
    {
        return new long[] { 1, 10, 100, 2024 };
    }

    public int[] GetConsecutiveIntegers(int limit)
    {
        var list = new List<int>();

        for (var n = -limit; n < limit + 1; n++)
        {
            list.Add(n);
        }

        return list.ToArray();
    }
}