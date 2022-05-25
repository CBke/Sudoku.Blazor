using Sudoku.Models;

namespace Sudoku.Extensions;

public static class MovePosition
{
    public static Position Move(this Position startPosition, int x, int y)
        => new()
        {
            X = startPosition.X + x,
            Y = startPosition.Y + y
        };
}