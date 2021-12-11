// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;

var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var measurements = new List<int>();
while (line != null)
{
    measurements.Add(int.Parse(line));
    line = reader.ReadLine();
}

var summed = new List<int>();
for (var i = 0; i + 2 < measurements.Count; i++)
{
    var first = measurements[i];
    var second = measurements[i + 1];
    var third = measurements[i + 2];
    
    summed.Add(first + second + third);
}

var increased = 0;
var previous = -1;
foreach (var current in summed)
{
    if (previous != -1 && current > previous) increased++;
    previous = current;
}

Console.WriteLine($"Counter: {measurements.Count}, Increased: {increased}");