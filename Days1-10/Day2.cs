namespace AdventOfCode2024;

public class Day2
{
    public void Run()
    {
        var input = FileParser.ReadInputFromFile("Day2.txt");

        var reportList = new List<int[]>();

        foreach (var line in input)
        {
            reportList.Add(GetValues(line));
        }

        var ct1 = 0;
        var ct2 = 0;
        var ct3 = 0;

        foreach (var report in reportList)
        {
            if (IsReportSafe(report))
            {
                ct1++;
            }

            Console.WriteLine(IsReportSafe3(report));

            if (IsReportSafe2(report))
            {
                ct2++;
            }

            if (IsReportSafe3(report))
            {
                ct3++;
            }
        }

        Console.WriteLine("RESULT1  =  " + ct1);
        Console.WriteLine("RESULT2  =  " + ct2);
        Console.WriteLine("RESULT3  =  " + ct3 + "\n");
    }

    public bool IsReportSafe3(int[] report)
    {
        if (IsReportSafe(report))
        {
            return true;
        }

        var len = report.Length;

        for (int i = 0; i < len; i++)
        {
            var reportList = report.ToList();
            reportList.RemoveAt(i);

            if (IsReportSafe(reportList.ToArray()))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsReportSafe2(int[] report)
    {
        var diffs = GetDiffs(report);
        var index = GetFirstUnsafeDiffIndex(diffs);

        if (index == -1 || index == 0 || index == diffs.Count() - 1)
        {
            return true;
        }

        var d1 = diffs[index];

        diffs.RemoveAt(index);
        diffs[index] = diffs[index] - d1;

        return AreDiffsSafe(diffs);
    }

    public bool IsReportSafe(int[] report) => AreDiffsSafe(GetDiffs(report));

    public bool AreDiffsSafe(List<int> diffs) => GetFirstUnsafeDiffIndex(diffs) == -1;

    public int GetFirstUnsafeDiffIndex(List<int> diffs)
    {
        var isPositive = diffs[0] > 0;
        var i = 0;

        foreach (var d in diffs)
        {
            if (d == 0 || isPositive && d < 0 || !isPositive && d > 0 || Math.Abs(d) > 3)
            {
                return i;
            }

            i++;
        }

        return -1;
    }

    public List<int> GetDiffs(IReadOnlyList<int> arr)
    {
        var len = arr.Count();
        var diffs = new List<int>();

        for (int i = 0; i < len - 1; i++)
        {
            diffs.Add(arr[i + 1] - arr[i]);
        }

        return diffs;
    }

    public int[] GetValues(string line)
    {
        return line
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(t => int.Parse(t))
        .ToArray();
    }
}