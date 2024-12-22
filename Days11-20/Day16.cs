namespace AdventOfCode2024;

public class Day16
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;
    private readonly (int, int) _start;
    private readonly (int, int) _finish;
    private IEnumerable<string> _input;

    private readonly RandomHelper _random;

    public Day16()
    {
        //_input = FileParser.ReadInputFromFile("Test16.txt");
        _input = FileParser.ReadInputFromFile("Day16.txt");

        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(_input);

        _start = Matrices.GetUniqueCharacterPosition(_mat, 'S');
        _finish = Matrices.GetUniqueCharacterPosition(_mat, 'E');

        _random = new RandomHelper();
    }

    public void Run()
    {
        Matrices.Draw(_m, _n, _mat);

        Console.WriteLine();

        var numberOfTries = 100000000;

        var bestPath = new List<(int, int)>();
        var bestScore = 100000000;

        for (var n = 0; n < numberOfTries; n++)
        {
            var path = GetRandomPath();
            var score = ScorePath(path.ToArray());

            if (score > -1 && score < bestScore)
            {
                bestPath = path;
                bestScore = score;
            }

            if (n % 100000 == 0)
            {
                Console.WriteLine(numberOfTries - n + "  " + score + "  " + bestScore);
            }
        }

        Console.WriteLine("Number of tries = " + numberOfTries);
        Console.WriteLine("Best score = " + bestScore);
    }

    public int ScorePath((int, int)[] path)
    {
        if (!path.Contains(_finish))
        {
            // Path did not reach the finish. Score -1 so it can be discarded.
            return -1;
        }

        var stepsForward = path.Length + 1;
        var turns = GetNumberOfTurnsInPath(path);

        return stepsForward + turns * 1000;
    }

    public int GetNumberOfTurnsInPath((int, int)[] path)
    {
        var directions = new Facing[] { Facing.Up, Facing.Left, Facing.Down, Facing.Right };

        var facing = Facing.Right;
        var turns = 0;

        for (var n = 0; n < path.Length - 1; n++)
        {
            if (path[n + 1] != Matrices.GetPointInFront(path[n], facing))
            {
                turns++;

                foreach (var dir in directions)
                {
                    if (path[n + 1] == Matrices.GetPointInFront(path[n], dir))
                    {
                        facing = dir;
                    }
                }
            }
        }

        return turns;
    }

    public List<(int, int)> GetRandomPath()
    {
        var position = _start;
        var path = new List<(int, int)>() { position };

        do
        {
            position = GetNextPosition(position.Item1, position.Item2, path);

            if (position == _finish)
            {
                break;
            }
        }
        while (position != (-1, -1));

        return path;
    }

    public (int, int) GetNextPosition(int i, int j, List<(int, int)> path)
    {
        var possibleSteps = Matrices.GetAdjacentPlaces(_m, _n, i, j)
        .Where(v => _mat[v.Item1][v.Item2] != '#')
        .Where(v => !path.Contains(v))
        .ToArray();

        if (!possibleSteps.Any())
        {
            ///Console.WriteLine($"({position.Item1}, {position.Item2})");
            return (-1, -1);
        }

        var next = _random.ChooseRandomFromArray(possibleSteps);

        path.Add(next);
        return next;
    }
}