// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;

var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var commands = new List<Command>();
while (line != null)
{
    if (line.StartsWith("forward "))
    {
        var amount = int.Parse(line.Substring(8, line.Length - 8));
        commands.Add(new Forward { Amount = amount });
    }
    else if (line.StartsWith("down "))
    {
        var amount = int.Parse(line.Substring(5, line.Length - 5));
        commands.Add(new Down { Amount = amount });
    }
    else if (line.StartsWith("up "))
    {
        var amount = int.Parse(line.Substring(3, line.Length - 3));
        commands.Add(new Up { Amount = amount });
    }

    line = reader.ReadLine();
}

var d = 0;
var hPos = 0;
var aim = 0;

foreach (var c in commands)
{
    switch (c)
    {
        case Forward f:
            hPos += f.Amount;
            d += f.Amount * aim;
            break;
        case Down down:
            aim += down.Amount;
            break;
        case Up u:
            aim -= u.Amount;
            break;
    }

    //Console.WriteLine($"Step: h:{h_pos}, d:{d}, a:{aim}");
}

Console.WriteLine($"Final position: h:{hPos}, d:{d}, h x d: {hPos*d} ");

// Classes
public class Command
{
    public int Amount { get; set; }
}
public class Forward : Command
{
}
public class Down : Command
{
}
public class Up : Command
{
}