// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Hello, World!");
string inString = "2333133121414131402";
StringBuilder sb = new StringBuilder();
for (int i = 0; i < inString.Length; i++)
{
    int amount = (int)char.GetNumericValue(inString[i]);
    if (i % 2 == 0)
    {
        //fileblocks
        sb.Append($"{i / 2}"[0], amount);
    }
    else
    {
        //empty spaces
        sb.Append('.', amount);
    }
}

int leftMostEmpty = GetLeftMostEmptyAfter(sb);
for (int i = sb.Length - 1; i > 0; i--)
{
    char current = sb[i];
    if (!char.IsDigit(current))
        continue;

    if( i <= leftMostEmpty)
        break;

    sb[leftMostEmpty] = current;
    
    sb[i] = '.';
    Console.WriteLine(sb);

    leftMostEmpty = GetLeftMostEmptyAfter(sb, leftMostEmpty);
    if (leftMostEmpty == -1)
        break;
}


int GetLeftMostEmptyAfter(StringBuilder sb, int i = -1)
{
    do
    {
        i++;
        if(i == sb.Length)
            return -1;
    }
    while (sb[i] != '.');

    return i;
}

Console.WriteLine(sb);