namespace Sudoku.Models;

public class Sudoku
{
    public Cell[] Cells { get; set; } = Enumerable
        .Range(0, 9 * 9)
        .Select(x => new Cell())
        .ToArray();
}