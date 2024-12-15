using System.Data.Common;
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
// inString = @"#######
// #...#.#
// #.....#
// #..OO@#
// #..O..#
// #.....#
// #######

// <vv<<^^<<^^";
inString = File.ReadAllText("input.txt");
var parts = inString.Trim().Split("\n\n");
var mapString = parts[0].Split("\n");
var movements = parts[1].Replace("\n", "");

int height = mapString.Length;
int width = mapString[0].Length * 2;

var moves = new Dictionary<char, (int y, int x)> {
    {'<', (0, -1)},
    {'>', (0, 1)},
    {'^', (-1, 0)},
    {'v', (1, 0)},
};

(int y, int x) robot = (0, 0);

var map = new char[height, width];
for (int i = 0; i < mapString.Length; i++)
{
    for (int j = 0; j < mapString[i].Length; j++)
    {
        if (mapString[i][j] == '@')
        {
            robot = (y: i, x: j * 2);
            map[i, j * 2] = '.';
            map[i, j * 2 + 1] = '.';
        }
        else if (mapString[i][j] == 'O')
        {
            map[i, j * 2] = '[';
            map[i, j * 2 + 1] = ']';
        }
        else
        {
            map[i, j * 2] = mapString[i][j];
            map[i, j * 2 + 1] = mapString[i][j];
        }
    }
}

// PrintMap(map, robot);
// Console.ReadLine();


foreach (char instr in movements)
{
    var robotOldPos = robot;
    var dir = moves[instr];
    // Console.WriteLine($"instr: {instr}, dir: {dir}");
    var nextPos = (y: robot.y + dir.y, x: robot.x + dir.x);
    if ("[]".Contains(map[nextPos.y, nextPos.x]))
    {
        if (CanPush(nextPos, dir))
        {
            Push(nextPos, dir);
            robot = nextPos;
        }
    }
    else if (map[nextPos.y, nextPos.x] == '.')
    {
        robot = nextPos;
    }
    // PrintMap(map, robot, instr);
    // Console.ReadLine();
}
PrintMap(map, robot);

// this is for manually driving the robot
// char instr = '<';
// while(true)
// {
//     var robotOldPos = robot;
//     var dir = moves[instr];
//     Console.WriteLine($"instr: {instr}, dir: {dir}");
//     var nextPos = (y: robot.y + dir.y, x: robot.x + dir.x);
//     if ("[]".Contains(map[nextPos.y, nextPos.x]))
//     {
//         if (CanPush(nextPos, dir))
//         {
//             Push(nextPos, dir);
//             robot = nextPos;
//         }
//     }
//     else if (map[nextPos.y, nextPos.x] == '.')
//     {
//         robot = nextPos;
//     }
//     PrintMap(map, robot);
//     instr = Console.ReadKey().KeyChar switch
//     {
//         'w' => '^',
//         'a' => '<',
//         's' => 'v',
//         'd' => '>',
//         _ => '>'
//     };
// }
// PrintMap(map, robot);

int GPS = 0;
int boxes = 0;
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (map[i, j] == '[')
        {
            GPS += i * 100 + j;
            boxes++;
            if (map[i, j + 1] != ']')
                Console.WriteLine((i, j));
        }
    }
}
Console.WriteLine($"Part 2: {GPS}, boxes {boxes}");

bool CanPush((int y, int x) pos, (int y, int x) dir)
{
    if (map[pos.y, pos.x] == '.')
        return true;

    List<(int y, int x)> nextpos = [(y: pos.y + dir.y, x: pos.x + dir.x)];

    if (dir.y != 0)
    {
        char thisChar = map[pos.y, pos.x];
        if (thisChar == '[')
            nextpos.Add((y: nextpos[0].y, x: nextpos[0].x + 1));
        if (thisChar == ']')
            nextpos.Add((y: nextpos[0].y, x: nextpos[0].x - 1));
    }

    if (nextpos.Any(next => IsWall(next)))
    {
        return false;
    }

    if (nextpos.All(pos => CanPush(pos, dir)))
    {
        return true;
    }

    return false;
}

void Push((int y, int x) pos, (int y, int x) dir)
{
    if (map[pos.y, pos.x] == '.') //already been moved
        return;

    List<(int y, int x)> nextpos = [(y: pos.y + dir.y, x: pos.x + dir.x)];

    if (dir.y != 0)
    {
        char thisChar = map[pos.y, pos.x];
        if (thisChar == '[')
            nextpos.Add((y: nextpos[0].y, x: nextpos[0].x + 1));
        if (thisChar == ']')
            nextpos.Add((y: nextpos[0].y, x: nextpos[0].x - 1));
    }

    //Move children
    nextpos.ForEach(pos => Push(pos, dir));

    //Move these
    nextpos.ForEach(Move);

    void Move((int y, int x) next)
    {
        (int y, int x) old = (y: next.y - dir.y, x: next.x - dir.x);
        map[next.y, next.x] = map[old.y, old.x];
        map[old.y, old.x] = '.';
    }
}

bool IsWall((int y, int x) pos) =>
     map[pos.y, pos.x] == '#';

bool IsEmpty((int y, int x) pos) =>
    map[pos.y, pos.x] == '.';


void PrintMap(char[,] map, (int y, int x) robot, char robotChar = '@')
{
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            if (i == robot.y && j == robot.x)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(robotChar);
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


// part 1
// using System.Diagnostics.Contracts;
// using System.Runtime.CompilerServices;
// using System.Runtime.InteropServices;
// using System.Security.Cryptography.X509Certificates;

// string inString = @"##########
// #..O..O.O#
// #......O.#
// #.OO..O.O#
// #..O@..O.#
// #O#..O...#
// #O..O..O.#
// #.OO.O.OO#
// #....O...#
// ##########

// <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
// vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
// ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
// <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
// ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
// ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
// >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
// <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
// ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
// v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";
// inString = File.ReadAllText("input.txt");
// var parts = inString.Trim().Split("\n\n");
// var mapString = parts[0].Split("\n");
// var movements = parts[1].Replace("\n", "");

// int height = mapString.Length;
// int width = mapString[0].Length;

// var moves = new Dictionary<char, (int y, int x)> {
//     {'<', (0, -1)},
//     {'>', (0, 1)},
//     {'^', (-1, 0)},
//     {'v', (1, 0)},
// };
// (int y, int x) robot = (0, 0);
// var map = new char[mapString.Length, mapString[0].Length];
// for (int i = 0; i < mapString.Length; i++)
// {
//     for (int j = 0; j < mapString[i].Length; j++)
//     {
//         if (mapString[i][j] == '@')
//         {
//             robot = (y: i, x: j);
//             map[i, j] = '.';
//         }
//         else
//             map[i, j] = mapString[i][j];
//     }
// }

// foreach (char instr in movements)
// {
//     var robotOldPos = robot;
//     var dir = moves[instr];
//     // Console.WriteLine($"instr: {instr}, dir: {dir}");
//     var nextPos = (y: robot.y + dir.y, x: robot.x + dir.x);
//     if( map[nextPos.y, nextPos.x] == 'O')
//     {
//         if(Push(nextPos, dir))
//         {
//             robot = nextPos;
//         }
//     }
//     else if(map[nextPos.y, nextPos.x] == '.')
//     {
//         robot = nextPos;
//     }
//     // PrintMap(map, robot, robotOldPos);
//     // Console.ReadLine();
// }

// PrintMap(map, robot);

// int GPS = 0;
// for (int i = 0; i < height; i++)
// {
//     for (int j = 0; j < width; j++)
//     {
//         if (map[i,j] == 'O')
//         {
//             GPS += i * 100 + j;
//         }
//     }
// }
// Console.WriteLine($"Part 1: {GPS}");


// bool Push((int y, int x) pos, (int y, int x) dir)
// {
//     var nextpos = (y: pos.y + dir.y, x: pos.x + dir.x);

//     if (IsWall(nextpos))
//     {
//         return false;
//     }
//     else if (IsEmpty(nextpos))
//     {
//         map[nextpos.y, nextpos.x] = 'O';
//         map[pos.y, pos.x] = '.';
//         return true;
//     }
//     else if (Push(nextpos, dir))
//     {
//         map[nextpos.y, nextpos.x] = 'O';
//         map[pos.y, pos.x] = '.';
//         return true;
//     }

//     return false;
// }

// bool IsWall((int y, int x) pos) =>
//      map[pos.y, pos.x] == '#';

// bool IsEmpty((int y, int x) pos) =>
//     map[pos.y, pos.x] == '.';


// void PrintMap(char[,] map, (int y, int x) robot )
// {
//     for (int i = 0; i < mapString.Length; i++)
//     {
//         for (int j = 0; j < mapString[i].Length; j++)
//         {
//             if (i == robot.y && j == robot.x)
//                 Console.Write('@');
//             // else if (i == mark.y && j == mark.x)
//             //     Console.Write('x');
//             else 
//                 Console.Write(map[i, j]);
//         }
//         Console.WriteLine();
//     }
// }
