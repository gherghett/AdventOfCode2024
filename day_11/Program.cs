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

string inString = "5178527 8525 22 376299 3 69312 0 275";
List<long> stones = inString.Trim().Split(' ')
    .Select(s => long.Parse(s))
    .ToList();

Console.WriteLine(string.Join(" ", stones));


for(int i = 0; i < 25; i++)
{
    stones = stones.SelectMany(TransformStone).ToList();
    // Console.WriteLine(string.Join(" ", stones));
}

Console.WriteLine(stones.Count());

List<long> TransformStone(long stone)
{
    if( stone  == 0 )
        return [1];
    
    string stoneString = stone.ToString();
    if( stoneString.Length % 2 == 0 )
    {
        long first = long.Parse(stoneString.Substring(0, stoneString.Length/2));
        long second = long.Parse(stoneString.Substring(stoneString.Length/2, stoneString.Length/2));
        return [first, second];
    }

    return [stone*2024];
}
