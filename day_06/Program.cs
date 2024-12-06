Console.WriteLine(@"Day 6!");

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
(int y, int x) guardStartPos = (0, 0);
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        map[i, j] = lines[i][j];
        if (map[i, j] == '^')
            guardStartPos = (i, j);

    }
}
Dictionary<int, (int y, int x)> directions = new Dictionary<int, (int y, int x)>
{
    { 0, (-1, 0) }, // up
    { 1, (0, 1) },  // right
    { 2, (1, 0) },  // down
    { 3, (0, -1) }  // left
};

void MarkMap(char[,] map, (int y, int x) guardPos)
{
    int walkingDirection = 0;
    while (true)
    {
        map[guardPos.y, guardPos.x] = 'X';

        (int y, int x) nextPos = (guardPos.y + directions[walkingDirection].y, guardPos.x + directions[walkingDirection].x);

        if (!IsInside(nextPos, map))
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

void PrintMap(char[,] map)
{
    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

var markedMap = (char[,])map.Clone();
MarkMap(markedMap, guardStartPos);
int XCount = 0;
for (int i = 0; i < width; i++)
{
    for (int j = 0; j < height; j++)
    {
        if (markedMap[i, j] == 'X')
            XCount++;
    }
}

Console.WriteLine("Part 1: " + XCount);

int loopPossibilies = 0;
for (int i = 0; i < width; i++)
{
    for (int j = 0; j < height; j++)
    {
        if (markedMap[i, j] == 'X' && !(guardStartPos.y == i && guardStartPos.x == j))
        {
            // PrintMap(markedMap);
            var mapWithNewObsticle = (char[,])map.Clone();
            mapWithNewObsticle[i, j] = '#';
            //PrintMap(mapWithNewObsticle);
            if (GoesToLoop(mapWithNewObsticle, guardStartPos))
            {
                loopPossibilies++;
                markedMap[i, j] = 'F';
                // Console.WriteLine("Found another loop, total:"+ loopPossibilies);
            }
        }
    }
    // Console.WriteLine($"Another line done!, line {i} of {width}");
}

Console.WriteLine("Part 2: " + loopPossibilies);


bool GoesToLoop(char[,] mapOriginal, (int y, int x) guardPos)
{
    int distance = 0;
    HashSet<(int y, int x, int dir)> visited = new HashSet<(int y, int x, int dir)>();
    char[,] map = (char[,])mapOriginal.Clone();
    int walkingDirection = 0;
    while (true)
    {
        if( !visited.Add((guardPos.y, guardPos.x, walkingDirection)) )
            return true;

        (int y, int x) nextPos = (guardPos.y + directions[walkingDirection].y, guardPos.x + directions[walkingDirection].x);

        if (!IsInside(nextPos, map))
            return false;

        if (map[nextPos.y, nextPos.x] == '#')
        {
            walkingDirection = (walkingDirection + 1) % 4;
        }
        else
        {
            guardPos = nextPos;
        }

        distance++;
        // if(distance == 1000000000 )
        // {
        //     Console.WriteLine("did " + distance + " steps here");
        //     Write2DArrayToFile(map, "out.txt");
        // }

        // PrintMap(map);
    }
}
bool IsInside((int x, int y) pos, char[,] map)
{
    return pos.x >= 0 && pos.y >= 0 && pos.x < map.GetLength(1) && pos.y < map.GetLength(0);
}


void Write2DArrayToFile(char[,] array, string filePath)
{
    using (StreamWriter writer = new StreamWriter(filePath))
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                writer.Write(array[i, j]); 
                if (j < cols - 1) writer.Write(" "); 
            }
            writer.WriteLine(); 
        }
    }
}