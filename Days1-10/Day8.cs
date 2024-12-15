namespace AdventOfCode2023;

public class Day8
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;

    public Day8()
    {
        //var input = FileParser.ReadInputFromFile("Test8.txt");
        var input = FileParser.ReadInputFromFile("Day8.txt");
        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(input);
    }

    public void Run()
    {
        Matrices.Draw(_m, _n, _mat);
        Console.WriteLine();

        var antennae = GetAntennae();
        var antennaSymbols = antennae.Select(x => x.Symbol).Distinct();

        var antennaGroups = antennae
        .GroupBy(x => x.Symbol)
        .Select(g => g.Select(a => (a.Row, a.Col)).ToArray())
        .ToArray();

        var antinodes = new List<(int, int)>();

        foreach (var group in antennaGroups)
        {
            var arr = GetAntinodes(group);
            antinodes.AddRange(arr);
        }

        foreach (var node in antinodes)
        {
            _mat[node.Item1][node.Item2] = '#';
        }

        Matrices.Draw(_m, _n, _mat);
        Console.WriteLine();
        Console.WriteLine(antinodes.Distinct().Count());
    }

    public (int, int)[] GetAntinodes(IEnumerable<(int, int)> antennaLocations)
    {
        var antinodes = new List<(int, int)>();
        var pairs = GetLocationPairs(antennaLocations);

        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                foreach (var p in pairs)
                {
                    if (PointsAreCollinear(i, j, p.Point1.Item1, p.Point1.Item2, p.Point2.Item1, p.Point2.Item2))
                    {
                        antinodes.Add((i, j));
                    }

                    // var d1 = DistanceSquared(i, j, p.Point1.Item1, p.Point1.Item2);
                    // var d2 = DistanceSquared(i, j, p.Point2.Item1, p.Point2.Item2);

                    // if (d1 == 4 * d2 || d2 == 4 * d1)
                    // {
                    //     if (PointsAreCollinear(i, j, p.Point1.Item1, p.Point1.Item2, p.Point2.Item1, p.Point2.Item2))
                    //     {
                    //         antinodes.Add((i, j));
                    //     }
                    // }
                }
            }
        }

        return antinodes.ToArray();
    }

    public PointPair[] GetLocationPairs(IEnumerable<(int, int)> points)
    {
        var pairs = from p in points
                    from q in points
                    where p.Item1 > q.Item1 || p.Item2 > q.Item2
                    select new PointPair
                    {
                        Point1 = p,
                        Point2 = q
                    };

        return pairs.ToArray();
    }

    public bool PointsAreCollinear(int i1, int j1, int i2, int j2, int i3, int j3)
    {
        var test = (j2 - j1) * (i1 - i3) - (j1 - j3) * (i2 - i1);
        return test == 0;
    }

    public int DistanceSquared(int i1, int j1, int i2, int j2) => (i2 - i1) * (i2 - i1) + (j2 - j1) * (j2 - j1);

    public Antenna[] GetAntennae()
    {
        var list = new List<Antenna>();

        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                if (_mat[i][j] != '.')
                {
                    list.Add(new Antenna
                    {
                        Row = i,
                        Col = j,
                        Symbol = _mat[i][j]
                    });
                }
            }
        }

        return list.ToArray();
    }
}

public class PointPair
{
    public (int, int) Point1 { get; set; }
    public (int, int) Point2 { get; set; }
}

public class Antenna
{

    public int Row { get; set; }
    public int Col { get; set; }
    public char Symbol { get; set; }
}