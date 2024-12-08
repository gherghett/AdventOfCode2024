Console.WriteLine("Day 8!");

// string inString  = 
// @"............
// ........0...
// .....0......
// .......0....
// ....0.......
// ......A.....
// ............
// ............
// ........A...
// .........A..
// ............
// ............";

// string inString = "..\n..";

// var lines = inString.Split("\n");
// var lines = File.ReadAllLines("test.txt");
var lines = File.ReadAllLines("input.txt");

int height = lines.Length;
int width = lines[1].Length;
Dictionary<char, List<(int x, int y)>> antennas = new();
for(int i = 0; i < height; i++)
{
    for(int j = 0; j < width; j++)
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

// the map was used for debugging
char [,] map = new char[height, width];
for(int i = 0; i < height; i++)
{
    for(int j = 0; j < width; j++)
    {
        map[i,j] = lines[i][j];
    }
}

Console.WriteLine(antennas.Count);

HashSet<(int x, int y)> antinodeLocationsPart1 = new();
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
                antinodeLocationsPart1.Add(antinode);
            }
        }
    }
}

HashSet<(int x, int y)> antinodeLocationsPart2 = new();
foreach(var kvp in antennas)
{
    var antennasOfOneKind = kvp.Value;
    foreach(var antenna in antennasOfOneKind)
    {
        foreach(var otherAntenna in antennasOfOneKind.Except([antenna]))
        {
            var offset = GetOffset(antenna, otherAntenna);
            (int x, int y) antinodePos = (antenna.x + offset.x, antenna.y + offset.y);

            // Antennas that have a corresponding antenna always have an antinode on top of them
            antinodeLocationsPart2.Add(antenna);
            // för debugging
            map[antenna.x,antenna.y] = '#';

            while(IsInsideBound(antinodePos, width, height))
            {
                antinodeLocationsPart2.Add(antinodePos);

                //used this for debugging
                map[antinodePos.x,antinodePos.y] = '#';
 
                antinodePos = (antinodePos.x + offset.x, antinodePos.y + offset.y);
            }
        }
    }
}

// ##....#....#
// .#.#....0...
// ..#.#0....#.
// ..##...0....
// ....0....#..
// .#...#A....#
// ...#..#.....
// #....#.#....
// ..#.....A...
// ....#....A..
// .#........#.
// ...#......##

Console.WriteLine($"part 1: {antinodeLocationsPart1.Count}");
Console.WriteLine($"part 2: {antinodeLocationsPart2.Count}");
// PrintMap(map);


(int x, int y) GetAntinode((int x, int y)a, (int x, int y)b )
{
    return (a.x*2 - b.x, a.y*2 - b.y);
}

(int x, int y) GetOffset((int x, int y)a, (int x, int y)b )
{
    return (a.x - b.x, a.y - b.y);
}

bool IsInsideBound((int x, int y)pos, int width, int height)
    => pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;

void PrintMap(char[,] map)
{
    for(int i = 0; i < height; i++)
    {
        for(int j = 0; j < width; j++)
        {
            Console.Write(map[i,j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();

}