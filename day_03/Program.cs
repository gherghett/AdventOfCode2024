// https://adventofcode.com/2024/day/3
using System.Text.RegularExpressions;

//Part 1
Console.WriteLine("Part 1");
string inputString = File.ReadAllText("in.txt");
string pattern = @"mul\((\d{1,}),(\d{1,})\)";

MatchCollection matches = Regex.Matches(inputString, pattern);
int total = 0;
foreach (Match match in matches)
{
    total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
}
Console.WriteLine(total);

Console.WriteLine("Part 2");

string[] enabledInstructions = inputString.Split("do()")
    .Select(s => s.Split("don't()")[0])
    .ToArray();

int totalPart2 = enabledInstructions.Select(GetProduct).Sum();

Console.WriteLine(totalPart2);

int GetProduct(string inString)
{
    string pattern = @"mul\((\d{1,}),(\d{1,})\)";
    MatchCollection matches = Regex.Matches(inString, pattern);
    int total = 0;
    foreach (Match match in matches)
    {
        // Console.WriteLine($"Found match: {match.Value}");
        // Console.WriteLine($"First number: {match.Groups[1].Value}");
        // Console.WriteLine($"Second number: {match.Groups[2].Value}");
        total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
    }
    return total;
}
//Console.WriteLine(GetProduct(inputString));



