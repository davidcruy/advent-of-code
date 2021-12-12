var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var rows = new List<int[]>();

while (!string.IsNullOrEmpty(line))
{
    var points = line.Select(c => c - '0').ToArray();
    rows.Add(points);
    line = reader.ReadLine();
}

var lowPoints = new List<int>();
var lowPointLocations = new List<(int x, int y)>();
var maxX = rows[0].Length - 1;
var maxY = rows.Count - 1;

for (var i = 0; i < rows.Count; i++)
{
    var points = rows[i];

    for (var j = 0; j < points.Length; j++)
    {
        var point = points[j];

        // Check previous
        if (j != 0)
            if (points[j - 1] <= point) continue;

        // Check next
        if (j < maxX)
            if (points[j + 1] <= point) continue;

        // Check top
        if (i != 0)
            if (rows[i - 1][j] <= point) continue;

        // Check bottom
        if (i < maxY)
            if (rows[i + 1][j] <= point) continue;

        //Console.WriteLine($"low point found: {point} ({i}, {j})");

        lowPoints.Add(point);
        lowPointLocations.Add((j, i));
    }
}

Console.WriteLine($"Rows {rows.Count}, low points: {string.Join(',', lowPoints)}");

var risk = lowPoints.Sum(p => p + 1);
Console.WriteLine($"Sum of risk levels: {risk}");

var bassins = new List<int>();
foreach (var point in lowPointLocations)
{
    var found = new List<(int x, int y)> { (point.x, point.y) };
    var result = LookAround(point.x, point.y, rows, found);
    bassins.Add(result.Count);
    Console.WriteLine($"Bassin ({point.x}, {point.y}): {result.Count}");
}

var multipli = 1;
foreach (var size in bassins.OrderByDescending(x => x).Take(3)) multipli *= size;

Console.WriteLine($"Multipli: {multipli}");

List<(int x, int y)> LookAround(int x, int y, IList<int[]> innerRows, List<(int x, int y)> found, int depth = 0)
{
    if (depth == 1000)
        throw new Exception("Too deep...");

    var pointHeight = innerRows[y][x];

    // previous
    if (x != 0 && innerRows[y][x - 1] != 9 && innerRows[y][x - 1] >= pointHeight)
        Merge(depth, found, innerRows, x - 1, y);

    // next
    if (x < maxX && innerRows[y][x + 1] != 9 && innerRows[y][x + 1] >= pointHeight)
        Merge(depth, found, innerRows, x + 1, y);

    // top
    if (y != 0 && innerRows[y - 1][x] != 9 && innerRows[y - 1][x] >= pointHeight)
        Merge(depth, found, innerRows, x, y - 1);

    // bottom
    if (y < maxY && innerRows[y + 1][x] != 9 && innerRows[y + 1][x] >= pointHeight)
        Merge(depth, found, innerRows, x, y + 1);

    return found;
}

void Merge(int depth, List<(int x, int y)> found, IList<int[]> innerRows, int x, int y)
{
    if (found.Any(f => f.x == x && f.y == y))
        return;

    found.Add((x, y));

    LookAround(x, y, innerRows, found, depth);
}