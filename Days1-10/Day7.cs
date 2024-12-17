
using System.ComponentModel;

namespace AdventOfCode2024;

public class Day7
{
    public void Run()
    {
        for (int i = 0; i < 32; i++)
        {
            Console.WriteLine(Maths.GetBaseBString(i, 2));
        }

        var input = FileParser.ReadInputFromFile("Day7.txt");
        ////var input = new List<string> () {"190: 10 19"};

        long total = 0;

        foreach (var line in input)
        {
            var eq = new Equation(line);
            if (CanEquationBeSatisfied(eq))
            {
                total += eq.Result;
            }
        }

        Console.WriteLine("RESULT = " + total);
    }

    public bool CanEquationBeSatisfied(Equation eq)
    {
        //eq.Print();
        var length = eq.Parameters.Length - 1;
        for (int n = 0; n < Maths.IntPower(2, length); n++)
        {
            var ops = GetOperationArrayFromInteger(n, length);
            if (Check(eq, ops))
            {
                return true;
            }
        }

        return false;
    }

    public Operation[] GetOperationArrayFromInteger(int n, int length)
    {
        if (n >= Maths.IntPower(2, length))
        {
            throw new Exception("Get operation array: number too large");
        }

        var binaryString = Convert.ToString(n, 2).PadLeft(length, '0');

        var ops = new List<Operation>();

        foreach (var ch in binaryString)
        {
            if (ch == '0')
            {
                ops.Add(Operation.Add);
            }
            else if (ch == '1')
            {
                ops.Add(Operation.Mult);
            }
        }

        return ops.ToArray();
    }

    public bool Check(Equation eq, Operation[] ops)
    {
        return eq.Result == EvaluateParameters(eq.Parameters, ops);
    }

    public long EvaluateParameters(long[] parameters, Operation[] ops)
    {
        if (ops.Length != parameters.Length - 1)
        {
            throw new Exception(parameters.Length - 1 + " operations were expected, but " + ops.Length + " were given");
        }

        var result = parameters[0];

        for (long i = 0; i < ops.Length; i++)
        {
            result = Eval(ops[i], result, parameters[i + 1]);
        }

        return result;
    }

    public long Eval(Operation op, long a, long b)
    {
        switch (op)
        {
            case Operation.Add:
                return a + b;
            case Operation.Mult:
                return a * b;
        }

        throw new Exception("Operation not recognised");
    }
}

public class Equation
{
    public Equation(string input)
    {
        var arr = input.Split(": ");

        Result = long.Parse(arr[0]);

        Parameters = arr[1].Split(" ")
        .Select(x => long.Parse(x))
        .ToArray();
    }

    public long Result { get; set; }
    public long[] Parameters { get; set; }

    public void Print()
    {
        var str = string.Join(' ', Parameters.Select(x => x.ToString()));
        Console.WriteLine(str);
    }
}

public enum Operation
{
    Add = 0,
    Mult,
    Concat
}