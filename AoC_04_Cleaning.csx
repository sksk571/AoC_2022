var data = File.ReadAllLines(@"c:\projects\AoC\2022\04\input.txt");

var assignments = data.Select(line => line.Split(new char[] { '-', ',' })
    .Select(s => int.Parse(s)).ToList())
    .Select(arr => ((begin: arr[0], end: arr[1]), (begin: arr[2], end: arr[3]))).ToList();
    
var r1 = assignments.Where(a => a.Item1.begin >= a.Item2.begin && a.Item1.end <= a.Item2.end ||
    a.Item2.begin >= a.Item1.begin && a.Item2.end <= a.Item1.end).Count();
r1.Dump();

var r2 = assignments.Where(a => a.Item1.begin <= a.Item2.begin && a.Item1.end >= a.Item2.begin ||
    a.Item1.begin <= a.Item2.end && a.Item1.end >= a.Item2.end ||
    a.Item1.begin >= a.Item2.begin && a.Item1.end <= a.Item2.end ||
    a.Item2.begin >= a.Item1.begin && a.Item2.end <= a.Item1.end).Count();
r2.Dump();