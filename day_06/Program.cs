// See https://aka.ms/new-console-template for more information
using System.Collections;

Console.WriteLine("Hello, World!");

var inString = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";
// var lines = inString.Split("\n");
var lines = File.ReadAllLines("input.txt");

int height = lines.Length;
int width = lines[0].Length;

char[,] map = new char[height, width];
(int y, int x) guardPos = (0, 0);
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        map[i, j] = lines[i][j];
        if (map[i, j] == '^')
            guardPos = (i, j);

    }
}
Dictionary<int, (int y, int x)> directions = new Dictionary<int, (int y, int x)>
{
    { 0, (-1, 0) }, // up
    { 1, (0, 1) },  // right
    { 2, (1, 0) },  // down
    { 3, (0, -1) }  // left
};

//start upwards
int walkingDirection = 0;

void MarkMap()
{
    while (true)
    {
        map[guardPos.y, guardPos.x] = 'X';

        (int y, int x) nextPos = (guardPos.y + directions[walkingDirection].y, guardPos.x + directions[walkingDirection].x);

        if (!IsInside(nextPos))
            return;

        // Console.WriteLine(nextPos);
        // Console.WriteLine(map[nextPos.y, nextPos.x]);

        if (map[nextPos.y, nextPos.x] == '#')
        {
            // Console.WriteLine("to ture: ", map[nextPos.y, nextPos.x]);
            // Console.WriteLine("turnnig");
            walkingDirection = (walkingDirection + 1) % 4;
        }
        else
        {
            guardPos = nextPos;
        }

        // PrintMap();
    }
}

void PrintMap()
{
    for (int i = 0; i < width; i++)
    {
        for (int j = 0; j < height; j++)
        {
            Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

MarkMap();
int XCount = 0;
for (int i = 0; i < width; i++)
{
    for (int j = 0; j < height; j++)
    {
        if (map[i, j] == 'X')
            XCount++;
    }
}

Console.WriteLine("Part 1: " + XCount);


bool IsInside((int x, int y) pos)
{
    return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
}

