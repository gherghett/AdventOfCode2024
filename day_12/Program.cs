using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Numerics;
using System.Runtime.InteropServices;

Console.WriteLine("Day 12!");

// const string inString = @"AAAA
// BBCD
// BBCC
// EEEC";

// const string inString = @"OOOOO
// OXOXO
// OOOOO
// OXOXO
// OOOOO";

// const string inString = @"EEEEE
// EXXXX
// EEEEE
// EXXXX
// EEEEE";
// const string inString = @"AAAAAA
// AAABBA
// AAABBA
// ABBAAA
// ABBAAA
// AAAAAA";

// const string inString = @"RRRRIICCFF
// RRRRIICCCF
// VVRRRCCFFF
// VVRCCCJFFF
// VVVVCJJCFE
// VVIVCCJJEE
// VVIIICJJEE
// MIIIIIJJEE
// MIIISIJEEE
// MMMISSJEEE";

string inString = File.ReadAllText("input.txt");

var lines = inString.Trim().Split("\n");

List<(int y, int x)> dirs = [(-1, 0), (0, 1), (1, 0), (0, -1)];

var map = new char[lines.Length, lines[0].Length];

for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        map[i, j] = lines[i][j];
    }
}

HashSet<(int y, int x)> explored = new();
List<HashSet<(int y, int x)>> allPlots = new();
for (int i = 0; i < lines.Length; i++)
{
    // Console.WriteLine($"mapping line {i}");
    for (int j = 0; j < lines[0].Length; j++)
    {
        if (!explored.Contains((i, j)))
        {
            var plot = FindPlot(i, j);
            explored.UnionWith(plot);
            // Console.WriteLine("found plot of size: "+plot.Count);
            allPlots.Add(plot);
        }
    }
}

List<int> fenceCount = Enumerable.Range(0, allPlots.Count()).Select(n => 0).ToList();

// count horizontal fences 
for (int i = -1; i < lines.Length; i++)
{
    // Console.WriteLine($"line {i}");
    //one bool per plot
    List<bool> lastWasFence = Enumerable.Range(0, allPlots.Count()).Select(n => false).ToList();

    for (int j = 0; j < lines[0].Length; j++)
    {

        for (int p = 0; p < allPlots.Count; p++)
        {
            var first = (i, j);
            var second = (i +1, j);
            var isFence = IsFenceBetween(first, second)
                && (allPlots[p].Contains(first) || allPlots[p].Contains(second));

            if (!lastWasFence[p] && isFence)
            {
                fenceCount[p]++;
            }

            (int y, int x) thisPlot = allPlots[p].Contains(first) ? first : second;

            lastWasFence[p] = isFence && !IsFenceBetween(thisPlot, (thisPlot.y, thisPlot.x+1));
        }
    }
}

// count vertical fences 
for (int j = -1; j < lines[0].Length; j++)
{
    // Console.WriteLine($"vert {j}");

    //one bool per plot
    List<bool> lastWasFence = Enumerable.Range(0, allPlots.Count()).Select(n => false).ToList();

    for (int i = 0; i < lines.Length; i++)
    {

        for (int p = 0; p < allPlots.Count; p++)
        {
            var first = (i, j);
            var second = (i, j + 1);
            var isFence = IsFenceBetween(first, second)
                && (allPlots[p].Contains(first) || allPlots[p].Contains(second));

            if (!lastWasFence[p] && isFence)
            {
                fenceCount[p]++;
            }

            (int y, int x) thisPlot = allPlots[p].Contains(first) ? first : second;

            lastWasFence[p] = isFence && !IsFenceBetween(thisPlot, (thisPlot.y+1, thisPlot.x));
        }
    }
}


var part2 = allPlots.Select((p, i) => p.Count * fenceCount[i]).Sum();
var part1 = allPlots
    .Select(p => p.Count * p.Aggregate(0, (sum, current) => sum + AmountOfFences(current.y, current.x)))
    .Sum();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

bool IsFenceBetween((int y, int x) posA, (int y, int x) posB)
{
    return !IsInsideBounds(posA) || !IsInsideBounds(posB)
        || map[posA.y, posA.x] != map[posB.y, posB.x];
}

// Used for Part 1 fence calculation
int AmountOfFences(int y, int x)
    => dirs.Select(dir => (y: dir.y + y, x: dir.x + x))
        .Count(pos => !IsInsideBounds(pos)
            || lines[pos.y][pos.x] != lines[y][x]);

HashSet<(int y, int x)> FindPlot(int y, int x)
{
    HashSet<(int y, int x)> found = new();
    HashSet<(int y, int x)> explored = new();

    Queue<(int y, int x)> look = new();
    look.Enqueue((y, x));
    explored.Add((y, x)); 
    while (look.Any())
    {
        var n = look.Dequeue();
        found.Add((n.y, n.x)); 

        var neighbors = GetNeighbors((n.y, n.x));
        foreach (var neighbor in neighbors)
        {
            look.Enqueue(neighbor);
            explored.Add(neighbor); 
        }
    }

    return found;

    List<(int y, int x)> GetNeighbors((int y, int x) nPos)
    {
        return dirs.Select(dir => (y: dir.y + nPos.y, x: dir.x + nPos.x))
        .Where(pos => IsInsideBounds(pos)
            && lines[pos.y][pos.x] == lines[y][x]
            && !explored.Contains(pos)).ToList();
    }
}

bool IsInsideBounds((int y, int x) pos)
{
    return pos.x >= 0 && pos.y >= 0 && pos.x < lines[0].Length && pos.y < lines.Length;
}


