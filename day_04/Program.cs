// https://adventofcode.com/2024/day/4
// part 1
Console.WriteLine("Part 1!");
string inputText = @"....XXMAS.
.SAMXMS...
...S..A...
..A.A.MS.X
XMASAMX.MM
X.....XA.A
S.S.S.S.SS
.A.A.A.A.A
..M.M.M.MM
.X.X.XMASX";

(int y, int x)[] directions = {
    (0, 1), //right
    (1, 0), //down
    (-1, 0), //up
    (0, -1), //left

    (1, 1), //downright
    (1, -1), //downleft
    (-1, -1), //upleft
    (-1, 1) //upright
};
string xmas = "XMAS";
// string[] lines = inputText.Split("\r\n");
string[] lines = File.ReadAllLines("input.txt");
int width = lines[0].Length;
int height = lines.Length;
int xmasCount = 0;

// Count XMAS in all directions
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        foreach(var dir in directions)
        {
            if(IsXmas(dir, (i, j)))
                xmasCount++;
        }
    }
}

Console.WriteLine($"XMAS count: {xmasCount}");

bool IsXmas((int y, int x) dir, (int y, int x) pos)
{
    foreach (char c in xmas)
    {
        if(!IsInsideBounds(pos))
                return false;

        if (c != lines[pos.y][pos.x])
            return false;

        else
        {
            pos = (pos.y + dir.y, pos.x + dir.x);
        }
    }
    return true;
}

Console.WriteLine("Part 2!");
string inputTextPart2 =
@".M.S......
..A..MSMS.
.M.S.MAA..
..A.ASMSM.
.M.S.M....
..........
S.S.S.S.S.
.A.A.A.A..
M.M.M.M.M.
..........";

int xDashMasCount = 0;

for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (IsMasCrossed((i, j)))
        {
            xDashMasCount++;
        }
    }
}

Console.WriteLine($"MAS cross count: {xDashMasCount}");

// takes the position if the 'A' in MAS, and      M S
// looks if its the middle of two crossed MASSES   A
//                                                M S    
bool IsMasCrossed((int y, int x) pos)
{
    // if the A is on the edge it cannot be a mas-cross
    if (pos.x == 0 || pos.y == 0 || pos.x == width - 1 || pos.y == height - 1)
        return false;

    if (lines[pos.y][pos.x] != 'A')
        return false;

    (int y, int x) upRight = (-1, 1);
    (int y, int x) downRight = (1, 1);

    return IsMas(pos, upRight) && IsMas(pos, downRight);
}

// reads a three characters on the map, and checks if its MAS or SAM
bool IsMas((int y, int x)aPos, (int y, int x)dir)
{
    string line = "";
    (int y, int x) pos = (aPos.y-dir.y, aPos.x-dir.x);
    for(int i = 0; i < 3; i++)
    {
        if(!IsInsideBounds(pos))
            return false;

        line += lines[pos.y][pos.x];
        pos = (pos.y+dir.y, pos.x+dir.x);
    }
    if(line == "MAS" || line == "SAM")
        return true;
    return false;
}

bool IsInsideBounds((int y, int x) pos)
    => pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;


