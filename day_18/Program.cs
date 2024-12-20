
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;

string inString = @"5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0";
inString = File.ReadAllText("input.txt");
const int height = 71;
const int width = 71;
const int bytesToFall = 1024;

var dirs = new List<(int x, int y)>{
    ( 0, -1),
    ( 0,  1),
    (-1,  0),
    ( 1,  0),
};

int[,] map = new int[width, height];

var bombs = inString.Split("\n")
    .Select(s => s.Split(",").Select(n => int.Parse(n)).ToArray())
    .Select(a => (x: a[0], y: a[1]));

foreach (var b in bombs.Take(bytesToFall))
{
    map[b.x, b.y] = 1;
}
bombs = bombs.Skip(bytesToFall);

// PrintMap(map);
Console.WriteLine($"Part 1: {ShortestPath()}");

(int x, int y) bomb = (-1, -1);
while( ShortestPath() != -1 )
{
    bomb = bombs.Take(1).First();
    bombs = bombs.Skip(1);
    map[bomb.x, bomb.y] = 1;
}
Console.WriteLine($"Part 2: {bomb}");

int ShortestPath()
{
    var goal = (x: width - 1, y: height - 1);
    HashSet<(int x, int y)> visited = new();
    Queue<((int x, int y) pos, int steps)> q = new();

    q.Enqueue(((0, 0), 0));

    while (q.Count > 0)
    {
        var next = q.Dequeue();
        if (next.pos.x == goal.x && next.pos.y == goal.y)
        {
            return next.steps;
        }

        foreach (var newNode in dirs.Select(d => (pos: (x: d.x + next.pos.x, y: d.y + next.pos.y), steps: next.steps + 1)))
        {

            if (visited.Contains(newNode.pos)
                || !IsInsideBounds(newNode.pos.x, newNode.pos.y)
                || map[newNode.pos.x, newNode.pos.y] != 0)
                continue;

            visited.Add(newNode.pos);
            q.Enqueue(newNode);
        }

    }
    return -1;
}

bool IsInsideBounds(int x, int y) =>
    x >= 0 && y >= 0 && x < width && y < height;

void PrintMap(int[,] map)
{
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            Console.Write(map[j, i]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}