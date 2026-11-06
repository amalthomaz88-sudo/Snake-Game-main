
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridConfig gridConfig;
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private GameObject foodPrefab;

    [Header("Food Types")]
    [SerializeField] private FoodData normalFood;
    [SerializeField] private FoodData bonusFood;
    [SerializeField] private FoodData shrinkFood;

    [Header("Spawn Chances")]
    [Range(0, 100)]
    [SerializeField] private int normalChance = 70;

    [Range(0, 100)]
    [SerializeField] private int bonusChance = 25;

    [Range(0, 100)]
    [SerializeField] private int shrinkChance = 5;

    private GameObject foodObject;
    private FoodView foodView;

    private GridPosition[] emptyPositions;
    private float lifetimeTimer;

    public GridPosition FoodPosition { get; private set; }
    public FoodData CurrentFoodData { get; private set; }

    private void Awake()
    {
        if (gridConfig == null || foodPrefab == null)
        {
            Debug.LogError("FoodManager references are missing!");
            enabled = false;
            return;
        }

        emptyPositions = new GridPosition[
            gridConfig.Width * gridConfig.Height
        ];

        foodObject = Instantiate(foodPrefab);

        foodView = foodObject.GetComponentInChildren<FoodView>(true);

        if (foodView == null)
        {
            Debug.LogError("FoodView is missing from Food Prefab!");
            enabled = false;
            return;
        }

        foodObject.SetActive(false);
    }

    private void Update()
    {
        if (foodObject == null ||
            !foodObject.activeSelf ||
            CurrentFoodData == null ||
            CurrentFoodData.Lifetime <= 0f)
            return;

        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Playing)
            return;

        lifetimeTimer -= Time.deltaTime;

        if (lifetimeTimer <= 0f)
            SpawnFood();
    }

    public void SpawnFood()
    {
        if (snakeController == null ||
            normalFood == null ||
            bonusFood == null ||
            shrinkFood == null)
        {
            Debug.LogError("FoodManager food references are missing!");
            return;
        }

        int emptyCount = 0;

        for (int x = 0; x < gridConfig.Width; x++)
        {
            for (int y = 0; y < gridConfig.Height; y++)
            {
                GridPosition position = new GridPosition(x, y);

                if (!snakeController.IsPositionOccupied(position))
                {
                    emptyPositions[emptyCount] = position;
                    emptyCount++;
                }
            }
        }

        if (emptyCount == 0)
        {
            foodObject.SetActive(false);
            CurrentFoodData = null;
            return;
        }

        FoodPosition = emptyPositions[Random.Range(0, emptyCount)];

        CurrentFoodData = ChooseFood();

        float worldX =
            (FoodPosition.X + 0.5f - gridConfig.Width / 2f)
            * gridConfig.CellSize;

        float worldY =
            (FoodPosition.Y + 0.5f - gridConfig.Height / 2f)
            * gridConfig.CellSize;

        foodObject.transform.position =
            new Vector3(worldX, worldY, 0f);

        foodView.SetFoodColor(CurrentFoodData.FoodColor);

        lifetimeTimer = CurrentFoodData.Lifetime;

        foodObject.SetActive(true);

        Debug.Log("Spawned Food: " + CurrentFoodData.FoodName);
    }

    private FoodData ChooseFood()
    {
        int totalChance =
            normalChance + bonusChance + shrinkChance;

        if (totalChance <= 0)
            return normalFood;

        int randomValue = Random.Range(0, totalChance);

        if (randomValue < normalChance)
            return normalFood;

        randomValue -= normalChance;

        if (randomValue < bonusChance)
            return bonusFood;

        return shrinkFood;
    }
}
