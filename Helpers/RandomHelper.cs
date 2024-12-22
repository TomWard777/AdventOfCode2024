namespace AdventOfCode2024;

public class RandomHelper
{
    private readonly Random _random;

    public RandomHelper()
    {
        _random = new Random();
    }

    public T ChooseRandomFromArray<T>(T[] arr)
    {
        if(arr.Length == 1)
        {
            return arr[0];
        }

        int index = _random.Next(0, arr.Length);
        return arr[index];  
    }
}