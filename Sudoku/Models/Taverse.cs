using Sudoku.Extensions;

namespace Sudoku.Models;

public static class Taverse
{
    public static IEnumerable<Position> IterateHorizontal(this Position startPosition)
        => Enumerable
            .Range(0, 9)
            .Select(y => startPosition.Move(0, y));

    public static IEnumerable<Position> IterateVertical(this Position startPosition)
        => Enumerable
            .Range(0, 9)
            .Select(x => startPosition.Move(x, 0));

    public static IEnumerable<Position> IterateQuadrant(this Position startPosition)
        => Enumerable
            .Range(0, 3)
            .SelectMany(x => Enumerable.Range(0, 3), startPosition.Move);

    public static int ToIndex(this Position position)
        => position.X + position.Y * 9;

    public static Position ToStartPosition(this Position position)
        => new()
        {
            X = position.X - position.X % 3,
            Y = position.Y - position.Y % 3,
        };

    public static Position ToPosition(this int index)
        => new()
        {
            X = index % 9,
            Y = index / 9
        };
}