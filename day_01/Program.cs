// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Globalization;

Console.WriteLine("Hello, World!");
string inputText = File.ReadAllText("input.txt");
string[] lines = inputText.Split('\n');
List<int> left = [];
List<int> right = [];
foreach (var line in lines)
{
    string[] numberStrings = line.Split("   ");
    left.Add(int.Parse(numberStrings[0]));
    right.Add(int.Parse(numberStrings[1]));
}
left.Sort();
right.Sort();
int total = 0;
for (int i = 0; i < lines.Length; i++)
{
    total += Math.Abs(left[i] - right[i]);
}
Console.WriteLine(total);

int partTwoTotal = 0;
for (int i = 0; i < lines.Length; i++)
{
    partTwoTotal += right
        .Count(n => n == left[i]) * left[i];
}
Console.WriteLine(partTwoTotal);