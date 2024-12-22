namespace AdventOfCode2024;

public static class ArrayHelper
{
    public static T[][] CartesianProductArray<T>(
        IEnumerable<T> arr1,
        IEnumerable<T> arr2)
    {
        return CartesianProduct(arr1, arr2).ToArray();
    }

    public static T[][] CartesianProductArray<T>(
        IEnumerable<T> arr1,
        IEnumerable<T> arr2,
        IEnumerable<T> arr3,
        IEnumerable<T> arr4)
    {
        return CartesianProduct(arr1, arr2, arr3, arr4).ToArray();
    }

    public static IEnumerable<T[]> CartesianProduct<T>(
        IEnumerable<T> arr1,
        IEnumerable<T> arr2)
    {
        foreach (var a in arr1)
        {
            foreach (var b in arr2)
            {
                yield return new T[] { a, b };
            }
        }
    }

    public static IEnumerable<T[]> CartesianProduct<T>(
        IEnumerable<T> arr1,
        IEnumerable<T> arr2,
        IEnumerable<T> arr3,
        IEnumerable<T> arr4)
    {
        foreach (var a in arr1)
        {
            foreach (var b in arr2)
            {
                foreach (var c in arr3)
                {
                    foreach (var d in arr4)
                    {
                        yield return new T[] { a, b, c, d };
                    }
                }
            }
        }
    }
}