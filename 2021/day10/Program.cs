var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var others = new Dictionary<char, char>()
{
    { '(', ')' },
    { ')', '(' },
    { '[', ']' },
    { ']', '[' },
    { '{', '}' },
    { '}', '{' },
    { '<', '>' },
    { '>', '<' }
};
var scoreTable = new Dictionary<char, int>()
{
    { ')', 3 },
    { ']', 57 },
    { '}', 1197 },
    { '>', 25137 }
};
var scoreTableCompletion = new Dictionary<char, int>()
{
    { ')', 1 },
    { ']', 2 },
    { '}', 3 },
    { '>', 4 }
};
var highScoreInvalid = 0;
var incompletionScores = new List<double>();

while (!string.IsNullOrEmpty(line))
{
    var depth = new List<char>();

    foreach (var c in line)
    {
        switch (c)
        {
            case '(':
            case '[':
            case '{':
            case '<':
                depth.Add(c);
                break;
            case ')':
            case ']':
            case '}':
            case '>':
                if (TryRemove(depth, c)) break;
                else goto loops;
            default:
                Console.WriteLine($"Unknown character! {c}");
                goto loops;
        }
    }

    if (depth.Any())
    {
        double incompletionScore = 0;

        depth.Reverse();
        foreach (var c in depth)
        {
            var other = others[c];
            incompletionScore = 5 * incompletionScore;
            incompletionScore += scoreTableCompletion[other];
        }
        Console.WriteLine($" Line is incomplete! {string.Join("", depth)} {incompletionScore} points");
        incompletionScores.Add(incompletionScore);
    }

    loops:

    line = reader.ReadLine();
}

var middleIdx = incompletionScores.Count / 2;

Console.WriteLine($"High-score incomplete: {highScoreInvalid}");
Console.WriteLine($"Middle incompletion score: {incompletionScores.OrderBy(x => x).ToArray()[middleIdx]}");

bool TryRemove(List<char> depth, char c)
{
    var other = others[c];
    var last = depth.Last();
    if (last == other)
    {
        depth.RemoveAt(depth.Count - 1);
        return true;
    }

    //Console.WriteLine($"Expected {last}, but found {c} instead. {string.Join(',', depth)}");
    highScoreInvalid += scoreTable[c];
    return false;
}