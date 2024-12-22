using Microsoft.VisualBasic;

namespace AdventOfCode2024;

public static class Matrices
{
    public static Matrix ReadToMatrix(IEnumerable<string> input)
    {
        var m = input.Count();
        var n = input.First().Length;

        var mat = input
        .Select(x => x.ToCharArray())
        .ToArray();

        return new Matrix(m, n, mat);
    }

    public static (int, int, char[][]) ReadToMatrixTuple(IEnumerable<string> input)
    {
        var m = input.Count();
        var n = input.First().Length;

        var mat = input
        .Select(x => x.ToCharArray())
        .ToArray();

        return (m, n, mat);
    }

    public static IEnumerable<(int, int)> GetDirectlyAdjacentPlaces(int m, int n, int i, int j)
    {
        var places = new List<(int, int)>
        {
            (i - 1, j),
            (i + 1, j),
            (i, j + 1),
            (i, j - 1)
        };

        return places
        .Where(p => p.Item1 >= 0 && p.Item1 < m && p.Item2 >= 0 && p.Item2 < n)
        .ToArray();
    }

    public static IEnumerable<(int, int)> GetAdjacentPlaces(int m, int n, int i, int j)
    {
        var places = new List<(int, int)>
        {
            (i - 1, j),
            (i + 1, j),
            (i - 1, j + 1),
            (i + 1, j + 1),
            (i - 1, j - 1),
            (i + 1, j - 1),
            (i, j + 1),
            (i, j - 1)
        };

        return places
        .Where(p => p.Item1 >= 0 && p.Item1 < m && p.Item2 >= 0 && p.Item2 < n)
        .ToArray();
    }

    public static char[] GetRow(int rowNumber, int n, char[][] mat)
    {
        var list = new List<char>();

        for (int j = 0; j < n; j++)
        {
            list.Add(mat[rowNumber][j]);
        }

        return list.ToArray();
    }

    public static char[] GetColumn(int colNumber, int m, char[][] mat)
    {
        var list = new List<char>();

        for (int i = 0; i < m; i++)
        {
            list.Add(mat[i][colNumber]);
        }

        return list.ToArray();
    }

    public static bool IsPointOutsideMatrix((int, int) ij, int m, int n)
    {
        return IsPointOutsideMatrix(ij.Item1, ij.Item2, m, n);
    }

    public static bool IsPointOutsideMatrix(int i, int j, int m, int n)
    {
        return i < 0 || j < 0 || i >= m || j >= n;
    }

    public static void Draw(Matrix mat)
    {
        for (int i = 0; i < mat.RowCount; i++)
        {
            for (int j = 0; j < mat.ColCount; j++)
            {
                Console.Write(mat.Entries[i][j]);
            }
            Console.Write("\n");
        }
    }

    public static void Draw(int m, int n, char[][] mat)
    {
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(mat[i][j]);
            }
            Console.Write("\n");
        }
        Console.WriteLine();
    }

    public static (int, int) GetUniqueCharacterPosition(char[][] mat, char ch)
    {
        var i = 0;

        foreach (var row in mat)
        {
            if (row.Contains(ch))
            {
                break;
            }

            i++;
        }

        for (int j = 0; ; j++)
        {
            if (mat[i][j] == ch)
            {
                return (i, j);
            }
        }
    }

    public static void DrawSubset(int m, int n, char[][] mat, IEnumerable<(int, int)> set, char fillerChar = ' ')
    {
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (set.Contains((i, j)))
                {
                    Console.Write(mat[i][j]);
                }
                else
                {
                    Console.Write(fillerChar);
                }
            }
            Console.Write("\n");
        }
    }

    public static (int, int) GetPointInFront((int, int) pair, Facing facing)
    {
        return GetPointInFront(pair.Item1, pair.Item2, facing);
    }

    public static (int, int) GetPointInFront(int i, int j, Facing facing)
    {
        return facing switch
        {
            Facing.Up => (i - 1, j),
            Facing.Down => (i + 1, j),
            Facing.Left => (i, j - 1),
            Facing.Right => (i, j + 1),
            _ => throw new NotSupportedException()
        };
    }
}
