// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

string inString = @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279";

string buttonAPattern = @"Button A:\s*X\+(\d+),\s*Y\+(\d+)";
string buttonBPattern = @"Button B:\s*X\+(\d+),\s*Y\+(\d+)";
string prizePattern = @"Prize:\s*X=(\d+),\s*Y=(\d+)";

inString = File.ReadAllText("input.txt");

// Parse input 
var machines = inString.Trim().Split("\n\n")
    .Select(line => line.Split("\n"))
    .Select(group => new {
        ButtonA = ExtractValues(buttonAPattern, group[0]),
        ButtonB = ExtractValues(buttonBPattern, group[1]),
        Prize = ExtractValues(prizePattern, group[2])
    });

// Make a list of list of al possible button presses
List<List<(int a, int b)>> pressCombinations = new();
for (int i = 0; i < 100 + 1 ; i++)
{
    var nextList = new List<(int a, int b)>();
    for (int j = 0; j < 100 + 1 ; j++)
    {
        nextList.Add((i, j));
    }
    pressCombinations.Add(nextList);
}

var flatCombinations = pressCombinations.SelectMany(l => l);

var cost = machines
    .Select(machine =>
    {
        return flatCombinations
            .Where(presses => ReachesGoal(presses, machine.Prize, machine.ButtonA, machine.ButtonB))
            .Select(presses => Cost(presses))
            .OrderByDescending(c => c)
            .FirstOrDefault();
    })
    .Sum();

Console.WriteLine($"Part 1: {cost}");


bool ReachesGoal((int a, int b) presses, (int x, int y) goal,
    (int x, int y) aMovement, (int x, int y) bMovement)
{
    var ax = aMovement.x * presses.a;
    var ay = aMovement.y * presses.a;

    var bx = bMovement.x * presses.b;
    var by = bMovement.y * presses.b;

    var newPos = (ax + bx, ay + by);

    return newPos == goal;

    // return aMovement.x * presses.a + bMovement.x * presses.b == goal.x
    // && aMovement.y * presses.a + bMovement.y * presses.b == goal.y;
}

int Cost((int a, int b) presses)
{
    const int aPrice = 3;
    const int bPrice = 1;
    return presses.a * aPrice + presses.b * bPrice;
}

(int x, int y) ExtractValues(string pattern, string input )
{
    Match match = Regex.Match(input, pattern);

    int x = int.Parse(match.Groups[1].Value);
    int y = int.Parse(match.Groups[2].Value);
    return (x, y);

}

