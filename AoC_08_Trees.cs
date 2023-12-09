var data = File.ReadAllLines(@"c:\projects\AoC\2022\08\input.txt");

var grid = data.Select(line => line.Select(cell => int.Parse(cell.ToString())).ToArray()).ToArray();

int N = grid.Length;
var r1 = Enumerable.Range(0, N).SelectMany(x => Enumerable.Range(0, N).Select(y => (x,  y)))
    .Where(p => IsVisible(p.x, p.y, grid, N, out _)).Count();
r1.Dump();

var r2 = Enumerable.Range(0, N).SelectMany(x => Enumerable.Range(0, N).Select(y => (x,  y)))
    .Select(p => { IsVisible(p.x, p.y, grid, N, out var score); return score; }).Max();
r2.Dump();

static bool IsVisible(int x, int y, int[][] grid, int N, out int score)
{
    int tree = grid[x][y];
    var visible = false;
    var scores = new int[4];
    for (int i = x + 1; i <= N; ++i)
    {
        if (i == N)
        {
            visible = true;
            break;
        }
        scores[0]++;
        if (grid[i][y] >= tree)
            break;
    }
    for (int i = x - 1; i >= -1; --i)
    {
        if (i == -1)
        {
            visible = true;
            break;
        }
        scores[1]++;
        if (grid[i][y] >= tree)
            break;
    }
    for (int j = y + 1; j <= N; ++j)
    {
        if (j == N)
        {
            visible = true;
            break;
        }
        scores[2]++;
        if (grid[x][j] >= tree)
            break;
    }
    for (int j = y - 1; j >= -1; --j)
    {
        if (j == -1)
        {
            visible = true;
            break;
        }
        scores[3]++;
        if (grid[x][j] >= tree)
            break;
    }
    score = scores.Aggregate((a, b) => a * b);
    return visible;
}