
using UnityEngine;

[CreateAssetMenu(
    fileName = "GridConfig",
    menuName = "Snake Game/Grid Configuration"
)]
public class GridConfig : ScriptableObject
{
    [Header("Grid Settings")]
    [SerializeField, Min(1)]
    private int width = 20;

    [SerializeField, Min(1)]
    private int height = 20;

    [SerializeField, Min(0.1f)]
    private float cellSize = 0.5f;

    [Header("Movement Settings")]
    [SerializeField, Min(0.1f)]
    private float initialMovesPerSecond = 5f;

    [Header("Speed Progression")]
    [SerializeField, Min(1)]
    private int foodsPerSpeedIncrease = 5;

    [SerializeField]
    private AnimationCurve speedCurve =
        AnimationCurve.Linear(0f, 1f, 1f, 2f);

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    public float InitialMovesPerSecond => initialMovesPerSecond;
    public int FoodsPerSpeedIncrease => foodsPerSpeedIncrease;

    public float GetMovesPerSecond(int foodCount)
    {
        float progress =
            foodCount / (float)foodsPerSpeedIncrease;

        return initialMovesPerSecond *
               speedCurve.Evaluate(progress);
    }
}
