
Console.WriteLine("Day 20!");
string inString = @"###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############";
inString = File.ReadAllText("input.txt");
var stringMap = inString.Split('\n');
int height = stringMap.Length;
int width = stringMap[0].Length;
const int shortCutLength = 2;

int[,] map = new int[stringMap.Length, stringMap[0].Length];

List<(int y, int x)> dirs = [
    (0, 1),
    (1, 0),
    (0, -1),
    (-1, 0)
];

(int y, int x) start = (0, 0);
(int y, int x) end = (0, 0);

for (int i = 0; i < stringMap.Length; i++)
{
    for (int j = 0; j < stringMap[i].Length; j++)
    {
        map[i, j] = stringMap[i][j] != '#' ? 0 : -1;
        if (stringMap[i][j] == 'S')
            start = (i, j);
        if (stringMap[i][j] == 'E')
            end = (i, j);
    }
}
Dictionary<(int y, int x), int > positionTravels = new();
(int y, int x) traverser = start;
int travelCount = 0;
while (true)
{
    // Console.WriteLine(pos);
    map[traverser.y, traverser.x] = travelCount++;
    positionTravels[(traverser)] = travelCount;

    var nextSteps = dirs
        .Select(n => (y: n.y + traverser.y, x: n.x + traverser.x))
        .Where(p => map[p.y, p.x] == 0 && p != start);
    if (!nextSteps.Any())
        break;
    traverser = nextSteps.Single();
    // PrintMap(map);
}

List<((int y, int x), (int y, int x), int)> savedTimes = new();
foreach (var mapPos in positionTravels)
{
    int place = mapPos.Value;
    var pos = mapPos.Key;
    var shortCuts = dirs
        .Where(n => map[n.y+pos.y, n.x+pos.x] == -1)
        .Select(n => (y: n.y * shortCutLength + pos.y, x: n.x * shortCutLength + pos.x));
    foreach (var shortCutEndUpPos in shortCuts)
    {
        if(!positionTravels.ContainsKey(shortCutEndUpPos))
            continue;

        int saved = positionTravels[shortCutEndUpPos] - place - shortCutLength;

        if(saved < 100)
            continue;
        
        savedTimes.Add((mapPos.Key, shortCutEndUpPos, saved));
    }
}

List<((int y, int x), (int y, int x), int)> savedTimesPart2 = new();
foreach (var mapPos in positionTravels)
{
    int travel = mapPos.Value;
    var pos = mapPos.Key;
    var window = positionTravels.Where(kvp => Distance(kvp.Key, pos) <= 20).ToList();

    foreach ((var shortCutPos, var shortCutTravel) in window)
    {
        int saved = shortCutTravel - travel - Distance(shortCutPos, pos);

        if(saved < 100)
            continue;
        
        savedTimesPart2.Add((mapPos.Key, shortCutPos, saved));
    }
}

// PrintMap(map);

// Console.WriteLine(string.Join("\n", savedTimes.OrderBy(s => s.Item3)));
// Console.WriteLine(string.Join("\n", savedTimes.GroupBy(s => s.Item3).Select(group => (group.Key, group.Count())).OrderBy(g => g.Key)));
var atLeast100 = savedTimes
    .GroupBy(s => s.Item3)
    .Select(group => (group.Key, Count:group.Count()))
    .Where(group => group.Key >= 100 );
Console.WriteLine($"part 1: {atLeast100.Select(g => g.Count).Sum()}");

// Console.WriteLine(string.Join("\n", savedTimesPart2.OrderBy(s => s.Item3)));
// Console.WriteLine(string.Join("\n",  savedTimesPart2.GroupBy(s => s.Item3).Select(group => (group.Key, group.Count())).OrderBy(g => g.Key)));
Console.WriteLine($"part 2: {savedTimesPart2.Count()}");

int Distance((int y, int x )a, (int y, int x )b  ) =>
    (int)Math.Abs(a.y - b.y) + (int)Math.Abs(a.x - b.x);

void PrintMap(int[,] map)
{
    var chars = new Dictionary<int, char> {
        {0, '0'},
        {-1, '#'},
    };
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            char display = map[i, j] < 1 ? chars[map[i, j]] : (map[i, j] % 10).ToString()[0];
            Console.Write(display);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}