var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var crabPosition = line.Split(',').Select(int.Parse).ToArray();
var crabsByPosition = crabPosition.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
var min = crabsByPosition.Keys.Min();
var max = crabsByPosition.Keys.Max();

var allFuelByPosition = new List<(int Pos, int Fuel)>();
for (var x = min; x <= max; x++)
{
    var totalFuel = 0;
    foreach (var (key, count) in crabsByPosition)
    {
        var steps = Math.Abs(x - key);
        var fuel = 0;
        for (var i = 1; i <= steps; i++) fuel += i;
        totalFuel += fuel * count;
        //Console.WriteLine($"Move from {x} to {key}: {fuel} fuel");
    }
    
    allFuelByPosition.Add((x, totalFuel));
    //Console.WriteLine($"Move to {x}: {totalFuel} fuel.");
}

var cheapestFuel = allFuelByPosition.Min(x => x.Fuel);
var cheapestPos = allFuelByPosition.Where(x => x.Fuel == cheapestFuel).Select(x => x.Pos);

Console.WriteLine($"Total fuel spend {cheapestFuel}, pos: {string.Join(", ", cheapestPos)}");