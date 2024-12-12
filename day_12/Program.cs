using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
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

var map = new (int fences, char letter, bool counted)[lines.Length, lines[0].Length];

for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        map[i, j] = (AmountOfFences(i, j), lines[i][j], counted: false);
    }
}
List<(char letter, (int fences, int area))> plots = new();
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        if(!map[i,j].counted)
            plots.Add((map[i,j].letter, FindValuesOfPlot(i,j)));
    }
}

// for (int i = 0; i < lines.Length; i++)
// {
//     for (int j = 0; j < lines[0].Length; j++)
//     {
//         Console.Write(map[i, j].letter);
//     }
//     Console.WriteLine();
// }

int total = 0;
foreach(var plot in plots)
{
    total += plot.Item2.fences*plot.Item2.area;
    Console.WriteLine($"{plot.letter}, fences:{plot.Item2.fences}, area:{plot.Item2.area}, value: {plot.Item2.fences*plot.Item2.area} ");
}
Console.WriteLine($"total: {total}");


// for (int i = 0; i < lines.Length; i++)
// {
//     for (int j = 0; j < lines[0].Length; j++)
//     {
//         Console.Write(map[i, j].fences);
//     }
//     Console.WriteLine();
// }

int AmountOfFences(int y, int x)
    => dirs.Select(dir => (y: dir.y + y, x: dir.x + x))
        .Count(pos => !IsInsideBounds(pos)
            || lines[pos.y][pos.x] != lines[y][x]);

(int fences, int area) FindValuesOfPlot(int y, int x)
{
    if (map[y, x].counted)
        return (0, 0);


    var neighbors = dirs.Select(dir => (y: dir.y + y, x: dir.x + x))
        .Where(pos => IsInsideBounds(pos)
            && lines[pos.y][pos.x] == lines[y][x]
            && !map[pos.y, pos.x].counted).ToList();

    map[y, x].counted = true;

    if(!neighbors.Any())
        return (map[y, x].fences, area:1);

    var ValuesOfneighbors = neighbors
        .Select(n => FindValuesOfPlot(n.y, n.x) ).ToList()
        .Append((map[y, x].fences, area:1));
    
    var total = ValuesOfneighbors
        .Aggregate((sum, current) => (
            sum.fences + current.fences, // Sum the 'fences'
            sum.area + current.area    // Sum the 'area'
        ));
    return total;
}

bool IsInsideBounds((int y, int x) pos)
{
    return pos.x >= 0 && pos.y >= 0 && pos.x < lines[0].Length && pos.y < lines.Length;
}


