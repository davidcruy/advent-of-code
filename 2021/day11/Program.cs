var input = File.ReadAllText("input.txt");

var dumbo = new int[10, 10];
var flashes = 0;
using var reader = new StringReader(input);
for (var y = 0; y < 10; y++)
{
    var line = reader.ReadLine();
    for (var x = 0; x < 10; x++)
    {
        dumbo[x, y] = int.Parse(line[x].ToString());
    }
}

var steps = 495;
for (var i = 0; i < steps; i++)
{
    // 1: Increase energy
    for (var y = 0; y < dumbo.GetLength(1); y++)
    for (var x = 0; x < dumbo.GetLength(0); x++)
        dumbo[x, y]++;

    // 2: Flash (above 9)
    var depth = 0;
    while (FlashUntilFinished())
    {
        if (AllZeroes())
        {
            Console.WriteLine($"All flashed during step {i+1}");
            goto end;
        }
        
        depth++;
        if (depth > 100)
        {
            Console.Error.WriteLine("Depth exceeded 100!");
            break;
        }
    }
}

end:
Console.WriteLine($"After {steps} steps (flashes: {flashes}): ");

for (var y = 0; y < dumbo.GetLength(1); y++)
{
    var l = "";
    for (var x = 0; x < dumbo.GetLength(0); x++) l += dumbo[x, y].ToString();
    Console.WriteLine(l);
}

// Functions

bool FlashUntilFinished()
{
    var limitHit = false;

    for (var y = 0; y < dumbo.GetLength(1); y++)
    for (var x = 0; x < dumbo.GetLength(0); x++)
    {
        if (dumbo[x, y] <= 9) continue;

        flashes++;

        // Set limit hit
        limitHit = true;

        // Adjacent flashes
        RippleFlash(x - 1, y - 1);
        RippleFlash(x, y - 1);
        RippleFlash(x + 1, y - 1);
        RippleFlash(x - 1, y + 1);
        RippleFlash(x, y + 1);
        RippleFlash(x + 1, y + 1);
        RippleFlash(x - 1, y);
        RippleFlash(x + 1, y);

        // Reset energy level
        dumbo[x, y] = 0;
    }

    return limitHit;
}

void RippleFlash(int x, int y)
{
    if (x < 0 || x > 9 || y < 0 || y > 9) return;
    if (dumbo[x, y] == 0) return;
    dumbo[x, y]++;
}

bool AllZeroes()
{
    for (var y = 0; y < dumbo.GetLength(1); y++)
    for (var x = 0; x < dumbo.GetLength(0); x++)
        if (dumbo[x, y] != 0) return false;

    return true;
}