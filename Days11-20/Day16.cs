namespace AdventOfCode2024;

public class Day16
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;
    private IEnumerable<string> _input;

    public Day16()
    {
        _input = FileParser.ReadInputFromFile("Test16.txt");
        //_input = FileParser.ReadInputFromFile("Day16.txt");
        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(_input);
    }

    public void Run()
    {
        Matrices.Draw(_m, _n, _mat);

        Console.WriteLine();
    }
}