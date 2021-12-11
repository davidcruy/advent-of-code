var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();

var segmentSearch = new List<(string SolverKey, string ToSolve)>();

while (!string.IsNullOrEmpty(line))
{
    var parts = line.Split('|').ToArray();
    var solverKey = parts[0];
    var toSolve = parts[1];

    segmentSearch.Add((solverKey, toSolve));

    line = reader.ReadLine();
}

var found = 0;
var sum = 0;

foreach (var (SolverKey, ToSolve) in segmentSearch)
{
    var solver = CreateSolver(SolverKey);
    var output = ToSolve.Split(' ')
        .Where(x => !string.IsNullOrEmpty(x))
        .Select(x =>
        {
            if (CanSolve(solver, x, out int result))
            {
                found++;
                return result.ToString();
            }

            return x;
        });

    var numberLine = string.Join("", output);

    Console.WriteLine(numberLine);
    sum += int.Parse(numberLine);
}

Console.WriteLine($"Total number of lines {segmentSearch.Count}, total found {found}, sum {sum}");

// Methods
bool CanSolve((string Key, int Value)[] solver, string input, out int result)
{
    foreach (var (Key, Value) in solver)
    {
        if (Key.Length != input.Length)
            continue;

        if (Key.Any(c => !input.Contains(c)))
            continue;

        result = Value;
        return true;
    }

    result = 0;
    return false;
}

(string Key, int Value)[] CreateSolver(string solverKey)
{
    var solver = new List<(string Key, int Value)>();

    var keyItems = solverKey.Split(' ').Where(x => !string.IsNullOrEmpty(x));
    string keyFor1 = "", keyFor4 = "", keyFor6 = "";

    foreach (var item in keyItems)
    {
        switch (item.Length)
        {
            case 2:
                solver.Add((item, 1));
                keyFor1 = item;
                break;
            case 3:
                solver.Add((item, 7));
                break;
            case 4:
                solver.Add((item, 4));
                keyFor4 = item;
                break;
            case 7:
                solver.Add((item, 8));
                break;
        }
    }

    foreach (var item in keyItems.Where(x => x.Length == 6))
    {
        if (keyFor1.Any(x => !item.Contains(x)))
        {
            solver.Add((item, 6));
            keyFor6 = item;
        }
        else
        {
            if (keyFor4.Any(x => !item.Contains(x)))
                solver.Add((item, 0));
            else
                solver.Add((item, 9));
        }
    }

    foreach (var item in keyItems.Where(x => x.Length == 5))
    {
        if (keyFor1.Any(x => !item.Contains(x)))
        {
            if (item.All(x => keyFor6.Contains(x)))
                solver.Add((item, 5));
            else
                solver.Add((item, 2));
        }
        else
            solver.Add((item, 3));
    }

    return solver.ToArray();
}