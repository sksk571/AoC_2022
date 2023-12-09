var data = File.ReadAllLines(@"c:\projects\AoC\2022\11\input.txt");

var r1 = Run(data, 20, 3);
r1.Dump();

var r2 = Run(data, 10000, 1);
r2.Dump();

static long Run(IEnumerable<string> lines, int rounds, int divisor)
{
    var monkeys = BuildList(lines);
    
    for (int i = 0; i < rounds; ++i)
    {
        foreach (var monkey in monkeys)
        {
            monkey.Inspect(divisor);
        }
    }
    return monkeys.Select(m => m.Business).OrderDescending().Take(2).Aggregate((m1, m2) => m1 * m2);;
}

static List<Monkey> BuildList(IEnumerable<string> lines)
{
    var monkeys = new List<Monkey>();
    foreach (var chunk in lines.Chunk(7))
    {
        //chunk.Dump();
        
        var monkey = new Monkey(startingItems: chunk[1].Substring(17).Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => long.Parse(i))
            .ToArray(),
            operation: chunk[2].Substring(23)[0],
            opArg: chunk[2].Substring(25),
            divisor: int.Parse(chunk[3].Substring(21)),
            ifTrue: int.Parse(chunk[4].Substring(29)),
            ifFalse: int.Parse(chunk[5].Substring(30)),
            monkeys: monkeys);
        monkeys.Add(monkey);
    }
    return monkeys;

}

class Monkey
{
    private readonly List<long> _items;
    private readonly char _operation;
    private readonly string _opArg;
    private readonly int _divisor;
    private readonly int _ifTrue, _ifFalse;
    private readonly List<Monkey> _monkeys;
    private int _lcd = 0;

    public Monkey(IEnumerable<long> startingItems, char operation, string opArg, int divisor, int ifTrue, int ifFalse, List<Monkey> monkeys)
    {
        _items = new List<long>(startingItems);
        _operation = operation;
        _opArg = opArg;
        _divisor = divisor;
        _ifTrue = ifTrue;
        _ifFalse = ifFalse;
        _monkeys = monkeys;
    }
    
    public long Business { get; private set; }
    
    public void Inspect(int divisor = 1)
    {
        if (_lcd == 0)
            _lcd = _monkeys.Select(m => m._divisor).Aggregate((m1, m2) => m1 * m2);
            
        foreach (var i in _items)
        {
            if (!long.TryParse(_opArg, out var arg))
            {
                arg = i;
            }
            var worry = _operation switch
            {
                '*' => i * arg,
                '+' => i + arg,
                _ => throw new NotSupportedException()
            };
            worry = (worry / divisor) % _lcd;
            int monkeyIndex = worry % _divisor == 0 ? _ifTrue : _ifFalse;
            _monkeys[monkeyIndex]._items.Add(worry);
            Business++;
        }
        _items.Clear();
    }

    public override string ToString()
    {
        return $"Items: {string.Join(",", _items)}, Operation: {_operation}, OpArg: {_opArg}, Divisor: {_divisor}, True: {_ifTrue}, False: {_ifFalse}";
    }
}