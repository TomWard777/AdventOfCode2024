using System.Security.AccessControl;

namespace AdventOfCode2024;

public class Day13
{
    string[] _separators;

    public Day13()
    {
        _separators = GetSeparators();
    }

    public void Run()
    {
        var isPart2 = true;

        var input = FileParser.ReadInputFromFile("Day13.txt");
        ///var input = FileParser.ReadInputFromFile("Test13.txt");

        var a = new int[2];
        var b = new int[2];
        var p = new int[2];

        var separators = GetSeparators();
        var machines = new List<Machine>();

        foreach (var line in input)
        {
            if (line.Contains("Button A"))
            {
                a = GetNumbersFromLine(line);
            }
            else if (line.Contains("Button B"))
            {
                b = GetNumbersFromLine(line);
            }
            else if (line.Contains("Prize"))
            {
                p = GetNumbersFromLine(line);
            }
            else
            {
                machines.Add(new Machine(a[0], a[1], b[0], b[1], p[0], p[1], isPart2));
            }
        }

        var costs = machines.Select(x => Cost(x)).ToArray();

        foreach(var c in costs)
        {
            Console.WriteLine(c);
        }

        var result = costs.Sum();
        Console.WriteLine("RESULT = " + result);
    }

    public decimal Cost(Machine mac)
    {
        var u = SolveLinearEquations(mac);
        if (u[0] % 1 == 0)
        {
            return u[0] * 3 + u[1];
        }
        else
        {
            return 0;
        }
    }

    public decimal[] SolveLinearEquations(Machine mac)
    {
        var u = new decimal[2];
        var det = Determinant(mac);

        u[0] = (mac.PrizeX * mac.BY - mac.PrizeY * mac.BX) / (decimal)det;
        u[1] = (mac.PrizeY * mac.AX - mac.PrizeX * mac.AY) / (decimal)det;

        return u;
    }

    public int Determinant(Machine mac)
    {
        return mac.AX * mac.BY - mac.AY * mac.BX;
    }

    public int[] GetNumbersFromLine(string line)
    {
        return line.Split(_separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToArray();
    }

    public string[] GetSeparators()
    {
        return new string[]
        {
            "Button A: X+",
            "Button B: X+",
            ", Y+",
            "Prize: X=",
            ", Y="
        };
    }
}

public class Machine
{
    public Machine(int ax, int ay, int bx, int by, long prizex, long prizey, bool isPart2 = false)
    {
        AX = ax;
        AY = ay;
        BX = bx;
        BY = by;
        PrizeX = prizex;
        PrizeY = prizey;

        if(isPart2)
        {
            PrizeX += 10000000000000;
            PrizeY += 10000000000000;
        }
    }

    public int AX { get; set; }
    public int AY { get; set; }
    public int BX { get; set; }
    public int BY { get; set; }
    public long PrizeX { get; set; }
    public long PrizeY { get; set; }

}