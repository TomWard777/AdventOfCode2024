namespace AdventOfCode2024;

public class Day15
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;
    private IEnumerable<string> _input;
    private string _directions;

    public Day15()
    {
        //_input = FileParser.ReadInputFromFile("TestMap15.txt");
        _input = FileParser.ReadInputFromFile("Map15.txt");
        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(_input);

        //var directionLines = FileParser.ReadInputFromFile("TestDirections15.txt");
        var directionLines = FileParser.ReadInputFromFile("Directions15.txt");
        _directions = string.Join(string.Empty, directionLines);
    }

    public void Run()
    {
        Matrices.Draw(_m, _n, _mat);

        Console.WriteLine();

        foreach (var dir in _directions)
        {
            Move(dir);
        }

        Matrices.Draw(_m, _n, _mat);
        Console.WriteLine("\n" + GetSumOfGPSCoordinates());
    }

    public void Move(char direction)
    {
        var (i, j) = GetRobotPosition();
        var entriesToMove = new List<(int, int)>();
        var currentObject = '@';

        while (currentObject != '#' && currentObject != '.')
        {
            entriesToMove.Add((i, j));
            (i, j) = GetPointInFront(i, j, direction);
            currentObject = _mat[i][j];
        }

        if (currentObject == '#')
        {
            // We hit a wall - we cannot move.
            return;
        }

        entriesToMove.Reverse();

        foreach (var point in entriesToMove)
        {
            var pointInFront = GetPointInFront(point, direction);
            _mat[pointInFront.Item1][pointInFront.Item2] = _mat[point.Item1][point.Item2];
            _mat[point.Item1][point.Item2] = '.';
        }
    }

    public int GetSumOfGPSCoordinates()
    {
        var total = 0;

        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                if (_mat[i][j] == 'O')
                {
                    total += 100 * i + j;
                }
            }
        }

        return total;
    }

    public (int, int) GetRobotPosition() => Matrices.GetUniqueCharacterPosition(_mat, '@');

    public (int, int) GetPointInFront((int, int) pair, char direction)
    {
        return GetPointInFront(pair.Item1, pair.Item2, direction);
    }

    public (int, int) GetPointInFront(int i, int j, char direction)
    {
        return direction switch
        {
            '^' => (i - 1, j),
            'v' => (i + 1, j),
            '<' => (i, j - 1),
            '>' => (i, j + 1),
            _ => throw new NotSupportedException()
        };
    }
}