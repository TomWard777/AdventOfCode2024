namespace AdventOfCode2024;

public class Day6Part2
{
    private IEnumerable<string> _input;
    private int _m;
    private int _n;
    private char[][] _mat;

    public Day6Part2()
    {
        //_input = FileParser.ReadInputFromFile("Test6.txt");
        _input = FileParser.ReadInputFromFile("Day6.txt");
    }

    public new void Run()
    {
        var ct = 0;
        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(_input);

        Console.WriteLine("Rows: " + _m);

        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {

                (_m, _n, _mat) = Matrices.ReadToMatrixTuple(_input);

                if (_mat[i][j] == '.')
                {
                    _mat[i][j] = '#';

                    if (IsThereALoop(GetStartEntry()))
                    {
                        ct++;
                    }
                }
            }

            Console.WriteLine(i);
        }

        Console.WriteLine("RESULT " + ct);
    }

    public bool IsThereALoop((int, int) start)
    {
        var visited = new List<(int, int)>();
        var guard = new Guard();
        guard.Postion = start;

        do
        {
            visited.Add(guard.Postion);

            ////Console.WriteLine($"{guard.Postion.Item1}  {guard.Postion.Item2}");
            ////_mat[guard.Postion.Item1][guard.Postion.Item2] = 'X';

            var next = GetNextStep(guard);
            guard.Postion = next;

            if (VisitedPointsAreLoop(visited))
            {
                //Draw();
                return true;
            }
        }
        while (!Matrices.IsPointOutsideMatrix(guard.Postion, _m, _n));

        return false;
    }

    public bool VisitedPointsAreLoop(List<(int, int)> visited)
    {
        var duplicatesCount = visited.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .Count();

        return duplicatesCount > 18;
    }
    public (int, int) GetNextStep(Guard guard)
    {
        var (i, j) = guard.Postion;
        var (i2, j2) = Matrices.GetPointInFront(i, j, guard.Direction);

        if (Matrices.IsPointOutsideMatrix(i2, j2, _m, _n))
        {
            return (i2, j2);
        }

        while (_mat[i2][j2] == '#')
        {
            guard.Direction = TurnRight(guard.Direction);
            (i2, j2) = Matrices.GetPointInFront(i, j, guard.Direction);

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

    public void Draw() => Matrices.Draw(_m, _n, _mat);
}