// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Day 10!");
// string inString = @"89010123
// 78121874
// 87430965
// 96549874
// 45678903
// 32019012
// 01329801
// 10456732";

// var lines = inString.Split("\n");
var lines = File.ReadAllLines("input.txt");
int height = lines.Length;
int width = lines[0].Length;
int[,] map = new int[height, width];
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        map[i, j] = int.Parse("" + lines[i][j]);
    }
}

(int y, int x)[] directions = [
    (-1, 0), //up
    (0, 1), //right
    (1, 0), //down
    (0,-1), //left
];

// Part 1, DFS 
int total = 0;
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (map[i, j] == 0)
        {
            int thisCount = TrailHeadCount((0,0), -1, map, (i, j));
            // Console.WriteLine($"{i}, {j}: {thisCount}");
            total += thisCount;
        }
    }
}
Console.WriteLine($"Part 1: {total}");

int TrailHeadCount((int y, int x) lastDir, int last, int[,] map, (int y, int x) pos, HashSet<(int y, int x)>? reachedNines = null)
{
    if (!IsInsideBounds(pos))
        return 0;


    int current = map[pos.y, pos.x];

    if(reachedNines is null)
        reachedNines = new HashSet<(int y, int x)>();

    // PrintMap(pos, (char)('a' + current));

    if (last == 8 && current == 9)
    {
        reachedNines.Add(pos);
        return 0;
    }

    if (last + 1 != current)
        return 0;

    foreach (var dir in directions.Except(new (int y, int x)[] {(-lastDir.y, -lastDir.x)}))
    {
        TrailHeadCount(dir, current, map, (pos.y + dir.y, pos.x + dir.x), reachedNines);
    }
    return reachedNines.Count;
}

void PrintMap((int y, int x) pos, char write)
{
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            if (pos.y == i && pos.x == j)
                Console.Write(write);
            else
                Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

bool IsInsideBounds((int y, int x) pos)
    => pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;