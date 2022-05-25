using Sudoku.Models;

namespace Sudoku.Extensions;

public static class SudokuExtensions
{
    public static void SetValue(this Models.Sudoku sudoku, string? value, int index)
    {
        if (int.TryParse(value, out var result))
            sudoku.SetValue(result, index);
    }

    private static void SetValue(this Models.Sudoku sudoku, int value, int index)
    {
        sudoku.Cells[index].Value = value;
        sudoku.Cells[index].PossibleValues = new List<int>();

        var position = index.ToPosition();

        foreach (var x in new Position { X = position.X, Y = 0 }.IterateHorizontal())
            sudoku.Cells[x.ToIndex()].PossibleValues.Remove(value);

        foreach (var x in new Position { X = 0, Y = position.Y }.IterateVertical())
            sudoku.Cells[x.ToIndex()].PossibleValues.Remove(value);

        foreach (var x in position.ToStartPosition().IterateQuadrant())
            sudoku.Cells[x.ToIndex()].PossibleValues.Remove(value);
    }

    private static int? HasOneOPosition(this Models.Sudoku sudoku)
        => sudoku
            .Cells
            .Select((cell, index) => new { cell, index })
            .Where(x => x.cell.Value == null)
            .FirstOrDefault(x => x.cell.PossibleValues.Count == 1)
            ?.index;

    public static void Solve(this Models.Sudoku sudoku)
    {
        var nextElementToSet = sudoku.HasOneOPosition();

        while (nextElementToSet != null)
        {
            sudoku.SetValue(sudoku.Cells[(int)nextElementToSet].PossibleValues.First(), (int)nextElementToSet);
            nextElementToSet = sudoku.HasOneOPosition();
        }
    }
}