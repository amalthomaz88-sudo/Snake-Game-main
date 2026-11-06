
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridConfig gridConfig;
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private FoodManager foodManager;
    [SerializeField] private ScoreManager scoreManager;

    private SnakeModel snakeModel;

    private GridPosition[] previousBodyPositions;
    private int previousBodyCount;

    private float movementTimer;
    private float bonusTimer;

    private int activeBonusMultiplier = 1;
    private int foodsCollected;

    private bool isGameOver;

    public IReadOnlyList<GridPosition> Body => snakeModel?.Body;
    public GridPosition[] PreviousBodyPositions => previousBodyPositions;
    public int PreviousBodyCount => previousBodyCount;
    public bool IsGameOver => isGameOver;
    public float BonusTimeRemaining => bonusTimer;
    public int FoodsCollected => foodsCollected;

    public float MovementProgress
    {
        get
        {
            if (snakeModel == null)
                return 0f;

            float speed = gridConfig.GetMovesPerSecond(foodsCollected);

            if (speed <= 0f)
                return 0f;

            return Mathf.Clamp01(movementTimer / (1f / speed));
        }
    }

    private void Awake()
    {
        if (gridConfig == null || inputReader == null ||
            foodManager == null || scoreManager == null)
        {
            Debug.LogError("SnakeController references are missing!");
            enabled = false;
            return;
        }

        previousBodyPositions =
            new GridPosition[gridConfig.Width * gridConfig.Height];

        CreateSnake();
    }

    private void Update()
    {
        if (snakeModel == null || isGameOver)
            return;

        HandlePauseInput();

        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Playing)
            return;

        UpdateBonusTimer();
        MoveSnake();
    }

    private void CreateSnake()
    {
        GridPosition startPosition = new GridPosition(
            gridConfig.Width / 2,
            gridConfig.Height / 2
        );

        snakeModel = new SnakeModel(startPosition);

        SavePreviousPositions();
        movementTimer = 0f;
    }

    private void SavePreviousPositions()
    {
        IReadOnlyList<GridPosition> body = snakeModel.Body;
        previousBodyCount = body.Count;

        for (int i = 0; i < body.Count; i++)
            previousBodyPositions[i] = body[i];
    }

    public bool IsPositionOccupied(GridPosition position)
    {
        return snakeModel != null &&
               snakeModel.IsPositionOccupied(position, false);
    }

    public void ResetGame()
    {
        inputReader.ClearDirectionBuffer();

        CreateSnake();

        movementTimer = 0f;
        bonusTimer = 0f;
        activeBonusMultiplier = 1;
        foodsCollected = 0;
        isGameOver = false;

        scoreManager.ResetScore();
        foodManager.SpawnFood();

        Debug.Log("Game Restarted!");
    }

    private void HandlePauseInput()
    {
        if (!inputReader.ConsumePausePressed() ||
            GameStateManager.Instance == null)
            return;

        if (GameStateManager.Instance.CurrentState == GameState.Playing)
            GameStateManager.Instance.PauseGame();
        else if (GameStateManager.Instance.CurrentState == GameState.Paused)
            GameStateManager.Instance.ResumeGame();
    }

    private void UpdateBonusTimer()
    {
        if (bonusTimer <= 0f)
            return;

        bonusTimer -= Time.deltaTime;

        if (bonusTimer <= 0f)
        {
            bonusTimer = 0f;
            activeBonusMultiplier = 1;
        }
    }

    private void MoveSnake()
    {
        movementTimer += Time.deltaTime;

        int movesThisFrame = 0;
        const int maxMovesPerFrame = 5;

        while (movesThisFrame < maxMovesPerFrame)
        {
            float speed = gridConfig.GetMovesPerSecond(foodsCollected);
            float moveInterval = 1f / Mathf.Max(0.1f, speed);

            if (movementTimer < moveInterval)
                break;

            movementTimer -= moveInterval;
            movesThisFrame++;

            if (inputReader.TryConsumeDirection(
                out SnakeDirection newDirection))
            {
                snakeModel.SetDirection(newDirection);
            }

            GridPosition nextPosition = snakeModel.GetNextPosition();

            if (nextPosition.X < 0 ||
                nextPosition.X >= gridConfig.Width ||
                nextPosition.Y < 0 ||
                nextPosition.Y >= gridConfig.Height)
            {
                EndGame();
                return;
            }

            bool isEating = nextPosition == foodManager.FoodPosition;

            FoodData foodData =
                isEating ? foodManager.CurrentFoodData : null;

            int growthAmount =
                foodData != null ? foodData.GrowthAmount : 0;

            bool tailWillMove = growthAmount == 0;

            if (snakeModel.IsPositionOccupied(
                nextPosition, tailWillMove))
            {
                EndGame();
                return;
            }

            SavePreviousPositions();

            snakeModel.Move(growthAmount);

            if (foodData != null)
            {
                CollectFood(foodData);

                if (foodData.ShrinkAmount > 0)
                {
                    snakeModel.Shrink(
                        foodData.ShrinkAmount,
                        3
                    );
                }

                foodsCollected++;
                foodManager.SpawnFood();
            }
        }
    }

    private void CollectFood(FoodData foodData)
    {
        int points = foodData.ScoreValue;

        if (foodData.BonusDuration <= 0f && bonusTimer > 0f)
            points *= activeBonusMultiplier;

        scoreManager.AddScore(points);

        if (foodData.BonusDuration > 0f)
        {
            bonusTimer = foodData.BonusDuration;
            activeBonusMultiplier = foodData.BonusScoreMultiplier;
        }

        Debug.Log("Food Collected: " + foodData.FoodName);
    }

    private void EndGame()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.EndGame();

        Debug.Log("GAME OVER! Snake collided.");
    }
}
