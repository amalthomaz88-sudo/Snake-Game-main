
public enum SnakeDirection
{
    Up,
    Down,
    Left,
    Right
}

public static class SnakeDirectionExtensions
{
    public static GridPosition ToGridOffset(this SnakeDirection direction)
    {
        switch (direction)
        {
            case SnakeDirection.Up:
                return new GridPosition(0, 1);

            case SnakeDirection.Down:
                return new GridPosition(0, -1);

            case SnakeDirection.Left:
                return new GridPosition(-1, 0);

            case SnakeDirection.Right:
                return new GridPosition(1, 0);

            default:
                return new GridPosition(0, 0);
        }
    }

    public static bool IsOpposite(
        this SnakeDirection direction,
        SnakeDirection other)
    {
        return
            (direction == SnakeDirection.Up && other == SnakeDirection.Down) ||
            (direction == SnakeDirection.Down && other == SnakeDirection.Up) ||
            (direction == SnakeDirection.Left && other == SnakeDirection.Right) ||
            (direction == SnakeDirection.Right && other == SnakeDirection.Left);
    }
}
