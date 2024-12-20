namespace AdventOfCode2024;

public class Day17
{
    ////306300000
    public void Run()
    {
        var prog = new int[] { 2, 4, 1, 1, 7, 5, 0, 3, 1, 4, 4, 0, 5, 5, 3, 0 };
        var testProg = new int[] { 0, 3, 5, 4, 3, 0 };

        long n = 88800000;

        while (n < 1000000000000000000)
        {
            var comp = new Computer(n, prog);
            comp.RunProgram();

            if (comp.IsOutputEqualToProgram())
            {
                break;
            }

            if (n % 100000 == 0)
            {
                Console.Write(n + ", ");
            }

            n++;
        }

        Console.WriteLine("\n RESULT = " + n);
    }
}

public class Computer
{
    public Computer(long a, int[] program)
    {
        A = a;
        B = 0;
        C = 0;
        Program = program;
        Pointer = 0;
        Output = new List<long>();
    }

    public long A { get; set; }
    public long B { get; set; }
    public long C { get; set; }
    public int[] Program { get; set; }
    public int Pointer { get; set; }
    public List<long> Output { get; set; }

    public void RunProgram()
    {
        do
        {
            if (Pointer > Program.Length - 1)
            {
                // Attempting to read past end of program - halt.
                break;
            }

            var opcode = Program[Pointer];
            var operand = Program[Pointer + 1];

            switch (opcode)
            {
                case 0:
                    Adv(operand);
                    Pointer += 2;
                    break;
                case 1:
                    Bxl(operand);
                    Pointer += 2;
                    break;
                case 2:
                    Bst(operand);
                    Pointer += 2;
                    break;
                case 3:
                    var jumped = Jnz(operand);
                    if (!jumped)
                    {
                        Pointer += 2;
                    }
                    break;
                case 4:
                    Bxc(operand);
                    Pointer += 2;
                    break;
                case 5:
                    Out(operand);
                    Pointer += 2;
                    break;
                case 6:
                    Bdv(operand);
                    Pointer += 2;
                    break;
                case 7:
                    Cdv(operand);
                    Pointer += 2;
                    break;
                default:
                    throw new Exception("Invalid opcode: " + opcode);
            }
        }
        while (true);
    }

    public bool IsOutputEqualToProgram()
    {
        if (Output.Count() != Program.Length)
        {
            return false;
        }

        var output = Output.ToArray();

        for (var i = 0; i < output.Length; i++)
        {
            if (output[i] != Program[i])
            {
                return false;
            }
        }

        return true;
    }

    public string PrintOutput()
    {
        return string.Join(',', Output);
    }

    public long GetComboOperandValue(int operand)
    {
        if (operand > -1 && operand < 4)
        {
            return operand;
        }

        switch (operand)
        {
            case 4:
                return A;
            case 5:
                return B;
            case 6:
                return C;
            default:
                throw new Exception("Invalid operand: " + operand);
        }
    }

    public void Adv(int operand)
    {
        var n = GetComboOperandValue(operand);
        var denom = Maths.LongPower(2, n);
        A = A / denom;
    }

    public void Bdv(int operand)
    {
        var n = GetComboOperandValue(operand);
        var denom = Maths.LongPower(2, n);
        B = A / denom;
    }

    public void Cdv(int operand)
    {
        var n = GetComboOperandValue(operand);
        var denom = Maths.LongPower(2, n);
        C = A / denom;
    }

    public void Bxl(int operand)
    {
        B = B ^ operand;
    }

    public void Bst(int operand)
    {
        var n = GetComboOperandValue(operand);
        B = n % 8;
    }

    public bool Jnz(int operand)
    {
        if (A == 0)
        {
            return false;
        }
        else
        {
            Pointer = operand;
            return true;
        }
    }

    public void Bxc(int operand)
    {
        B = B ^ C;
    }

    public void Out(int operand)
    {
        var n = GetComboOperandValue(operand);
        Output.Add(n % 8);
    }
}
