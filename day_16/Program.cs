using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

string inString = @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################";

inString = File.ReadAllText("input.txt");
var mapString = inString.Split("\n");

int height = mapString.Length;
int width = mapString[0].Length;

(int y, int x) reindeerStart = (0, 0);
(int y, int x) end = (0, 0);

//starting orientation is east
var startOrientation = (y: 0, x: 1);

// var orentations = new Dictionary<char, (int y, int x)> {
//     {'<', (0, -1)},
//     {'>', (0, 1)},
//     {'^', (-1, 0)},
//     {'v', (1, 0)},
// };

var orentations = new Dictionary<(int y, int x), char> {
    {( 0, -1), '<'},
    {( 0,  1), '>'},
    {(-1,  0), '^'},
    {( 1,  0), 'v'},
};

var rotations = new List<Func<(int y, int x), (int y, int x)>>
{
    pos => (y: pos.x , x: -pos.y),
    pos => pos,
    pos => (y: -pos.x , x: pos.y)
};

// y:  -1 x: 0


var map = new char[height, width];
for (int i = 0; i < mapString.Length; i++)
{
    for (int j = 0; j < mapString[i].Length; j++)
    {
        if (mapString[i][j] == 'S')
        {
            reindeerStart = (y: i, x: j);
        }
        else if (mapString[i][j] == 'E')
        {
            end = (y: i, x: j);
        }
        map[i, j] = mapString[i][j];
        map[i, j] = mapString[i][j];

    }
}



PrintMap(map, reindeerStart);
Console.WriteLine(Solve());

int Solve()
{
    HashSet<((int y, int x) pos, int price)> visited = new();
    PriorityQueue<Movement, int> queue = new();

    queue.Enqueue(new Movement { Position = reindeerStart, Orientation = startOrientation }, 0);
    visited.Add((reindeerStart, 0));

    while (queue.Count > 0)
    {
        Movement m = queue.Dequeue();

        // heres the problem we need the cheapest route not the shortest
        if (m.Position == end)
        {
            return GetPathCost(m);
        }

        foreach (var turn in rotations)
        {
            var dir = turn(m.Orientation);
            (int y, int x) newPos = (m.Position.y + dir.y, m.Position.x + dir.x);

            var nextMovement = new Movement
            {
                LastMovement = m,
                Position = newPos,
                Move = dir,
                Orientation = dir,
                HasRotation = dir != m.Orientation
            };

            nextMovement.PathCost = m.PathCost + nextMovement.GetPrice();

            // PrintMap(map, m.Position, orentations[nextMovement.Orientation]);
            // Console.WriteLine($"cost: {nextMovement.PathCost}, has turn:{nextMovement.HasRotation}, movement:{nextMovement.Move}, price by itself:{nextMovement.GetPrice()}");

            if (!visited.Any(visited =>
                    visited.pos == newPos &&
                    nextMovement.PathCost >= visited.price)
                && map[newPos.y, newPos.x] != '#')
            {
                visited.Add((newPos, nextMovement.PathCost));
                queue.Enqueue(nextMovement, nextMovement.PathCost);
            }
        }
    }

    int GetPathCost(Movement m)
    {
        int totalCost = 0;

        while (m.LastMovement is not null)
        {
            totalCost += m.GetPrice();
            m = m.LastMovement;
        }

        return totalCost;
    }

    return 0;
}

void PrintMap(char[,] map, (int y, int x) reindeer, char reindeerChar = 'R')
{
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            if (i == reindeer.y && j == reindeer.x)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(reindeerChar);
                Console.ResetColor();
            }
            // else if (i == mark.y && j == mark.x)
            //     Console.Write('x');
            else
                Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
}

class Movement
{
    public (int y, int x) Position { get; set; }
    public (int y, int x) Orientation { get; set; } = (0, 0);
    public (int y, int x) Move { get; set; } = (0, 0);
    public bool HasRotation { get; set; } = false;
    public Movement? LastMovement = null;

    public int PathCost { get; set; } = 0;
    public int GetPrice()
    {
        int total = 0;
        if (HasRotation)
        {
            total += 1000;
        }
        if (Move != (0, 0))
        {
            total += 1;
        }
        return total;
    }
}