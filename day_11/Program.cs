// If the stone is engraved with the number 0,
//  it is replaced by a stone engraved with the
//   number 1.

// If the stone is engraved with a number that 
// has an even number of digits, it is replaced 
// by two stones. The left half of the digits are 
// engraved on the new left stone, and the right 
// half of the digits are engraved on the new right 
// stone. (The new numbers don't keep extra leading 
// zeroes: 1000 would become stones 10 and 0.)

// If none of the other rules apply, the stone is 
// replaced by a new stone; the old stone's number 
// multiplied by 2024 is engraved on the new 
// stone.

Console.WriteLine("Day 11!");

string inString = "5178527 8525 22 376299 3 69312 0 275";
List<ulong> stones = inString.Trim().Split(' ')
    .Select(s => ulong.Parse(s))
    .ToList();

Console.WriteLine(string.Join(" ", stones));

Dictionary<(ulong stone, int levels), ulong>? tested = new();
ulong total = 0;
for (int i = 0; i < stones.Count; i++)
{
    total += GetNumberOfStones(stones[i], 75, tested);
}

for(int i = 0; i < 25; i++)
{
    stones = stones.SelectMany(TransformStone).ToList();
    // Console.WriteLine(string.Join(" ", stones));
}


Console.WriteLine($"Part 1: {stones.Count}");
Console.WriteLine($"Part 2: {total}");


List<ulong> TransformStone(ulong stone)
{
    if( stone  == 0 )
        return [1];
    
    string stoneString = stone.ToString();
    if( stoneString.Length % 2 == 0 )
    {
        ulong first = ulong.Parse(stoneString.Substring(0, stoneString.Length/2));
        ulong second = ulong.Parse(stoneString.Substring(stoneString.Length/2, stoneString.Length/2));
        return [first, second];
    }

    return [stone*2024];
}


ulong GetNumberOfStones( ulong stone, int levels, Dictionary<(ulong stone, int levels), ulong>? tested = null)
{
    if(tested is null)
        tested = new();

    if(tested.ContainsKey((stone, levels)))
    {
        //Console.WriteLine("testing working");
        return tested[(stone, levels)];
    }

    if( levels == 0 )
        return 1;
    
    var stones = TransformStone(stone);

    ulong total = 0;
    foreach (var s in stones)
    {
        total += GetNumberOfStones(s, levels - 1, tested);
    }
    tested[(stone, levels)] = total;
    return total;
}