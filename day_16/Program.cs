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


// Console.WriteLine(Solve().Count());
var all = Solve();
// PrintMap(map, reindeerStart, all);
Console.WriteLine($"Part 2, coords in all best paths: {all.Select(s => s.Item1).Distinct().Count()}");

List<((int y, int x), int price)> Solve()
{
    HashSet<((int y, int x) pos, int price)> visited = new();
    PriorityQueue<Movement, int> queue = new();

    // Dictionary<(int y, int x) , int> bestCostForTile = new();
    List<Movement> paths = new List<Movement>();
    int bestCost = int.MaxValue;

    queue.Enqueue(new Movement { Position = reindeerStart, Orientation = startOrientation }, 0);
    visited.Add((reindeerStart, ManhattanDistance(reindeerStart, end)));

    int turns = 0;
    while (queue.Count > 0)
    {
        turns++;

        Movement m = queue.Dequeue();
        if (turns % 10000 == 0)
        {
            // PrintMap(map, m.Position, AllPathPositions(m), orentations[m.Orientation]);
            Console.WriteLine($"cost: {m.PathCost}, has turn:{m.HasRotation}, movement:{m.Move}, price by itself:{m.GetPrice()}");
            Console.WriteLine($"{turns}: length of q: {queue.Count} visited: {visited.Count}");
            Console.WriteLine($"Paths found: {paths.Count}");
        }

        // heres the problem we need the cheapest route not the shortest
        if (m.Position == end)
        {
            if(m.PathCost <= bestCost)
            {
                paths.Add(m);
                bestCost = m.PathCost;
                // Console.WriteLine("found a path");
                continue;
            }
            else
            {
                break;
            }
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


            var visitedOnThisPosition = visited.Where(visited => visited.pos == newPos);

            // int pathCost = GetPathCost(nextMovement);

            var anyCheaper = visitedOnThisPosition.Where(visited => visited.price < nextMovement.PathCost - 1000).Any();

            if( !anyCheaper
                && map[newPos.y, newPos.x] != '#')
            {
                visited.Add((newPos, nextMovement.PathCost));
                var dist = ManhattanDistance(newPos, end);
                queue.Enqueue(nextMovement, nextMovement.PathCost+ManhattanDistance(newPos, end));
            }
        }
    }

    Console.WriteLine($"Part 1, best cost: {bestCost}");

    return paths.SelectMany(last => AllPathPositions(last)).Distinct().ToList();


    List<((int y , int x), int price)> AllPathPositions(Movement m)
    {
        var results = new List<((int y , int x), int price)>();
        while (m is not null)
        {
            results.Add((m.Position, m.PathCost));
            m = m.LastMovement!;
        }
        return results;
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

}

int ManhattanDistance( (int y, int x) from, (int y, int x)to) =>
    Math.Abs(to.y - from.y) + Math.Abs(to.x - from.x);

void PrintMap(char[,] map, (int y, int x) reindeer, List<((int y, int x)pos, int price)> best,  char reindeerChar = 'R')
{
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            if (i == reindeer.y && j == reindeer.x)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Write(reindeerChar);
                Console.ResetColor();
            }
            else if(best.Any(b => b.pos == (i,j)))
            {
                var b = best.Where(b => b.pos == (i,j)).First();
                Console.Write(b.price.ToString().Substring(b.price.ToString().Length-1, 1)[0]);
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