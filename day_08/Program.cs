// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

string inString  = 
@"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............";

// var lines = inString.Split("\n");
var lines = File.ReadAllLines("input.txt");

int height = lines.Length;
int width = lines[0].Length;
Dictionary<char, List<(int x, int y)>> antennas = new();
for(int i = 0; i < lines.Length; i++)
{
    for(int j = 0; j < lines[0].Length; j++)
    {
        char current = lines[i][j];
        if(char.IsAsciiLetterOrDigit(current))
        {
            if(!antennas.TryAdd(current, [(i,j)]))
            {
                antennas[current].Add((i,j));
            }
        }
    }
}

Console.WriteLine(antennas.Count);

HashSet<(int x, int y)> antinodeLocations = new();
foreach(var kvp in antennas)
{
    var antennasOfOneKind = kvp.Value;
    foreach(var antenna in antennasOfOneKind)
    {
        foreach(var otherAntenna in antennasOfOneKind.Except([antenna]))
        {
            var antinode = GetAntinode(antenna, otherAntenna);
            if(IsInsideBound(antinode, width, height))
            {
                antinodeLocations.Add(antinode);
            }
        }
    }
}

Console.WriteLine(antinodeLocations.Count);

(int x, int y) GetAntinode((int x, int y)a, (int x, int y)b )
{
    return (a.x*2 - b.x, a.y*2 - b.y);
}

bool IsInsideBound((int x, int y)pos, int width, int height)
    => pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;