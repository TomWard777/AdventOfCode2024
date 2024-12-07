namespace AdventOfCode2023;

public class Day6
{
        private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;

    public Day6()
    {
        var input = FileParser.ReadInputFromFile("Test4.txt");
        //var input = FileParser.ReadInputFromFile("Day4.txt");
        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(input);
    }

    public void Run()
    {
        Console.WriteLine("RESULT = ");
    }
}