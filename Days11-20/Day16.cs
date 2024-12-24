namespace AdventOfCode2024;

public class Day16
{
    // 166576 - WRONG
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

        Matrices.DrawSubset(_m, _n, _mat, GetRandomPath());

        var numberOfTries = 100000;

       // var bestPath = new (int, int)[];
        var bestScore = 100000000;

        for (var n = 0; n < numberOfTries; n++)
        {
            var path = GetRandomPath();
            var score = ScorePath(path.ToArray());

            if (score > -1 && score < bestScore)
            {
                //bestPath = path;
                bestScore = score;
            }

            if (n % 100 == 0)
            {
                Console.WriteLine(numberOfTries - n + "  " + score + " Best score = " + bestScore);
            }
        }

        Console.WriteLine("\nNumber of tries = " + numberOfTries);
        Console.WriteLine("Best score = " + bestScore);
    }

    public int ScorePath((int, int)[] path)
    {
        if (path.Length == 0)
        {
            // Path did not reach the finish. Score -1 so it can be discarded.
            return -1;
        }

        var stepsForward = path.Length - 1;
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

    public (int, int)[] GetRandomPath()
    {
        var pos = _start;
        var path = new Stack<(int, int)>();
        var visited = new List<(int, int)> { pos };

        path.Push(pos);
        var success = true;

        do
        {
            pos = Step(pos.Item1, pos.Item2, path, visited);
        }
        while (_mat[pos.Item1][pos.Item2] != 'E' && success);

        return path.ToArray();
    }

    public (int, int) Step(
        int i,
        int j,
        Stack<(int, int)> path,
        List<(int, int)> visited)
    {
        var possibleSteps = Matrices.GetDirectlyAdjacentPlaces(_m, _n, i, j)
        .Where(v => _mat[v.Item1][v.Item2] != '#')
        .Where(v => !visited.Contains(v))
        .ToArray();

        if (!possibleSteps.Any())
        {
            // If there are no possible steps, step back.
            path.Pop();

            if (!path.Any())
            {
                return (-1, -1);
            }

            return path.Peek();
        }
        else
        {
            var next = _random.ChooseRandomFromArray(possibleSteps);

            path.Push(next);
            visited.Add(next);

            return next;
        }
    }
}