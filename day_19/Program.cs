Console.WriteLine("Day 19");
string inString = @"r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb";
inString = File.ReadAllText("input.txt");

var available = inString.Trim().Split("\n\n")[0].Split(", ");
var desiredPatterns = inString.Trim().Split("\n\n")[1].Split("\n");

var Ways = new Dictionary<string, long>();

long howManyCanBeMade = desiredPatterns.Count(d => WaysBeMade(d) > 0);
Console.WriteLine($"Part 1: {howManyCanBeMade}");

long howManyWaysCanBeMade = desiredPatterns.Select(d => WaysBeMade(d)).Sum();
Console.WriteLine($"PArt 2: {howManyWaysCanBeMade}");

long WaysBeMade( string pattern)
{
    if( pattern == "")
        return 1;

    if(Ways.ContainsKey(pattern))
        return Ways[pattern];
    
    int patternLength = pattern.Length;
    
    long ways = 0;
    foreach( var ext in available.Where(s => s.Length <= patternLength))
    {
        if( ext != pattern.Substring(0, ext.Length))
            continue;

        var rest = pattern.Substring(ext.Length, patternLength-ext.Length);

        ways += WaysBeMade(rest);
    }

    Ways[pattern] = ways;
    return ways;
}