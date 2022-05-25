using Sudoku.Models;

namespace Sudoku.Extensions;

public static class SudokuExtensions
{
    public static void SetValue(this Models.Sudoku sudoku, string? value, int index)
    {
        if (int.TryParse(value, out var result))
            sudoku.SetValue(result, index);
    }

    public static void SetValue(this Models.Sudoku sudoku, int value, int index)
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
        int ntf;
        int ntf2;
        do
        {
            ntf = sudoku.Cells.SelectMany(x => x.PossibleValues).Count();

            sudoku.Solver();

            ntf2 = sudoku.Cells.SelectMany(x => x.PossibleValues).Count();
        } while (ntf > ntf2);
    }

    public static void Solver(this Models.Sudoku sudoku)
    {
        var nextElementToSet = sudoku.HasOneOPosition();

        while (nextElementToSet != null)
        {
            sudoku.SetValue(sudoku.Cells[(int)nextElementToSet].PossibleValues.First(), (int)nextElementToSet);
            nextElementToSet = sudoku.HasOneOPosition();
        }

        foreach (var x in Enumerable.Range(0, 9))
            new Position
            {
                X = x,
                Y = 0
            }
                .IterateHorizontal()
                .Select(y => y.ToIndex())
                .SelectMany(y => sudoku.Cells[y].PossibleValues, (idx, value) => new { idx, value })
                .GroupBy(y => y.value)
                .Where(y => y.Count() == 1)
                .Select(y => new { value = y.Key, idx = y.Select(z => z.idx).First() })
                .ToList()
                .ForEach(y => sudoku.SetValue(y.value, y.idx));

        foreach (var y in Enumerable.Range(0, 9))
            new Position
            {
                X = 0,
                Y = y
            }
                .IterateVertical()
                .Select(x => x.ToIndex())
                .SelectMany(x => sudoku.Cells[x].PossibleValues, (idx, value) => new { idx, value })
                .GroupBy(x => x.value)
                .Where(x => x.Count() == 1)
                .Select(x => new { value = x.Key, idx = x.Select(z => z.idx).First() })
                .ToList()
                .ForEach(x => sudoku.SetValue(x.value, x.idx));

        foreach (var x in Enumerable.Range(0, 3))
            foreach (var y in Enumerable.Range(0, 3))
                new Position
                {
                    X = x * 3,
                    Y = y * 3
                }
                    .IterateQuadrant()
                    .Select(z => z.ToIndex())
                    .SelectMany(z => sudoku.Cells[z].PossibleValues, (idx, value) => new { idx, value })
                    .GroupBy(z => z.value)
                    .Where(z => z.Count() == 1)
                    .Select(z => new { value = z.Key, idx = z.Select(a => a.idx).First() })
                    .ToList()
                    .ForEach(z => sudoku.SetValue(z.value, z.idx));
    }
}