
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentState = GameState.MainMenu;
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
            SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
            SetState(GameState.Playing);
    }

    public void EndGame()
    {
        SetState(GameState.GameOver);
    }

    public void ReturnToMainMenu()
    {
        SetState(GameState.MainMenu);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        Time.timeScale = newState == GameState.Playing ? 1f : 0f;

        Debug.Log("Game State: " + CurrentState);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && CurrentState == GameState.Playing)
            PauseGame();
    }
}
