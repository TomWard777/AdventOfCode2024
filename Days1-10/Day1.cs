namespace AdventOfCode2023;

public class Day1
{
    public void Run()
    {
        var input = FileParser.ReadInputFromFile("Day1.txt");
        ////var input = FileParser.ReadInputFromFile("Day1.txt");
        var directions = input.First().ToCharArray();

        var steps = 0;
        var total = 0;

        foreach (var d in directions)
        {
            Console.Write(d);

            if (d == '(')
            {
                total++;
            }
            else
            {
                total--;
            }

            steps++;
            Console.WriteLine($"  {steps}");

            if(total < 0)
            {
                break;
            }
        }

        Console.WriteLine("\nEnding");
    }
}