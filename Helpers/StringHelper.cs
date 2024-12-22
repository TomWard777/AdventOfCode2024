namespace AdventOfCode2024;

public static class StringHelper
{
    public static IEnumerable<string> ReadMultilineString(string input)
    {
        using (StringReader reader = new StringReader(input))
        {
            string line = string.Empty;
            do
            {
                line = reader.ReadLine();

                if (line != null)
                {
                    yield return line;
                }

            } while (line != null);
        }
    }

    public static string[] CartesianPower(IEnumerable<string> arr, int n)
    {
        var result = new string[] { string.Empty };

        for (var i = 0; i < n; i++)
        {
            result = CartesianProduct(result, arr).ToArray();
        }

        return result;
    }

    public static IEnumerable<string> CartesianProduct(
        IEnumerable<string> arr1,
        IEnumerable<string> arr2)
    {
        foreach (var a in arr1)
        {
            foreach (var b in arr2)
            {
                yield return a + b;
            }
        }
    }
}