
using System.Text;
using System.Transactions;

Console.WriteLine("Day 9!");
// string inString = "2333133121414131402";
string inString = File.ReadAllText("input.txt");
List<int> fileSystem = new();
for (int i = 0; i < inString.Length; i++)
{
    int amount = (int)char.GetNumericValue(inString[i]);
    if (i % 2 == 0)
    {
        //fileblocks
        int index = i / 2;
        for (int j = 0; j < amount; j++)
        {
            fileSystem.Add(index);
        }
    }
    else
    {
        //empty spaces
        for (int j = 0; j < amount; j++)
        {
            fileSystem.Add(-1);
        }
    }
}
// Console.WriteLine(string.Join(",", fileSystem));


int leftMostEmpty = GetLeftMostEmptyAfter(fileSystem);
for (int i = fileSystem.Count - 1; i > 0; i--)
{
    int current = fileSystem[i];
    if (current == -1)
        continue;

    if (i <= leftMostEmpty)
        break;

    fileSystem[leftMostEmpty] = current;

    fileSystem[i] = -1;
    // Console.WriteLine(string.Join("", fileSystem));

    leftMostEmpty = GetLeftMostEmptyAfter(fileSystem, leftMostEmpty);
    if (leftMostEmpty == -1)
        break;
}

Console.WriteLine($"Part 1: {GetCheckSum(fileSystem)}");
Part2Solution.Solve();

int GetLeftMostEmptyAfter(List<int> fs, int i = -1)
{
    int count = fs.Count;
    do
    {
        i++;
        if (i >= count)
            return -1;
    }
    while (fs[i] != -1);

    return i;
}

long GetCheckSum(List<int> fs)
 => fs
    .Select((i, index) => i > 0 ? (long)(i * index) : 0)
    .Sum();

// Console.WriteLine(fileSystem);
