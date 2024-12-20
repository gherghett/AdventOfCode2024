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

int howManyCanBeMade = desiredPatterns.Count(d => CanBeMade(d));

Console.WriteLine(howManyCanBeMade);

bool CanBeMade( string pattern, string from = "")
{
    if( pattern == from )
        return true;
    
    var extentions = available.Select(s => from+s);
    foreach( var ext in extentions)
    {
        if( ext.Length > pattern.Length)
            continue;
        
        if( ext != pattern.Substring(0, ext.Length))
            continue;
        
        if (CanBeMade(pattern, ext) )
            return true;
    }

    return false;
}