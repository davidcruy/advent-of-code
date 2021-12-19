// See https://aka.ms/new-console-template for more information

using System.Collections;

var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);
var line = reader.ReadLine();
var diagnostics = new Diagnostics();

while (line != null)
{
    diagnostics.AddLine(line);
    line = reader.ReadLine();
}

var gammaRate = diagnostics.GetMostCommon();
var epsilonRate = (BitArray) gammaRate.Clone();
epsilonRate.Not();

var iGammaRate = ToDouble(gammaRate);
var iEpsilonRate = ToDouble(epsilonRate);
Console.WriteLine($"Gamma rate: {iGammaRate}, {iEpsilonRate} : {iGammaRate * iEpsilonRate}");

Diagnostics oxyDiagnostics = null!;
Diagnostics co2Scrubber = null!;
for (var idx = 0; idx < diagnostics.Size; idx++)
{
    oxyDiagnostics = idx == 0 ? diagnostics.FilterByMostCommonBit(idx) : oxyDiagnostics.FilterByMostCommonBit(idx);
    co2Scrubber = idx == 0 ? diagnostics.FilterByMostCommonBit(idx, true) : co2Scrubber.FilterByMostCommonBit(idx, true);
}

var oxygenRating = ToDouble(oxyDiagnostics.FirstLine);
var co2ScrubberRating = ToDouble(co2Scrubber.FirstLine);

Console.WriteLine($"Oxy diagnostics: {oxygenRating}, co2 scrubber: {co2ScrubberRating} : {oxygenRating * co2ScrubberRating}");

// Methods & Classes

double ToDouble(BitArray bits)
{
    double output = 0;
    for (var i = 0; i < bits.Length; i++) output += bits[i] ? Math.Pow(2, bits.Length - 1 - i) : 0;
    return output;
}

public class Diagnostics
{
    private readonly List<BitArray> _lines = new();
    public int Size => FirstLine.Length;
    public BitArray FirstLine => _lines.First();
    
    public Diagnostics() {}
    public Diagnostics(List<BitArray> lines)
    {
        _lines = lines;
    }

    public void AddLine(string line)
    {
        var bitArray = new BitArray(line.Length);

        for (var idx = 0; idx < line.Length; idx++)
        {
            var character = line[idx];
            bitArray[idx] = character switch
            {
                '0' => false,
                '1' => true,
                _ => throw new NotImplementedException()
            };
        }
        
        _lines.Add(bitArray);
    }

    public BitArray GetMostCommon()
    {
        var result = new BitArray(Size);

        for (var i = 0; i < Size; i++) result[i] = GetMostCommonBit(i);

        return result;
    }

    public Diagnostics FilterByMostCommonBit(int idx, bool least = false)
    {
        if (_lines.Count == 1)
            return this;
        
        var b = GetMostCommonBit(idx);
        if (least) b = !b;
        var filter = _lines.Where(l => l[idx] == b).ToList();

        var d = new Diagnostics(filter);

        //Console.WriteLine($"\r\nFilter by most common, idx: {idx}, least: {least}, most-common: {b}:\r\n{d.ToString()}");

        return d;
    }

    private bool GetMostCommonBit(int idx)
    {
        var trueCount = _lines.Count(l => l[idx]);
        var falseCount = _lines.Count - trueCount;

        // If 0 and 1 are equally common, keep values with a 0 in the position being considered.
        return trueCount >= falseCount;
    }

    public string ToString() => string.Join("\r\n", _lines.Select(l => l.Cast<bool>().Aggregate("", (current, b) => current + (b ? '1' : '0'))));
}