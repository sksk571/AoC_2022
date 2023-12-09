var data = File.ReadAllLines(@"c:\tprojects\AoC\2022\07\input.txt");

DirectoryNode root = new DirectoryNode("/");
DirectoryNode pwd = root;
var enumerator = ((IEnumerable<string>)data).GetEnumerator();
var hasNext = enumerator.MoveNext();
while (hasNext)
{
    if (enumerator.Current.StartsWith("$"))
    {
        string commandLine = enumerator.Current[1..].TrimStart();
        string[] args = commandLine.Split(' ');
        string command = args[0];
        //Console.WriteLine($"{string.Join(",", args)}");
        switch (command)
        {
            case "cd":
                if (args[1] == "/")
                    pwd = root;
                else if (args[1] == "..")
                    pwd = pwd.Up;
                else
                {
                    var node = pwd.Items.Find(dir => dir.Name == args[1]);
                    //Console.WriteLine($"Changing directory to {node}");
                    if (node is DirectoryNode dir)
                        pwd = dir;
                }
                hasNext = enumerator.MoveNext();
                break;
            case "ls":
                hasNext = enumerator.MoveNext();
                while (hasNext && !enumerator.Current.StartsWith("$"))
                {
                    var parts = enumerator.Current.Split(' ');
                    if (parts[0] == "dir")
                    {
                        pwd.Items.Add(new DirectoryNode(parts[1], pwd));
                    }
                    else
                    {
                        pwd.Items.Add(new FileNode(parts[1], long.Parse(parts[0])));
                    }
                    hasNext = enumerator.MoveNext();
                }
                break;
        }
    }
    //hasNext = enumerator.MoveNext();
}
//root.Dump();
long r1 = root.Enumerate().Where(item => item is DirectoryNode && item.Size <= 100000).Sum(item => item.Size);

r1.Dump();

long totalSpace = 70000000;
long required = 30000000;
long occupiedSpace = root.Size;
long r2 = root.Enumerate().Where(item => item is DirectoryNode).OrderBy(item => item.Size).First(item => (totalSpace - occupiedSpace + item.Size) >= required).Size;
r2.Dump();

interface INode
{
    long Size { get; }
    string Name { get; }
}

class DirectoryNode : INode
{
    public DirectoryNode Up { get; }
    public List<INode> Items { get; }

    public long Size => Items.Sum(i => i.Size);

    public string Name { get; }

    public DirectoryNode(string name)
    {
        Up = this;
        Items = new List<INode>();
        Name = name;
    }

    public DirectoryNode(string name, DirectoryNode up)
        : this(name)
    {
        Up = up;
    }
    
    public IEnumerable<INode> Enumerate()
    {
        foreach (var item in Items)
        {
            yield return item;
            if (item is DirectoryNode dir)
            {
                foreach (var subItem in dir.Enumerate())
                {
                    yield return subItem;
                }
            }
        }
    }

    public override string ToString()
    {
        return $"{Name} (dir)";
    }

}

class FileNode : INode
{
    public long Size { get; }

    public string Name { get; }

    public FileNode(string name, long size)
    {
        Name = name;
        Size = size;
    }
    
    public override string ToString()
    {
        return $"{Name} ({Size})";
    }
}