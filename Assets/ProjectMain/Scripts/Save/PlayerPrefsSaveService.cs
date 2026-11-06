
using UnityEngine;

public class PlayerPrefsSaveService : MonoBehaviour, ISaveService
{
    private const string HighScoreKey = "Snake_HighScore";

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(HighScoreKey, score);
        PlayerPrefs.Save();
    }
}
