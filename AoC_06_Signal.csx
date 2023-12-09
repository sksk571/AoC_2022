var data = File.ReadAllText(@"c:\projects\AoC\2022\06\input.txt");

var r1 = FindMarker(data, 4);
r1.Dump();

var r2 = FindMarker(data, 14);
r2.Dump();

int FindMarker(string transmission, int length)
{
    for (int i = length; i <= transmission.Length; ++i)
    {
        var slice = transmission[(i - length)..i];
        for (int j = 0; j < length - 1; ++j)
        {
            for (int k = j + 1; k < length; ++k)
            {
                if (slice[j] == slice[k])
                    goto next;
            }
        }
        return i;
    next:
        ;

        //        if (slice.Distinct().Count() == length)
        //            return i;
    }
    return 0;
}