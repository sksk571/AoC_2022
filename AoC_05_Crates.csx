var data = File.ReadAllLines(@"c:\projects\AoC\2022\05\input.txt");

var crates = new List<List<char>>();
var linesRead = 0;
foreach (var line in data)
{
    linesRead++;
    if (string.IsNullOrEmpty(line))
        break;
    int listIndex = 0;
    foreach (var g in line.Chunk(4))
    {
        if (crates.Count <= listIndex)
            crates.Add(new List<char>());
        var c = g.ElementAt(1);
        if (char.IsLetter(c))
            crates[listIndex].Add(c);
        listIndex++;
    }
}
crates.ForEach(list => list.Reverse());
var stacks = crates.Select(list => new Stack<char>(list)).ToList();
Unload(stacks, data.Skip(linesRead), false);

var r1 = string.Join("", stacks.Select(s => s.Peek()));
r1.Dump();

stacks = crates.Select(list => new Stack<char>(list)).ToList();
Unload(stacks, data.Skip(linesRead), true);

var r2 = string.Join("", stacks.Select(s => s.Peek()));
r2.Dump();

void Unload(List<Stack<char>> crates, IEnumerable<string> instructions, bool inverse)
{
    foreach (var line in instructions)
    {
        var instruction = Regex.Match(line, @"move (?<count>\d+) from (?<from>\d+) to (?<to>\d+)");
        if (instruction.Success)
        {
            int count = int.Parse(instruction.Groups["count"].Value);
            int from = int.Parse(instruction.Groups["from"].Value) - 1;
            int to = int.Parse(instruction.Groups["to"].Value) - 1;
            if (inverse)
            {
                var buffer = new List<char>();
                for (int i = 0; i < count; ++i)
                {
                    buffer.Add(stacks[from].Pop());
                }
                buffer.Reverse();
                for (int i = 0; i < count; ++i)
                {
                    stacks[to].Push(buffer[i]);
                }
            }
            else
            {
                for (int i = 0; i < count; ++i)
                {
                    stacks[to].Push(stacks[from].Pop());
                }
            }
        }
    }
}