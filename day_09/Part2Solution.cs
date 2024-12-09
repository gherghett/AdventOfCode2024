using System.Text;
public class Part2Solution(string inString)
{
    string _inString = inString;
    List<Block> _fileSystem = ParseInputString(inString);
    long Answer {get; set;}

    private static List<Block> ParseInputString(string data)
    {
        List<Block> result = new List<Block>();
        for (int i = 0; i < data.Length; i++)
        {
            int amount = (int)char.GetNumericValue(data[i]);
            if (i % 2 == 0)
            {
                //fileblocks
                result.Add(new Block { Id = i / 2, Size = amount, IsEmpty = false });
            }
            else
            {
                //empty spaces
                result.Add(new Block { Size = amount, IsEmpty = true });
            }
        }
        return result;
    }

    public void Solve()
    {
        Console.WriteLine("Part 2");
        Console.WriteLine("Defragmenting... might take a minute");
        
        int times = 0;
        foreach (var file in _fileSystem.Where(b => !b.IsEmpty).Reverse())
        {
            // Console.WriteLine(file.Id);
            var leftmostThatFits = _fileSystem
                .Where(b => b.IsEmpty && 
                    b.Size >= file.Size && 
                    _fileSystem.IndexOf(b) < _fileSystem.IndexOf(file))
                .FirstOrDefault();

            if (leftmostThatFits is null)
                continue;

            _fileSystem.Insert(_fileSystem.IndexOf(file), new Block { Size = file.Size, IsEmpty = true });
            _fileSystem.Remove(file);
            _fileSystem.Insert(_fileSystem.IndexOf(leftmostThatFits), file);
            leftmostThatFits.Size -= file.Size;
            PackBlocks(_fileSystem);
            // PrintFileSystem(fileSystem);
            // Console.WriteLine($"{times++} ");
        }

        // PrintFileSystem(fileSystem);
        Console.WriteLine("Calculating CheckSum...");
        Answer = GetCheckSum(_fileSystem);
        Console.WriteLine(Answer);
    }


    static long GetCheckSum(List<Block> fs)
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
    
    static void PrintFileSystem(List<Block> fs)
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

    static void PackBlocks(List<Block> fs)
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

}