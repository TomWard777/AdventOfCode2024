using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Day4
{
    private readonly int _m;
    private readonly int _n;
    private readonly char[][] _mat;
    private IEnumerable<string> _input;
    public Day4()
    {
        _input = FileParser.ReadInputFromFile("Day4.txt");
        //_input = FileParser.ReadInputFromFile("Test4.txt");
        (_m, _n, _mat) = Matrices.ReadToMatrixTuple(_input);
    }

    public void Run()
    {
        var r = CountRows();
        var c = CountCols();
        var d = CountDiagonals();
        var total = r + c + d;

        Console.WriteLine("Rows " + r);
        Console.WriteLine("Cols " + c);
        Console.WriteLine("Diagonals " + d);
        Console.WriteLine("TOTAL " + total);
        Console.WriteLine("Xmas squares " + CountXmasSquares());
    }

    public int CountXmasSquares()
    {
        var ct = 0;

        for (int i = 1; i < _m - 1; i++)
        {
            for (int j = 1; j < _n - 1; j++)
            {
                if(IsCentreOfXmasSquare(i,j))
                {
                    ct++;
                }
            }
        }

        return ct;
    }

    public bool IsCentreOfXmasSquare(int i, int j)
    {
        if (_mat[i][j] != 'A')
        {
            return false;
        }

        char[] arr =
        {
            _mat[i-1][j-1],
            _mat[i + 1][j + 1],
            _mat[i -1][j + 1],
            _mat[i + 1][j -1]
        };

        var str = new string(arr.Order().ToArray());

        if(str == "MMSS")
        {
            // If the ends of a diagonal are equal, it can't be an Xmas square.
            return _mat[i-1][j-1] != _mat[i + 1][j + 1];
        }

        return false;
    }

    public int CountRows()
    {
        var ct = 0;

        for (int i = 0; i < _m; i++)
        {
            var str = new string(Matrices.GetRow(i, _n, _mat));
            ct += Regex.Matches(str, @"XMAS").Count;
            ct += Regex.Matches(str, @"SAMX").Count;
        }

        return ct;
    }

    public int CountCols()
    {
        var ct = 0;

        for (int j = 0; j < _n; j++)
        {
            var str = new string(Matrices.GetColumn(j, _m, _mat));
            ct += Regex.Matches(str, @"XMAS").Count;
            ct += Regex.Matches(str, @"SAMX").Count;
        }

        return ct;
    }

    public int CountDiagonals()
    {
        var ct = 0;
        for (int i = 0; i < _m - 3; i++)
        {
            for (int j = 0; j < _n - 3; j++)
            {
                char[] letters = { _mat[i][j], _mat[i + 1][j + 1], _mat[i + 2][j + 2], _mat[i + 3][j + 3] };
                var str = new string(letters);

                if (str == "XMAS" || str == "SAMX")
                {
                    ct++;
                }
            }
        }

        for (int i = 0; i < _m - 3; i++)
        {
            for (int j = 3; j < _n; j++)
            {

                char[] letters = { _mat[i][j], _mat[i + 1][j - 1], _mat[i + 2][j - 2], _mat[i + 3][j - 3] };
                var str = new string(letters);

                if (str == "XMAS" || str == "SAMX")
                {
                    ct++;
                }
            }
        }

        return ct;
    }
}