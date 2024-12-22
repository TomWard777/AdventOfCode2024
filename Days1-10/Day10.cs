namespace AdventOfCode2024;

public class Day10
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;
    private readonly RandomHelper _randomHelper;

    public Day10()
    {
        //var input = StringHelper.ReadMultilineString(GetTestInput());
        var input = FileParser.ReadInputFromFile("Day10.txt");

        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(input);
        _randomHelper = new RandomHelper();
    }


    public void Run()
    {
        Matrices.Draw(_m, _n, _mat);

        var starts = GetStartingPositions();
        var total = 0;

        foreach (var start in starts)
        {
            //var ends = GetPossibleEndPositions(start, 1000);
            var paths = GetPossiblePaths(start, 10000);
            Console.WriteLine($"Start = {start.Item1} {start.Item2}  Paths = {paths.Count}");
            total += paths.Count;
        }

        Console.WriteLine("RESULT = " + total);
    }

    public (int, int)[] GetStartingPositions()
    {
        var list = new List<(int, int)>();

        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                if (_mat[i][j] == '0')
                {
                    list.Add((i, j));
                }
            }
        }

        return list.ToArray();
    }

    public (int, int)[] GetPossibleEndPositions((int, int) start, int numberOfTries)
    {
        var list = new List<(int, int)>();

        for (var n = 0; n < numberOfTries; n++)
        {
            var path = GetRandomPath(start);

            if (path.Any())
            {
                list.Add(path.First());
            }
        }

        return list.Distinct().ToArray();
    }

    public List<(int, int)[]> GetPossiblePaths((int, int) start, int numberOfTries)
    {
        var list = new List<(int, int)[]>();

        for (var n = 0; n < numberOfTries; n++)
        {
            var path = GetRandomPath(start);

            var add = true;

            foreach (var p in list)
            {
                if (Enumerable.SequenceEqual(path, p))
                {
                    add = false;
                    break;
                }
            }

            if (add && path.Any())
            {
                list.Add(path);
            }
        }

        return list;
    }

    public (int, int)[] GetRandomPath((int, int) start)
    {
        var pos = start;
        var path = new Stack<(int, int)>();
        var visited = new List<(int, int)> { pos };
        path.Push(pos);

        do
        {

            var val = _mat[pos.Item1][pos.Item2];

            var possibleNext = Matrices.GetDirectlyAdjacentPlaces(_m, _n, pos.Item1, pos.Item2)
            .Where(p => !visited.Contains(p))
            .Where(p => _mat[p.Item1][p.Item2] == val + 1)
            .ToArray();

            if (possibleNext.Length == 0)
            {
                path.Pop();

                if (!path.Any())
                {
                    return new (int, int)[] { };
                }

                pos = path.Peek();
            }
            else
            {
                pos = _randomHelper.ChooseRandomFromArray(possibleNext);
                path.Push(pos);
                visited.Add(pos);
            }
        }
        while (_mat[pos.Item1][pos.Item2] != '9');

        path.Push(pos);
        return path.ToArray();
    }

    public string GetTestInput()
    {
        return @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";
    }
}
