using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var fishTimers = line.Split(',').Select(int.Parse).ToArray();

var groups = new List<FishAgeGroup>();
foreach (var t in fishTimers)
{
    var group = groups.FirstOrDefault(g => g.Timer == t);
    if (group == null)
        groups.Add(new FishAgeGroup(t));
    else
        group.Add();
}

var days = 256;

for (var d = 1; d <= days; d++)
{
    var newFish = groups.Sum(f => f.Tick());
    if (newFish > 0) groups.Add(new FishAgeGroup(8, newFish));
    Console.WriteLine($"Calculated day {d} ({groups.Sum(g => g.Amount)})");
}

Console.WriteLine($"There will be {groups.Sum(g => g.Amount)} after {days} days.");

// Classes

public class FishAgeGroup
{
    public int Timer { get; private set; }
    public double Amount { get; private set; }

    public FishAgeGroup(int timer, double amount = 1)
    {
        Timer = timer;
        Amount = amount;
    }

    public void Add() => Amount++;

    public double Tick()
    {
        if (Timer == 0)
        {
            Timer = 6;
            return Amount;
        }

        Timer--;
        return 0;
    }
}