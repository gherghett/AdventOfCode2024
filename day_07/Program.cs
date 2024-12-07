// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
string inString = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";

// var lines = inString.Split("\n");
var lines = File.ReadAllLines("input.txt");
List<(long target , List<long> nums)> values = [];
foreach (var line in lines)
{
    long testValue = long.Parse(line.Split(":")[0]);
    List<long> nums = line.Split(":")[1].Trim()
        .Split(" ")
        .Select(s => long.Parse(s.Trim()))
        .ToList();
    values.Add((testValue, nums));
}

List<Func<long, long, long>> operators = [
    (long a, long b) => a * b,
    (long a, long b) => a + b
];

long sumOfMatches = 0;
foreach (var value in values)
{
    long target = value.target;
    var nums = value.nums;
    bool canMatch = CanMatch(target, nums);
    if (canMatch)
    {
        sumOfMatches += target;
        // Console.WriteLine($"{target}, total: {sumOfMatches}");
    }
    // Console.WriteLine($"{target}: {string.Join(" ",nums)}, {canMatch}");
}
Console.WriteLine("Part 1, sum of matches: "+ sumOfMatches);

bool CanMatch(long target, List<long> nums)
{
    if (nums.Count == 1)
    {
        if(nums[0] == target)
            return true;
        else
            return false;
    }

    // it can only get bigger or stay the same so 
    if(nums[0] > target)
        return false;
    
    foreach( var op in operators)
    {
        var newList = nums.Skip(2).Prepend(op(nums[0], nums[1])).ToList();
        if(CanMatch(target,  newList))
            return true;
    }

    return false;
}