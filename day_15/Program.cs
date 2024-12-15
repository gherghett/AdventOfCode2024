using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

string inString = @"##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";
inString = File.ReadAllText("input.txt");
var parts = inString.Trim().Split("\n\n");
var mapString = parts[0].Split("\n");
var movements = parts[1].Replace("\n", "");

int height = mapString.Length;
int width = mapString[0].Length;

var moves = new Dictionary<char, (int y, int x)> {
    {'<', (0, -1)},
    {'>', (0, 1)},
    {'^', (-1, 0)},
    {'v', (1, 0)},
};
(int y, int x) robot = (0, 0);
var map = new char[mapString.Length, mapString[0].Length];
for (int i = 0; i < mapString.Length; i++)
{
    for (int j = 0; j < mapString[i].Length; j++)
    {
        if (mapString[i][j] == '@')
        {
            robot = (y: i, x: j);
            map[i, j] = '.';
        }
        else
            map[i, j] = mapString[i][j];
    }
}

foreach (char instr in movements)
{
    var robotOldPos = robot;
    var dir = moves[instr];
    // Console.WriteLine($"instr: {instr}, dir: {dir}");
    var nextPos = (y: robot.y + dir.y, x: robot.x + dir.x);
    if( map[nextPos.y, nextPos.x] == 'O')
    {
        if(Push(nextPos, dir))
        {
            robot = nextPos;
        }
    }
    else if(map[nextPos.y, nextPos.x] == '.')
    {
        robot = nextPos;
    }
    // PrintMap(map, robot, robotOldPos);
    // Console.ReadLine();
}

PrintMap(map, robot);

int GPS = 0;
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (map[i,j] == 'O')
        {
            GPS += i * 100 + j;
        }
    }
}
Console.WriteLine($"Part 1: {GPS}");


bool Push((int y, int x) pos, (int y, int x) dir)
{
    var nextpos = (y: pos.y + dir.y, x: pos.x + dir.x);

    if (IsWall(nextpos))
    {
        return false;
    }
    else if (IsEmpty(nextpos))
    {
        map[nextpos.y, nextpos.x] = 'O';
        map[pos.y, pos.x] = '.';
        return true;
    }
    else if (Push(nextpos, dir))
    {
        map[nextpos.y, nextpos.x] = 'O';
        map[pos.y, pos.x] = '.';
        return true;
    }

    return false;
}

bool IsWall((int y, int x) pos) =>
     map[pos.y, pos.x] == '#';

bool IsEmpty((int y, int x) pos) =>
    map[pos.y, pos.x] == '.';


void PrintMap(char[,] map, (int y, int x) robot )
{
    for (int i = 0; i < mapString.Length; i++)
    {
        for (int j = 0; j < mapString[i].Length; j++)
        {
            if (i == robot.y && j == robot.x)
                Console.Write('@');
            // else if (i == mark.y && j == mark.x)
            //     Console.Write('x');
            else 
                Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
}
