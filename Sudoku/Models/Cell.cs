namespace Sudoku.Models;

public class Cell
{
    public int? Value { get; set; }

    public List<int> PossibleValues { get; set; } = Enumerable
        .Range(1, 9)
        .ToList();
}