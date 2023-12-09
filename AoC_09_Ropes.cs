var data = File.ReadAllLines(@"c:\projects\AoC\2022\09\input.txt");

var r1 = SimulateMovement(data, new (int x, int y)[2]).Count;
r1.Dump();

var r2 = SimulateMovement(data, new (int x, int y)[10]).Count;
r2.Dump();

HashSet<(int,int)> SimulateMovement(IEnumerable<string> commands, (int x, int y)[] knots)
{
    var tailPositions = new HashSet<(int,int)>();
    
    foreach (var line in commands)
    {
        string[] parts = line.Split(' ');
        var dir = parts[0];
        var distance = int.Parse(parts[1]);
        
        (int x, int y) diff = dir switch
        {
            "R" => (1, 0),
            "L" => (-1, 0),
            "U" => (0, 1),
            "D" => (0, -1),
            _ => throw new NotImplementedException()
        };
        
        for (int i = 0; i < distance; ++i)
        {
            knots[0] = (knots[0].x + diff.x, knots[0].y + diff.y);
            for (int j = 1; j < knots.Length; ++j)
            {
                SimulateStep(ref knots[j-1], ref knots[j]);
            }
            tailPositions.Add(knots[^1]);
        }
    }
    return tailPositions;
}

void SimulateStep(ref (int x, int y) head, ref (int x, int y) tail)
{
    var (diffX, diffY) = (head.x - tail.x, head.y - tail.y);
    if (Math.Abs(diffX) <= 1 && Math.Abs(diffY) <= 1)
    {
        // do nothing
    }
    else if (diffX == 0) // diffY = {-2;2}
    {
        tail.y += diffY > 0 ? 1 : -1;
    }
    else if (diffY == 0) // diffX = {-2;2}
    {
        tail.x += diffX > 0 ? 1 : -1;
    }
    else
    {
        tail.x += diffX > 0 ? 1 : -1;
        tail.y += diffY > 0 ? 1 : -1;
    }
}