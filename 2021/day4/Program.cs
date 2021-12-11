// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt");

using var reader = new StringReader(input);

var numbers = reader.ReadLine().Split(',').Select(int.Parse).ToArray();
reader.ReadLine();
var line = reader.ReadLine();
var boardPart = "";
var boardParts = new List<string>();

while (line != null)
{
    boardPart += line + "\r\n";
    line = reader.ReadLine();
    if (line == "")
    {
        boardParts.Add(boardPart);
        boardPart = "";
        line = reader.ReadLine();
    }
}

// Add final board
boardParts.Add(boardPart);

var bingo = new Bingo();
foreach (var p in boardParts) bingo.AddBoard(p);

foreach (var n in numbers)
{
    Console.WriteLine($"{n}");
    bingo.Draw(n);

    foreach (var winner in bingo.GetWinners(n))
    {
        Console.WriteLine($"BINGO! Score = {winner}");
    }
}

Console.WriteLine($"GAME OVER");

// Methods & Classes

public class Bingo
{
    private readonly List<Board> _boards = new();

    public void AddBoard(string input)
    {
        var numbers = input.Split("\r\n").Where(x => x.Length > 12).Select(l =>
        {
            return new[]
            {
                int.Parse(l[..2]),
                int.Parse(l.Substring(3, 2)),
                int.Parse(l.Substring(6, 2)),
                int.Parse(l.Substring(9, 2)),
                int.Parse(l.Substring(12, 2))
            };
        });

        _boards.Add(new Board(numbers));
    }

    public void Draw(int number)
    {
        foreach (var b in _boards) b.Mark(number);
    }

    public IEnumerable<int> GetWinners(int number) => _boards.Where(b => b.Wins()).Select(b => b.GetScore(number));

    private class Board
    {
        public Board(IEnumerable<IEnumerable<int>> numbers)
        {
            _marks = numbers.Select(r => r.Select(n => new BoardMark(n)).ToArray()).ToArray();
        }

        private readonly BoardMark[][] _marks;
        private bool _won;

        public void Mark(int number)
        {
            foreach (var boardMark in _marks.SelectMany(m => m).Where(m => m.Number == number))
            {
                boardMark.IsMarked = true;
            }
        }

        public bool Wins()
        {
            // Already won
            if (_won) return false;

            if (_marks.Any(line => line.All(m => m.IsMarked)))
            {
                _won = true;
                return true;
            }

            for (var i = 0; i < 5; i++)
            {
                if (!_marks.All(line => line[i].IsMarked)) continue;
                _won = true;
                return true;
            }

            return false;
        }

        public int GetScore(int number) => _marks.SelectMany(m => m).Where(m => !m.IsMarked).Sum(m => m.Number) * number;
    }

    private class BoardMark
    {
        public BoardMark(int number)
        {
            Number = number;
        }

        public int Number { get; }
        public bool IsMarked { get; set; }
    }
}