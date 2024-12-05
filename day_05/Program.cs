// https://adventofcode.com/2024/day/5
Console.WriteLine("Hello, World!");

string inString = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47";
inString = File.ReadAllText("input.txt");

var pairs = inString.Split("\n\n")[0].Split("\n")
    .Select(line => line.Split("|").Select(int.Parse).ToArray());

var manuals = inString.Split("\n\n")[1].Split("\n")
    .Select(line => line.Split(",").Select(int.Parse).ToArray())
    .ToList();

Dictionary<int, List<int>> after = [];
Dictionary<int, List<int>> before = [];

foreach (var pair in pairs)
{
    before.TryAdd(pair[1], []);
    before[pair[1]].Add(pair[0]);

    after.TryAdd(pair[0], []);
    after[pair[0]].Add(pair[1]);
}

int totalOrdered = 0;
int totalUnorderedButFixed = 0;
foreach(var manual in manuals)
{
    if(IsValid(manual))
    {
        totalOrdered += manual[manual.Length/2];
    }
    else
    {
        var fixedManual = manual
            .OrderBy(i => i,Comparer<int>.Create((a, b) => Compare(a, b)))
            .ToArray();
        totalUnorderedButFixed += fixedManual[manual.Length/2];
    }
}

Console.WriteLine($"Part One: {totalOrdered}");
Console.WriteLine($"Part Two: {totalUnorderedButFixed}");

bool IsValid(int[] manual)
{
    for (int i = 0; i < manual.Length - 1; i++)
    {
        for (int j = i + 1; j < manual.Length; j++)
        {
            if (before.TryGetValue(manual[i], out var numbersThanShouldBeBefore))
            {
                if (numbersThanShouldBeBefore.Contains(manual[j]))
                {
                    return false;
                }
            }
        }
    }
    return true;
}

// returns wheter a should be before b
int Compare(int a, int b)
{
    if (before.TryGetValue(b, out var numbersThanShouldBeBefore))
    {
        if(numbersThanShouldBeBefore.Contains(a))
            return -1;
    }
    if (after.TryGetValue(b, out var numbersThanShouldBeAfter))
    {
        if(numbersThanShouldBeAfter.Contains(a))
            return 1;
    }
    return 0;
}
