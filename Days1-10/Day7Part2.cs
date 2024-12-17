
namespace AdventOfCode2024;

public class Day7Part2
{
    public void Run()
    {
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
        for (int n = 0; n < Maths.IntPower(3, length); n++)
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
        if (n >= Maths.IntPower(3, length))
        {
            throw new Exception("Get operation array: number too large");
        }

        var binaryString = Maths.GetBaseBString(n, 3).PadLeft(length, '0');
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
            else if (ch == '2')
            {
                ops.Add(Operation.Concat);
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
            case Operation.Concat:
                return long.Parse(a.ToString() + b.ToString());
        }

        throw new Exception("Operation not recognised");
    }
}