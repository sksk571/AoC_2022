var data = File.ReadAllLines(@"c:\projects\AoC\2022\13\input.txt");

List<Packet> allPackets = new();
int r1 = 0;
for (int i = 1, j = 0; j < data.Length; i++)
{
    var p1 = Packet.Parse(data[j++]);
    var p2 = Packet.Parse(data[j++]);
    j++;
    //Console.WriteLine(i);
    if (p1.CompareTo(p2) < 0)
    {
        r1 += i;
    }
    allPackets.Add(p1);
    allPackets.Add(p2);
}
r1.Dump();

var div1 = Packet.Parse("[[2]]");
var div2 = Packet.Parse("[[6]]");

allPackets.Add(div1);
allPackets.Add(div2);
allPackets.Sort();

int r2 = (allPackets.FindIndex(p => p.CompareTo(div1) == 0)+1) * (allPackets.FindIndex(p => p.CompareTo(div2) == 0)+1);
r2.Dump();

interface IPacketData : IComparable<IPacketData>
{
}

class Packet : IPacketData
{
    private readonly IPacketData[] _contents;

    public Packet(params IPacketData[] contents)
    {
        _contents = contents;
    }

    public int CompareTo(IPacketData? other)
    {
        //Console.WriteLine($"Comparing {this} with {other}");
        var r = other switch
        {
            Int i => CompareTo(new Packet(i)),
            Packet p => CompareTo(p),
            _ => 0
        };
        //Console.WriteLine($"Comparing {this} with {other} = {r}");
        return r;
    }

    private int CompareTo(Packet other)
    {
        int i;
        for (i = 0; i < _contents.Length && i < other._contents.Length; ++i)
        {
            int sub = _contents[i].CompareTo(other._contents[i]);
            if (sub != 0)
            {
                return sub;
            }
        }

        var r = i == _contents.Length && i == other._contents.Length ? 0 : i == _contents.Length ? -1 : 1;
        return r;
    }
    
    public static Packet Parse(ReadOnlySpan<char> s) => Parse(s, out _);

    public static Packet Parse(ReadOnlySpan<char> s, out int consumed)
    {
        List<IPacketData> contents = new();
        int i = 0, value = -1;
        while ((++i) < s.Length)
        {
            char token = s[i];
            if ((token == ',' || token == ']') && value != -1)
            {
                contents.Add(new Int(value));
                value = -1;
            }
            if (char.IsDigit(token))
            {
                if (value == -1) value = token - '0';
                else
                    value = value * 10 + (token - '0');
            }
            if (token == '[')
            {
                contents.Add(Packet.Parse(s[i..], out int read));
                i += read;
            }
            if (token == ']')
                break;
        }
        consumed = i;
        return new Packet(contents.ToArray());
    }

    public override string ToString()
    {
        return $"[{string.Join<IPacketData>(",", _contents)}]";
    }

}

class Int : IPacketData
{
    private readonly int _value;
    public Int(int value)
    {
        _value = value;
    }

    public int CompareTo(IPacketData? other)
    {
        //Console.WriteLine($"Comparing {this} with {other}");
        var r = other switch
        {
            Int i => _value.CompareTo(i._value),
            Packet p => (new Packet(this)).CompareTo(p),
            _ => 0
        };
        //Console.WriteLine($"Comparing {this} with {other} = {r}");
        return r;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}