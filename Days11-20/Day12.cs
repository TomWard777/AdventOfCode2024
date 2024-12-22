namespace AdventOfCode2024;

public class Day12
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;
    private IEnumerable<string> _input;

    public Day12()
    {
        _input = FileParser.ReadInputFromFile("Day12.txt");
        //_input = StringHelper.ReadMultilineString(GetTestString());

        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(_input);
    }

    public void Run()
    {
        Matrices.Draw(_m, _n, _mat);

        var pointsViewed = new List<(int, int)>();
        var total = 0;

        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                if (!pointsViewed.Contains((i, j)))
                {
                    var region = GetContainingRegion(i, j);
                    pointsViewed.AddRange(region);

                    var score = GetRegionScore2(region);
                    total += score;

                    Console.WriteLine(_mat[i][j] + " scores " + score);
                }
            }
        }

        Console.WriteLine("\nRESULT = " + total);
    }

    public int GetRegionScore2(List<(int, int)> region)
    {
        var numberOfSides = GetTotalCornerCount(region);
        var area = region.Count();
        return area * numberOfSides;
    }

    public int GetRegionScore(List<(int, int)> region)
    {
        var perim = GetPerimiter(region);
        var area = region.Count();
        return area * perim;
    }

    public int GetTotalCornerCount(List<(int, int)> region)
    {
        var total = 0;

        foreach (var p in region)
        {
            total += GetCornerCount(p, region);
        }

        return total;
    }

    public int GetCornerCount((int, int) point, List<(int, int)> region)
    {
        var i = point.Item1;
        var j = point.Item2;

        if (!region.Contains(point))
        {
            throw new Exception("Point outside region");
        }

        var count = 0;

        var left = Matrices.GetPointInFront(point, Facing.Left);
        var right = Matrices.GetPointInFront(point, Facing.Right);
        var up = Matrices.GetPointInFront(point, Facing.Up);
        var down = Matrices.GetPointInFront(point, Facing.Down);

        var upleft = (i - 1, j - 1);
        var upright = (i - 1, j + 1);
        var downleft = (i + 1, j - 1);
        var downright = (i + 1, j + 1);

        // "Convex" corners
        if (!region.Contains(left) && !region.Contains(up))
        {
            count++;
        }

        if (!region.Contains(up) && !region.Contains(right))
        {
            count++;
        }

        if (!region.Contains(right) && !region.Contains(down))
        {
            count++;
        }

        if (!region.Contains(down) && !region.Contains(left))
        {
            count++;
        }

        // "Concave" corners
        if (region.Contains(left) && region.Contains(up) && !region.Contains(upleft))
        {
            count++;
        }

        if (region.Contains(right) && region.Contains(up) && !region.Contains(upright))
        {
            count++;
        }

        if (region.Contains(left) && region.Contains(down) && !region.Contains(downleft))
        {
            count++;
        }

        if (region.Contains(right) && region.Contains(down) && !region.Contains(downright))
        {
            count++;
        }

        return count;
    }

    public List<(int, int)> GetContainingRegion(int i, int j)
    {
        var region = new List<(int, int)>() { (i, j) };

        var label = _mat[i][j];

        do
        {
            var oldSize = region.Count();
            var pointsToAdd = new List<(int, int)>();

            foreach (var p in region)
            {
                var newPoints = Matrices.GetDirectlyAdjacentPlaces(_m, _n, p.Item1, p.Item2)
                .Where(p => !region.Contains(p) && _mat[p.Item1][p.Item2] == label)
                .ToArray();

                pointsToAdd.AddRange(newPoints);
            }

            region.AddRange(pointsToAdd.Distinct());

            if (oldSize == region.Count())
            {
                break;
            }
        }
        while (true);

        return region;
    }

    public int GetPerimiter(List<(int, int)> region)
    {
        var internalEdgeDoubleCount = 0;

        foreach (var point in region)
        {
            // Console.Write($"({point.Item1} {point.Item2})  ");

            var n = Matrices.GetDirectlyAdjacentPlaces(_m, _n, point.Item1, point.Item2)
            .Where(p => region.Contains(p))
            .Count();

            internalEdgeDoubleCount += n;
        }

        // The above counts each internal edge twice, but so does counting 4 times
        // the area to get the perimiter - so these overcounts cancel.
        var area = region.Count();

        ///Console.WriteLine($"\nPerim calc: {area} squares, {internalEdgeDoubleCount/2} internal edges, result {4 * area - internalEdgeDoubleCount}");

        return 4 * area - internalEdgeDoubleCount;
    }

    public string GetTestString()
    {
        return @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE";
    }
}