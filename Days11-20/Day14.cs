using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace AdventOfCode2024;

public class Day14
{
    private readonly string[] _separators;
    private readonly int _xLimit;
    private readonly int _yLimit;

    public Day14()
    {
        _separators = GetSeparators();
        (_xLimit, _yLimit) = (11, 7);
        (_xLimit, _yLimit) = (101, 103);
    }

    public void Run()
    {
        var input = FileParser.ReadInputFromFile("Day14.txt");
        //var input = FileParser.ReadInputFromFile("Test14.txt");

        var robots = input
        .Select(str => str.Split(_separators, StringSplitOptions.RemoveEmptyEntries))
        .Select(a => new Robot(int.Parse(a[0]), int.Parse(a[1]), int.Parse(a[2]), int.Parse(a[3])))
        .ToArray();

        for (var i = 0; i < 1000000000; i++)
        {
            for (var j = 0; j < robots.Length; j++)
            {
                Move(robots[j]);
            }

            if (HorizontalGuess(robots.Select(r => r.Position).ToArray()))
            {
                DrawPositions(robots.Select(x => x.Position).ToArray());
                Console.WriteLine("//////////////////" + i.ToString() + "\n");
                Thread.Sleep(2000);
            }

            if(i%10000 == 0)
            Console.Write(i.ToString() + ", ");
        }

        var positions = robots.Select(r => r.Position).ToArray();

        var quadrantCounts = positions.Select(p => GetQuadrant(p.Item1, p.Item2))
        .GroupBy(x => x)
        .Select(g => new
        {
            Quadrant = g.Key,
            Count = g.Count()
        })
        .Where(x => x.Quadrant > 0)
        .ToArray();

        var prod = Maths.Product(quadrantCounts.Select(x => x.Count));
        Console.WriteLine("RESULT = " + prod);
    }

    public bool HorizontalGuess((int, int)[] positions)
    {
        var rowCounts = positions.GroupBy(p => p.Item2).Select(g => g.Count()).ToArray();
        return rowCounts.Any(ct => ct > _xLimit/2);
    }

    public void DrawPositions((int, int)[] positions)
    {
        Console.Clear();

        var positionCounts = positions.GroupBy(p => p)
        .Select(g => new { Position = g.Key, Count = g.Count() })
        .ToArray();

        for (var y = 0; y < _yLimit; y++)
        {
            for (var x = 0; x < _xLimit; x++)
            {
                var posCount = positionCounts.FirstOrDefault(pc => pc.Position.Equals((x, y)));
                var str = posCount != null ? posCount.Count.ToString() : ".";
                Console.Write(str);
                Console.Write(" ");
            }
            Console.Write("\n");
        }
    }

    public int GetQuadrant(int x, int y)
    {
        var xMiddle = (_xLimit - 1) / 2;
        var yMiddle = (_yLimit - 1) / 2;

        if (x == xMiddle || y == yMiddle)
        {
            return 0;
        }
        else if (x < xMiddle && y < yMiddle)
        {
            return 1;
        }
        else if (x > xMiddle && y < yMiddle)
        {
            return 2;
        }
        else if (x < xMiddle && y > yMiddle)
        {
            return 3;
        }
        else if (x > xMiddle && y > yMiddle)
        {
            return 4;
        }

        throw new Exception("Whoops");
    }

    public void Move(Robot robot)
    {
        var p = robot.Position;
        var v = robot.Velocity;
        robot.Position = (Mod(p.Item1 + v.Item1, _xLimit), Mod(p.Item2 + v.Item2, _yLimit));
    }

    public void PrintPosition((int, int) v)
    {
        Console.WriteLine($"({v.Item1}, {v.Item2})");
    }

    public int Mod(int a, int n)
    {
        var y = a % n;

        if (y < 0)
        {
            return y + n;
        }
        else
        {
            return y;
        }
    }

    public string[] GetSeparators()
    {
        return new string[]
        {
            "p=",
            " v=",
            ","
        };
    }
}

public class Robot
{
    public Robot(int x, int y, int vx, int vy)
    {
        Position = (x, y);
        Velocity = (vx, vy);
    }

    public (int, int) Position { get; set; }
    public (int, int) Velocity { get; set; }
}