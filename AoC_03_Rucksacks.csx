var data = File.ReadAllLines(@"c:\projects\AoC\2022\03\input.txt");

var r1 = data.SelectMany(line => GetDuplicates(line).Select(dupe => GetPriority(dupe))).Sum();
r1.Dump();

var r2 = data.Chunk(3).Select(group => GetPriority(GetCommon(group))).Sum();
r2.Dump();

int GetPriority(char item)
{
    if (item >= 'a' && item <= 'z')
        return item - 'a' + 1;
    if (item >= 'A' && item <= 'Z')
        return item - 'A' + 27;
    return 0;
}

IEnumerable<char> GetDuplicates(string contents)
{
    var half = contents.Length/2;
    var (left, right) = (contents[..half], contents[half..]);
    return left.Where(right.Contains).Distinct();
}

char GetCommon(IEnumerable<string> rucksacks)
{
    return rucksacks.SelectMany(c => c).Distinct().Single(c => rucksacks.All(r => r.Contains(c)));
}