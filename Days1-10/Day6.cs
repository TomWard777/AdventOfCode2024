namespace AdventOfCode2023;

public class Day6
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;

    public Day6()
    {
        //var input = FileParser.ReadInputFromFile("Test6.txt");
        var input = FileParser.ReadInputFromFile("Day6.txt");
        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(input);
    }

    public void Run()
    {
        var guard = new Guard();
        guard.Postion = GetStartEntry();

        Console.WriteLine();
        var visited = new List<(int, int)>();

        Walk(guard, visited);
        Matrices.Draw(_m, _n, _mat);
        Console.WriteLine(visited.Distinct().Count());
    }

    public void Walk(Guard guard, List<(int, int)> visited)
    {
        do
        {
            visited.Add(guard.Postion);
            _mat[guard.Postion.Item1][guard.Postion.Item2] = 'X';

            guard.Postion = GetNextStep(guard);
        }
        while(!Matrices.IsPointOutsideMatrix(guard.Postion, _m, _n));
    }

    public (int, int) GetNextStep(Guard guard)
    {
        var (i, j) = guard.Postion;
        var (i2, j2) = GetPointInFront(i, j, guard.Direction);

        if (Matrices.IsPointOutsideMatrix(i2, j2, _m, _n))
        {
            return (i2, j2);
        }

        while (_mat[i2][j2] == '#')
        {
            guard.Direction = TurnRight(guard.Direction);
            (i2, j2) = GetPointInFront(i, j, guard.Direction);

            if (Matrices.IsPointOutsideMatrix(i2, j2, _m, _n))
            {
                return (i2, j2);
            }
        }

        return (i2, j2);
    }

    public (int, int) GetStartEntry()
    {
        var i = 0;

        foreach (var row in _mat)
        {
            if (row.Contains('^'))
            {
                break;
            }

            i++;
        }

        for (int j = 0; ; j++)
        {
            if (_mat[i][j] == '^')
            {
                return (i, j);
            }
        }
    }

    public (int, int) GetPointInFront(int i, int j, Facing facing)
    {
        return facing switch
        {
            Facing.Up => (i - 1, j),
            Facing.Down => (i + 1, j),
            Facing.Left => (i, j - 1),
            Facing.Right => (i, j + 1),
            _ => throw new NotSupportedException()
        };
    }

    public Facing TurnRight(Facing facing)
    {
        return facing switch
        {
            Facing.Up => Facing.Right,
            Facing.Down => Facing.Left,
            Facing.Left => Facing.Up,
            Facing.Right => Facing.Down,
            _ => throw new NotSupportedException()
        };
    }
}

public class Guard
{
    public (int, int) Postion { get; set; }
    public Facing Direction { get; set; }
}

public enum Facing
{
    Up,
    Down,
    Left,
    Right
}
