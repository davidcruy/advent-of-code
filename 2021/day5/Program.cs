// See https://aka.ms/new-console-template for more information

using System.Drawing;

var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var ventLines = new List<VentLine>();

while (line != null)
{
    var v = new VentLine(line);
    if (v.Points().Any())
        ventLines.Add(v);
    line = reader.ReadLine();
}

var maxX = ventLines.Max(v => v.Points().Max(p => p.X));
var maxY = ventLines.Max(v => v.Points().Max(p => p.Y));
int[,] grid = new int[maxX + 1, maxY + 1];

Console.WriteLine($"The grid {maxX}x{maxY}:");

// Fill grid
var overlaps = 0;
for (var y = 0; y < grid.GetLength(1); y++)
{
    for (var x = 0; x < grid.GetLength(0); x++)
    {
        var covers = ventLines.Where(v => v.Touch(new Point(x, y))).Count(v => v.Points().Any(p => p.X == x && p.Y == y));
        if (covers > 1) overlaps++;
        grid[x, y] = covers;
    }
    Console.WriteLine($"{y}/{grid.GetLength(1)}");
}

// Show grid
for (var y = 0; y < grid.GetLength(1); y++)
{
    string l = "";
    for (var x = 0; x < grid.GetLength(0); x++) l += grid[x, y].ToString().Replace('0', '.');
    Console.WriteLine(l);
}

Console.WriteLine("Overlaps: " + overlaps);

// Classes

public class VentLine
{
    private Point[] _points = null!;

    public Point Start { get; }
    public Point End { get; }

    public VentLine(string input)
    {
        var points = input.Split(" -> ");
        Start = Parse(points[0]);
        End = Parse(points[1]);
    }

    public IEnumerable<Point> Points()
    {
        EnsurePoints();
        return _points;
    }

    public bool Touch(Point other)
    {
        return Between(other.X, Start.X, End.X) && Between(other.Y, Start.Y, End.Y);
    }

    private static bool Between(int a, int startX, int endX) => a >= startX && a <= endX || a <= startX && a >= endX;

    private void EnsurePoints()
    {
        if (_points != null) return;

        // For now, only consider horizontal and vertical lines: lines where either x1 = x2 or y1 = y2.
        var points = new List<Point> { Start };
        var step = Start;

        while (step != End)
        {
            step.X = CalcStep(step.X, End.X);
            step.Y = CalcStep(step.Y, End.Y);
            points.Add(step);
        }

        _points = points.ToArray();

        Console.WriteLine($"P: {Start} -> {End}: {string.Join(" > ", _points.Select(p => p.X + "," + p.Y))}");
    }

    private static int CalcStep(int a, int b) => a > b ? a - 1 : a < b ? a + 1 : a;

    private static Point Parse(string input)
    {
        var s = input.Split(",");
        return new Point(int.Parse(s[0]), int.Parse(s[1]));
    }
}