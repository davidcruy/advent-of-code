var input = File.ReadAllText("input.txt");

var start = new Cave { Name = "start", Start = true };
var end = new Cave { Name = "end", End = true };
var caves = new Dictionary<string, Cave>();

using var reader = new StringReader(input);
var text = reader.ReadLine();

while (!string.IsNullOrEmpty(text))
{
    var connection = text.Split('-');
    var caveA = Ensure(connection[0]);
    var caveB = Ensure(connection[1]);

    caveA.Connections.Add(caveB);
    caveB.Connections.Add(caveA);

    text = reader.ReadLine();
}

Console.WriteLine($"{caves.Count} caves found");

var path = new Path();
path.Add(start);
var paths = start.Travel(path).ToList();

Console.WriteLine($"{paths.Count} paths found");
// foreach (var p in paths)
//     Console.WriteLine($"{string.Join(',', p.Caves.Select(c => c.Name))}");

// Methods & Classes

Cave Ensure(string name)
{
    if (name == "start") return start;
    if (name == "end") return end;

    Cave cave;
    if (caves.ContainsKey(name))
    {
        cave = caves[name];
    }
    else
    {
        cave = new Cave { Name = name, Big = name.All(char.IsUpper) };
        caves.Add(name, cave);
    }

    return cave;
}

public class Cave
{
    public Cave()
    {
        Connections = new List<Cave>();
    }

    public string Name { get; init; }
    public bool Start { get; init; }
    public bool End { get; init; }
    public bool Big { get; init; }
    public List<Cave> Connections { get; }

    public IEnumerable<Path> Travel(Path path, int depth = 0)
    {
        //Console.WriteLine($"Travelling {string.Join(',', path.Caves.Select(c => c.Name))}");

        depth++;
        if (depth > 100)
            throw new Exception("Travelled too deep");

        foreach (var cave in Connections)
        {
            if (cave.Start)
                continue;

            if (cave.End)
            {
                yield return path.Clone().Add(cave);
                continue;
            }

            // Skip
            if (!cave.Big && path.Contains(cave) && path.ContainsSmallVisitedTwice())
                continue;

            // Add & travel
            var innerPath = path.Clone().Add(cave);
            foreach (var p in cave.Travel(innerPath, depth))
                yield return p;
        }
    }
}

public class Path
{
    public Path()
    {
        Caves = new List<Cave>();
    }

    public List<Cave> Caves { get; }

    public Path Clone()
    {
        var path = new Path();
        foreach (var cave in Caves) path.Add(cave);

        return path;
    }

    public bool Contains(Cave c) => Caves.Any(x => x == c);

    public bool ContainsSmallVisitedTwice()
    {
        var groupedCaves = Caves.Where(x => !x.Big && !x.Start && !x.End).GroupBy(x => x);
        return groupedCaves.Any(x => x.Count() > 1);
    }

    public Path Add(Cave cave)
    {
        Caves.Add(cave);
        return this;
    }
}