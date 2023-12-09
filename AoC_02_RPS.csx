var data = File.ReadAllLines(@"c:\projects\AoC\2022\02\input.txt");
var r1 = 0;

foreach (var line in data)
{
    var (enemy, me) = (line[0], line[2]);
    switch (me)
    {
        case 'X': // Rock
            r1 += 1;
            r1 += enemy == 'A' ? 3 : enemy == 'B' ? 0 : 6;
            break;
        case 'Y': // Paper
            r1 += 2;
            r1 += enemy == 'A' ? 6 : enemy == 'B' ? 3 : 0;
            break;
        case 'Z': // Scissors
            r1 += 3;
            r1 += enemy == 'A' ? 0 : enemy == 'B' ? 6 : 3; 
            break;
    }
}
r1.Dump();
var r2 = 0;

foreach (var line in data)
{
    var (enemy, me) = (line[0], line[2]);
    switch (me)
    {
        case 'X': // Lose
            r2 += 0;
            r2 += enemy == 'A' ? 3 : enemy == 'B' ? 1 : 2;
            break;
        case 'Y': // Draw
            r2 += 3;
            r2 += enemy == 'A' ? 1 : enemy == 'B' ? 2 : 3;
            break;
        case 'Z': // Win
            r2 += 6;
            r2 += enemy == 'A' ? 2 : enemy == 'B' ? 3 : 1; 
            break;
    }
}
r2.Dump();