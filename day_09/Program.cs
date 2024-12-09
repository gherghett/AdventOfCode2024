
using System.Text;

string inString = "2333133121414131402";
// inString = System.IO.File.ReadAllText("input.txt");
List<Block> fileSystem = new();
for (int i = 0; i < inString.Length; i++)
{
    int amount = (int)char.GetNumericValue(inString[i]);
    if (i % 2 == 0)
    {
        //fileblocks
        fileSystem.Add(new Block { Id = i / 2, Size = amount, IsEmpty = false });
    }
    else
    {
        //empty spaces
        fileSystem.Add(new Block { Size = amount, IsEmpty = true });
    }
}
// Console.WriteLine(string.Join(",", fileSystem));
// PrintFileSystem(fileSystem);

int times = 0;
foreach (var file in fileSystem.Where(b => !b.IsEmpty).Reverse())
{
    // Console.WriteLine(file.Id);
    var leftmostThatFits = fileSystem
        .Where(b => b.IsEmpty && 
            b.Size >= file.Size && 
            fileSystem.IndexOf(b) < fileSystem.IndexOf(file))
        .FirstOrDefault();

    if (leftmostThatFits is null)
        continue;

    var indexOfRemoved = fileSystem.IndexOf(file);
    fileSystem.Insert(indexOfRemoved, new Block { Size = file.Size, IsEmpty = true });
    fileSystem.Remove(file);
    fileSystem.Insert(fileSystem.IndexOf(leftmostThatFits), file);
    leftmostThatFits.Size -= file.Size;
    PackBlocks(fileSystem);
    // PrintFileSystem(fileSystem);
    Console.WriteLine(times++);
}

// PrintFileSystem(fileSystem);
Console.WriteLine(GetCheckSum(fileSystem));

// int leftMostEmpty = GetLeftMostEmptyAfter(fileSystem);
// for (int i = fileSystem.Count - 1; i > 0; i--)
// {
//     int current = fileSystem[i];
//     int to = i;
//     while(fileSystem[to] == current || to == 0)
//         to--;

//     if (current == -1)
//         continue;

//     if (i <= leftMostEmpty)
//         break;

//     fileSystem[leftMostEmpty] = current;

//     fileSystem[i] = -1;
//     // Console.WriteLine(string.Join("", fileSystem));

//     leftMostEmpty = GetLeftMostEmptyAfter(fileSystem, leftMostEmpty);
//     if (leftMostEmpty == -1)
//         break;
// }

// Console.WriteLine(GetCheckSum(fileSystem));


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


long GetCheckSum(List<Block> fs)
{
    long sum = 0;
    long index = 0;
    for(int i = 0; i < fs.Count; i++)
    {
        if (fs[i].IsEmpty)
        {
            index += fs[i].Size;
            continue;
        }

        for(int j = 0; j  < fs[i].Size; j++)
        {
            sum += fs[i].Id * index++;
        }
    }
    return sum;
}
// Console.WriteLine(fileSystem);
/*
2_3_1_3_2_4_4_3_4_2
 3 3 3 1 1 1 1 1 0

2
*/
void PrintFileSystem(List<Block> fs)
{
    for (int i = 0; i < fs.Count; i++)
    {
        Console.Write(fs[i].Size);
    }
    Console.WriteLine();

    StringBuilder sb = new();
    int index = 0;
    for (int i = 0; i < fs.Count; i++)
    {
        if (!fs[i].IsEmpty)
            sb.Append(fs[i].Id.ToString()[0], fs[i].Size);
        else
            sb.Append('.', fs[i].Size);
    }
    Console.WriteLine(sb);
}

void PackBlocks(List<Block> fs)
{
    int index = 0;
    while (index < fs.Count - 1)
    {
        while (index < fs.Count - 1 &&
            fs[index].IsEmpty &&
            fs[index + 1].IsEmpty)
        {
            fs[index].Size += fs[index + 1].Size;
            fs.RemoveAt(index + 1);
        }
        index++;
    }
}

class Block
{
    public int Id { get; set; }
    public int Size { get; set; }
    public bool IsEmpty { get; set; } = false;


}


class File : Block
{

}

class Empty : Block
{

}