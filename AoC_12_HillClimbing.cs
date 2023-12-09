var data = File.ReadAllLines(@"c:\projects\AoC\2022\12\input.txt");

var destination = All(data).Single(p => data[p.x][p.y] == 'E');
int r1 = ShortestPaths(data, All(data).Where(p => data[p.x][p.y] == 'S'), destination).Single();
r1.Dump();
int r2 = ShortestPaths(data, All(data).Where(p => data[p.x][p.y] == 'a' || data[p.x][p.y] == 'S'), destination).Min();
r2.Dump();

int[] ShortestPaths(string[] data, IEnumerable<(int x, int y)> sources, (int x, int y) destination)
{
    Dictionary<(int x, int y), int> dist = new();
    Dictionary<(int x, int y), (int x, int y)> prev = new();
    HashSet<(int x, int y)> q = new();
    foreach (var v in All(data))
    {
        dist[v] = short.MaxValue;
        q.Add(v);
    }
    dist[destination] = 0;
    while (q.Count != 0)
    {
        var min = q.Min(v => dist[v]);
        var u = q.Where(v => dist[v] == min).First();
        q.Remove(u);
        foreach (var v in Neighbours(u, data).Where(q.Contains))
        {
            var alt = dist[u] + Distance(u, v, data);
            if (alt < dist[v])
            {
                dist[v] = alt;
                prev[v] = u;
            }
        }
    }
    return sources.Select(d => dist[d]).ToArray();
}


IEnumerable<(int x, int y)> All(string[] data)
{
    return Enumerable.Range(0, data.Length).SelectMany(x => Enumerable.Range(0, data[x].Length).Select(y => (x, y)));
}

IEnumerable<(int x, int y)> Neighbours((int x, int y) v, string[] data)
{
    if (v.x > 0) yield return (v.x - 1, v.y);
    if (v.x < data.Length - 1) yield return (v.x + 1, v.y);
    if (v.y > 0) yield return (v.x, v.y - 1);
    if (v.y < data[v.x].Length - 1) yield return (v.x, v.y + 1);
}

int Distance((int x, int y) u, (int x, int y) v, string[] data)
{
    if (Math.Abs(u.x - v.x) > 1 || Math.Abs(u.y - v.y) > 1)
        return short.MaxValue;

    char eu = data[u.x][u.y];
    eu = eu == 'S' ? 'a' : eu == 'E' ? 'z' : eu;
    char ev = data[v.x][v.y];
    ev = ev == 'S' ? 'a' : ev == 'E' ? 'z' : ev;
    if (eu <= ev || (eu - ev) == 1)
        return 1;
    return short.MaxValue;
}