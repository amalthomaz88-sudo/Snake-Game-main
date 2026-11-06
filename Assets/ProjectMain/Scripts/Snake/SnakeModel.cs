
using System.Collections.Generic;

public class SnakeModel
{
    private readonly List<GridPosition> body =
        new List<GridPosition>();

    public IReadOnlyList<GridPosition> Body => body;

    public SnakeDirection Direction { get; private set; }

    public GridPosition Head => body[0];

    public int Length => body.Count;

    public SnakeModel(GridPosition startPosition)
    {
        // Head cube
        body.Add(startPosition);

        // Second cube behind the head
        body.Add(new GridPosition(
            startPosition.X - 1,
            startPosition.Y
        ));

        Direction = SnakeDirection.Right;
    }

    public void SetDirection(SnakeDirection newDirection)
    {
        if (newDirection.IsOpposite(Direction))
            return;

        Direction = newDirection;
    }

    public GridPosition GetNextPosition()
    {
        return Head + Direction.ToGridOffset();
    }

    public bool IsPositionOccupied(
        GridPosition position,
        bool ignoreTail)
    {
        int checkCount = ignoreTail
            ? body.Count - 1
            : body.Count;

        for (int i = 0; i < checkCount; i++)
        {
            if (body[i] == position)
                return true;
        }

        return false;
    }

    public void Move(int growthAmount)
    {
        GridPosition nextPosition = GetNextPosition();

        body.Insert(0, nextPosition);

        if (growthAmount <= 0)
        {
            body.RemoveAt(body.Count - 1);
        }
        else
        {
            for (int i = 1; i < growthAmount; i++)
                body.Add(body[body.Count - 1]);
        }
    }

    public void Shrink(int amount, int minimumLength = 3)
    {
        int targetLength =
            System.Math.Max(minimumLength, body.Count - amount);

        while (body.Count > targetLength)
            body.RemoveAt(body.Count - 1);
    }
}
