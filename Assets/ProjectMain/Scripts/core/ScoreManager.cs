
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Save Service")]
    [SerializeField] private MonoBehaviour saveServiceComponent;

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    private ISaveService saveService;

    private void Awake()
    {
        saveService = saveServiceComponent as ISaveService;

        if (saveService == null)
        {
            Debug.LogError(
                "ScoreManager: Save Service is missing or invalid!"
            );
            return;
        }

        HighScore = saveService.LoadHighScore();
        CurrentScore = 0;

        Debug.Log("High Score Loaded: " + HighScore);
    }

    public void AddScore(int points)
    {
        if (points <= 0)
            return;

        CurrentScore += points;

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;

            if (saveService != null)
                saveService.SaveHighScore(HighScore);

            Debug.Log("New High Score Saved: " + HighScore);
        }

        Debug.Log("Current Score: " + CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        Debug.Log("Score Reset");
    }
}
