
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("References")]
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Score Text")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private GameState lastState;

    private void Start()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManager is missing!");
            return;
        }

        lastState = GameStateManager.Instance.CurrentState;

        UpdatePanels();
        UpdateScore();
    }

    private void Update()
    {
        if (GameStateManager.Instance == null)
            return;

        GameState currentState =
            GameStateManager.Instance.CurrentState;

        if (currentState != lastState)
        {
            lastState = currentState;
            UpdatePanels();
        }

        UpdateScore();
    }

    private void UpdatePanels()
    {
        if (GameStateManager.Instance == null)
            return;

        GameState state =
            GameStateManager.Instance.CurrentState;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(state == GameState.MainMenu);

        if (gameplayPanel != null)
            gameplayPanel.SetActive(state == GameState.Playing);

        if (pausePanel != null)
            pausePanel.SetActive(state == GameState.Paused);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(state == GameState.GameOver);

        UpdateHighScore();
    }

    private void UpdateScore()
    {
        if (scoreManager == null)
            return;

        if (scoreText != null)
        {
            scoreText.text =
                "Score: " + scoreManager.CurrentScore;
        }

        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if (scoreManager == null || highScoreText == null)
            return;

        highScoreText.text =
            "High Score: " + scoreManager.HighScore;
    }

    public void StartButton()
    {
        if (snakeController == null ||
            GameStateManager.Instance == null)
            return;

        snakeController.ResetGame();

        GameStateManager.Instance.StartGame();

        RefreshUI();
    }

    public void PauseButton()
    {
        if (GameStateManager.Instance == null)
            return;

        GameStateManager.Instance.PauseGame();
        RefreshUI();
    }

    public void ResumeButton()
    {
        if (GameStateManager.Instance == null)
            return;

        GameStateManager.Instance.ResumeGame();
        RefreshUI();
    }

    public void RestartButton()
    {
        if (snakeController == null ||
            GameStateManager.Instance == null)
            return;

        snakeController.ResetGame();

        GameStateManager.Instance.StartGame();

        RefreshUI();
    }

    public void MainMenuButton()
    {
        if (GameStateManager.Instance == null)
            return;

        GameStateManager.Instance.ReturnToMainMenu();

        RefreshUI();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void RefreshUI()
    {
        if (GameStateManager.Instance == null)
            return;

        lastState = GameStateManager.Instance.CurrentState;

        UpdatePanels();
        UpdateScore();
    }

   
}
