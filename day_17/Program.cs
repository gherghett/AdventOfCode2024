// See https://aka.ms/new-console-template for more information
string inString = @"Register A: 66245665
Register B: 0
Register C: 0

Program: 2,4,1,7,7,5,1,7,4,6,0,3,5,5,3,0";

string registers = inString.Split("\n\n")[0].Trim();
int regA = int.Parse(registers.Split("A: ")[1].Split("\n")[0]);
int regB = int.Parse(registers.Split("B: ")[1].Split("\n")[0]);
int regC = int.Parse(registers.Split("C: ")[1].Split("\n")[0]);


List<int> program = inString.Split("Program: ")[1].Trim().Split(",").Select(s => int.Parse(s)).ToList();
List<string> outputs = new();

Dictionary<int, Action<int>> instructions = new Dictionary<int, Action<int>>{
    {0, Adv},
    {1, Bxl},
    {2, Bst},
    {3, Jnz},
    {4, Bxc},
    {5, Out},
    {6, Bdv},
    {7, Cdv},
};
int instructionPointer = 0;

void Adv(int operand)
{   
    int combo = Combo(operand);
    regA = regA / (int)Math.Pow(2, combo);
}

void Bxl( int operand)
{
    regB = regB ^ operand;
}

void Bst( int operand)
{
    regB = Combo(operand) % 8;
}

void Jnz (int operand)
{
    if(regA != 0)
        instructionPointer = operand - 2;
}

void Bxc( int operand)
{
    regB = regB ^regC;
}

void Out(int operand)
{
    outputs.Add((Combo(operand) % 8).ToString());
}

void Bdv(int operand)
{
    int combo = Combo(operand);
    regB = regA / (int)Math.Pow(2, combo);
}

void Cdv(int operand)
{
    int combo = Combo(operand);
    regC = regA / (int)Math.Pow(2, combo);
}

int Combo(int c)
{
    switch (c)
    {
        case 0: case 1: case 2: case 3: return c;
        case 4: return regA;
        case 5: return regB;
        case 6: return regC;
        default: throw new Exception("Not expected combo");
    }
}

// Run the "program"
while(instructionPointer < program.Count)
{
    // Console.WriteLine($"Beginning at: {instructionPointer}");
    var inst = instructions[program[instructionPointer]];
    // Console.WriteLine($"running {program[instructionPointer]}");
    int operand = program[instructionPointer+1];
    inst(operand);
    instructionPointer += 2;
    // Console.WriteLine($"regA : {regA}\nregB: {regB}\nregC: {regC}");
    // Console.WriteLine($"Ending at: {instructionPointer}");
}

Console.WriteLine(string.Join(",", outputs));
