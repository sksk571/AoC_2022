var data = File.ReadAllText(@"c:\projects\AoC\2022\01\input.txt");

var elfs = data.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Split("\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(s => int.Parse(s.Trim())).Sum());
var r1 = elfs.Max();
r1.Dump();
var r2 = elfs.OrderDescending().Take(3).Sum();
r2.Dump();