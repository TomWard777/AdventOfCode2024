using System.Text;

namespace AdventOfCode2024;

public class Day24
{
    private readonly Dictionary<string, bool> _wireDictionary;
    private readonly List<LogicGate> _gateList;

    public Day24()
    {
        _wireDictionary = new Dictionary<string, bool>();
        _gateList = new List<LogicGate>();

        var input = FileParser.ReadInputFromFile("Day24.txt");
        //var input = FileParser.ReadInputFromFile("Test24.txt");

        foreach (var line in input)
        {
            if (line.Contains(":"))
            {
                ReadInputWire(line);
            }
            else if (line.Contains(">"))
            {
                ReadInputGate(line);
            }
        }
    }

    public void Run()
    {
        var totalZCount = _gateList
        .Select(g => g.Output)
        .Where(x => x.StartsWith('z'))
        .Count();

        Console.WriteLine("Total z-wires = " + totalZCount + "\n");

        EvaluateUntilAllWiresFound(totalZCount);

        var result = ReadResultNumber();
        Console.WriteLine("\nRESULT = " + result);
    }

    public void EvaluateUntilAllWiresFound(int totalZWires)
    {
        var zCount = 0;

        do
        {
            foreach (var gate in _gateList)
            {
                TryEvaluateGate(gate);
            }

            zCount = _wireDictionary.Keys
                .Where(k => k.StartsWith('z'))
                .Count();
        }
        while (zCount < totalZWires);
    }

    public long ReadResultNumber()
    {
        var zWires = _wireDictionary.Keys
            .Where(k => k.StartsWith('z'))
            .OrderBy(k => k)
            .ToArray();

        Console.WriteLine($"Z-wires found = {zWires.Length}");

        long result = 0;
        var sb = new StringBuilder();

        for (var n = 0; n < zWires.Length; n++)
        {
            if (_wireDictionary[zWires[n]])
            {
                result += Maths.LongPower(2, n);
                sb.Append("1");
            }
            else
            {
                sb.Append("0");
            }
        }

        Console.WriteLine(new string(sb.ToString().Reverse().ToArray()));

        return result;
    }

    public bool TryEvaluateGate(LogicGate gate)
    {
        bool val1;
        bool val2;

        // The return value indicates whether or not the evaluation was successful.
        if (_wireDictionary.TryGetValue(gate.Input1, out val1)
            && _wireDictionary.TryGetValue(gate.Input2, out val2))
        {
            /// TODO operation
            if (!_wireDictionary.ContainsKey(gate.Output))
            {
                _wireDictionary.Add(gate.Output, EvaluateLogicOperation(val1, val2, gate.Operation));
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EvaluateLogicOperation(bool val1, bool val2, LogicOperation op)
    {
        return op switch
        {
            LogicOperation.And => val1 && val2,
            LogicOperation.Or => val1 || val2,
            LogicOperation.Xor => val1 ^ val2,
            _ => throw new NotSupportedException()
        };
    }

    public void ReadInputWire(string line)
    {
        var arr = line.Split(": ", StringSplitOptions.RemoveEmptyEntries);
        var val = arr[1] == "1";

        _wireDictionary.Add(arr[0], val);
    }

    public void ReadInputGate(string line)
    {
        var opString = line.Substring(4, 3);

        var op = opString switch
        {
            "AND" => LogicOperation.And,
            "OR " => LogicOperation.Or,
            "XOR" => LogicOperation.Xor,
            _ => throw new NotSupportedException()
        };

        var separators = new string[] { opString, "->" };
        var arr = line.Split(separators, StringSplitOptions.TrimEntries);

        var gate = new LogicGate
        {
            Input1 = arr[0],
            Input2 = arr[1],
            Output = arr[2],
            Operation = op
        };

        _gateList.Add(gate);
    }
}

public class LogicGate
{
    public string Input1 { get; set; }
    public string Input2 { get; set; }
    public string Output { get; set; }
    public LogicOperation Operation { get; set; }
}

public enum LogicOperation
{
    And,
    Or,
    Xor
}