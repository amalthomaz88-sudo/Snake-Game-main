
using System.Collections.Generic;
using UnityEngine;

public class SnakeView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private GridConfig gridConfig;
    [SerializeField] private GameObject snakeSegmentPrefab;

    private readonly List<Transform> segmentPool =
        new List<Transform>();

    private void Start()
    {
        if (snakeController == null ||
            gridConfig == null ||
            snakeSegmentPrefab == null)
        {
            Debug.LogError("SnakeView references are missing!");
            enabled = false;
            return;
        }

        CreateSegmentPool();
        UpdateSegments();
    }

    private void Update()
    {
        if (snakeController == null)
            return;

        UpdateSegments();
    }

    private void CreateSegmentPool()
    {
        int poolSize = gridConfig.Width * gridConfig.Height;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject segment =
                Instantiate(snakeSegmentPrefab, transform);

            segment.SetActive(false);
            segmentPool.Add(segment.transform);
        }
    }

    private void UpdateSegments()
    {
        IReadOnlyList<GridPosition> body = snakeController.Body;

        if (body == null)
            return;

        GridPosition[] previousPositions =
            snakeController.PreviousBodyPositions;

        int previousCount = snakeController.PreviousBodyCount;

        float progress = snakeController.MovementProgress;

        float cellSize = gridConfig.CellSize;

        float halfWidth = gridConfig.Width * cellSize * 0.5f;
        float halfHeight = gridConfig.Height * cellSize * 0.5f;

        for (int i = 0; i < segmentPool.Count; i++)
        {
            bool shouldBeActive = i < body.Count;

            if (segmentPool[i].gameObject.activeSelf != shouldBeActive)
                segmentPool[i].gameObject.SetActive(shouldBeActive);

            if (!shouldBeActive)
                continue;

            GridPosition currentPosition = body[i];

            GridPosition previousPosition;

            if (i < previousCount)
            {
                previousPosition = previousPositions[i];
            }
            else if (previousCount > 0)
            {
                // New growth segment starts at the previous tail
                previousPosition = previousPositions[previousCount - 1];
            }
            else
            {
                previousPosition = currentPosition;
            }

            Vector3 previousWorldPosition =
                GridToWorld(previousPosition, halfWidth, halfHeight, cellSize);

            Vector3 currentWorldPosition =
                GridToWorld(currentPosition, halfWidth, halfHeight, cellSize);

            segmentPool[i].position = Vector3.Lerp(
                previousWorldPosition,
                currentWorldPosition,
                progress
            );
        }
    }

    private Vector3 GridToWorld(
        GridPosition position,
        float halfWidth,
        float halfHeight,
        float cellSize)
    {
        return new Vector3(
            -halfWidth + (position.X + 0.5f) * cellSize,
            -halfHeight + (position.Y + 0.5f) * cellSize,
            -0.1f
        );
    }
}
