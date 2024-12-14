// string inString = @"p=0,4 v=3,-3
// p=6,3 v=-1,-3
// p=10,3 v=-1,2
// p=2,0 v=2,-1
// p=0,0 v=1,3
// p=3,0 v=-2,-2
// p=7,6 v=-1,-3
// p=3,0 v=-1,-2
// p=9,3 v=2,3
// p=7,3 v=-1,2
// p=2,4 v=2,-3
// p=9,5 v=-3,-3";

// int roomHeight = 7;
// int roomWidth = 11;

using System.Text;

Console.WriteLine("Day 14!");

var inString = File.ReadAllText("input.txt");

int roomHeight = 103; //this is stateted in question
int roomWidth = 101;

var lines = inString.Trim().Split("\n");

var robots = lines.Select(line =>{
    int[] p = ArrayOfStringToInt(line.Split("=")[1].Split(" ")[0].Split(","));
    int[] v = ArrayOfStringToInt(line.Split("=")[2].Split(","));

    int[] ArrayOfStringToInt(string[] strings) => strings.Select(int.Parse).ToArray();

    return new Robot(Position:p, Velocity:v);
});

var movedRobots = robots.Select(robot => MoveRobot(robot, 100));
// PrintRoom(movedRobots);
int safetyFactor = GetSafetyFactor(movedRobots);
Console.WriteLine($"Part 1: {safetyFactor}");

var r = robots.ToList();
int count = 0;
bool run = true;
var sb = new StringBuilder();
// I started by print out every iteration, and soon noticed a pattern,
//  1, 104, and 207 did not look all random
// after seeing 310 was also sorta squarelooking, i looked at each 103rd iteration, and
// found a christmas tree at 7623
// for(int i = 0; i < 100; i++)
// {
//     count = 1 + i * 103;
//     r = robots.Select(robot => MoveRobot(robot,count)).ToList();
//     sb.AppendLine(PrintRoom(r));
//     sb.AppendLine(count.ToString());
// }
// File.WriteAllText("out3.txt", sb.ToString());
// Console.WriteLine($"Count of runs: {count}");

Console.WriteLine(PrintRoom(robots.Select(r => MoveRobot(r, 7623))));

Robot MoveRobot(Robot robot, int times)
{
    int x = robot.Position[0];
    int y = robot.Position[1];

    int vx = robot.Velocity[0];
    int vy = robot.Velocity[1];

    // Modulus in C# doesnt work with negative numbers, unlike for example python where -1 % 5 == 4
    int newX = ((x + vx * times) + roomWidth * times) % roomWidth;
    int newY = ((y + vy * times) + roomHeight * times ) % roomHeight;

    return robot with {Position = [newX, newY]};
}

int GetSafetyFactor( IEnumerable<Robot> robots)
{
    int quadrantWidth = (roomWidth -1) / 2;
    int quadrantHeight = (roomHeight -1) / 2;

    int upperLeft = robots.Where(r => r.Position[0] < quadrantWidth && r.Position[1] < quadrantHeight).Count();
    int upperRight = robots.Where(r => r.Position[0] > quadrantWidth && r.Position[1] < quadrantHeight).Count();
    int lowerLeft = robots.Where(r => r.Position[0] < quadrantWidth && r.Position[1] > quadrantHeight).Count();
    int lowerRight = robots.Where(r => r.Position[0] > quadrantWidth && r.Position[1] > quadrantHeight).Count();
    
    return upperLeft * upperRight * lowerLeft * lowerRight;
}

string PrintRoom(IEnumerable<Robot> robots)
{
    var sb = new StringBuilder();
    for(int i = 0; i < roomHeight; i++)
    {
        for(int j = 0; j < roomWidth; j++)
        {
            int robotCount = 0;
            foreach(var robot in robots)
            {
                if(robot.Position[0] == j && robot.Position[1] == i)
                {
                    robotCount++;
                }    
            }
            if(robotCount == 0)
                sb.Append(".");
            else    
                sb.Append(robotCount);
        }
        sb.AppendLine();
    }
    return sb.ToString();
}
record Robot(int[] Position, int[] Velocity);