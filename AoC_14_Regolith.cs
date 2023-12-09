#r "nuget: LoveSharp, 11.0.51"

var data = File.ReadAllLines(@"c:\projects\AoC\2022\14\input.txt");

HashSet<(int x, int y)> rock = new();

foreach (var line in data)
{
    var points = line.Split(" -> ").Select(p => p.Split(",")).Select(p => (x: int.Parse(p[0]), y: int.Parse(p[1])));
    foreach (var (p1, p2) in points.Zip(points.Skip(1)))
    {
        var p = p1;
        rock.Add(p);
        while (p != p2)
        {
            p = (p.x + Math.Sign(p2.x - p1.x), p.y + Math.Sign(p2.y - p1.y));
            rock.Add(p);
        }
    }
}

Love.Boot.Run(new Vis(new Simulator(rock)));

class Simulator
{
    private readonly HashSet<(int x, int y)> rock;
    private readonly HashSet<(int x, int y)> sand = new();
    private readonly int maxY;
    
    private (int x, int y) source = (500, 0);
    private int resting = 0;

    public Simulator(HashSet<(int x, int y)> rock, bool floor = false)
    {
        this.rock = rock;
        maxY = rock.Max(r => r.y) + 2;
        if (floor)
        {
            foreach (var x in Enumerable.Range(-500, 2000))
            {
                rock.Add((x, maxY));
            }
        }
    }

    public IEnumerable<(int x, int y)> Rock => rock;
    public IEnumerable<(int x, int y)> Sand => sand;
    public int Resting => resting;

    public void SimulateStep()
    {
        if (!sand.Contains(source))
            sand.Add(source);

        int resting = 0;
        foreach (var (x, y) in sand.ToList().OrderByDescending(p => p.y))
        {
            bool isResting = true;
            foreach (var newp in new[] { (x, y + 1), (x - 1, y + 1), (x + 1, y + 1), })
            {
                if (!rock.Contains(newp) && !sand.Contains(newp))
                {
                    sand.Remove((x, y));
                    if (y < maxY)
                        sand.Add(newp);
                    isResting = false;
                    break;
                }
            }
            if (isResting) resting++;
        }
        this.resting = resting;
    }
}

class Vis : Love.Scene
{
    private const int Offset = -600;
    private const int Scale = 2;

    private readonly Simulator simulator;

    public Vis(Simulator simulator)
    {
        this.simulator = simulator;
    }

    public override void Draw()
    {
        Love.Graphics.Print($"Resting: {simulator.Resting}", 20, 550);

        foreach (var (x, y) in simulator.Rock)
        {
            Love.Graphics.Rectangle(Love.DrawMode.Fill, new Love.Rectangle(x * Scale + Offset, y * Scale, 1, 1));
        }
        foreach (var (x, y) in simulator.Sand)
        {
            //Love.Graphics.Rectangle(Love.DrawMode.Fill, new Love.Rectangle(x, y, 1, 1));
            Love.Graphics.Circle(Love.DrawMode.Fill, x * Scale + Offset, y * Scale, 1);
        }
    }

    public override void Update(float dt)
    {
        simulator.SimulateStep();
        base.Update(dt);
    }
}