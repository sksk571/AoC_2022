var data = File.ReadAllLines(@"c:\projects\AoC\2022\10\input.txt");

// 20th, 60th, 100th, 140th, 180th, and 220th
var r1 = Execute(data).Where(r => r.clock == 20 || r.clock == 60 || r.clock == 100 || r.clock == 140 || r.clock == 180 || r.clock == 220)
    .Select(r => r.clock * r.x).Sum();
r1.Dump();

var r2 = string.Join("\n", Execute(data).Select(r =>
{
    var crt = (r.clock-1) % 40;
    int[] sprite = { r.x-1, r.x, r.x+1 };
    return sprite.Contains(crt) ? '#' : ' ';
        
}).Chunk(40).Select(chars => new string(chars)));

r2.Dump();

static IEnumerable<(int clock, int x)> Execute(IEnumerable<string> instructions)
{
    int c = 1, x = 1;
    foreach (var i in instructions)
    {
        var args = i.Split(' ');
        if (args[0] == "noop")
        {
            yield return (c, x);
        }
        else if (args[0] == "addx")
        {
            yield return (c, x);
            c++;
            yield return (c, x);
            x += int.Parse(args[1]);
            
        }
        
        c++;
    }
}