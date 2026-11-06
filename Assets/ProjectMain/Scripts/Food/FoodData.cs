
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Snake Game/Food Data")]
public class FoodData : ScriptableObject
{
    [Header("Food Information")]
    [SerializeField] private string foodName = "Normal";
    [SerializeField] private Color foodColor = Color.white;
    [SerializeField] private int scoreValue = 1;

    [Header("Snake Effect")]
    [SerializeField] private int growthAmount = 1;
    [SerializeField] private int shrinkAmount = 0;

    [Header("Bonus Settings")]
    [SerializeField] private float lifetime = 0f;
    [SerializeField] private float bonusDuration = 0f;
    [SerializeField] private int bonusScoreMultiplier = 1;

    public string FoodName => foodName;
    public Color FoodColor => foodColor;
    public int ScoreValue => scoreValue;
    public int GrowthAmount => growthAmount;
    public int ShrinkAmount => shrinkAmount;
    public float Lifetime => lifetime;
    public float BonusDuration => bonusDuration;
    public int BonusScoreMultiplier => bonusScoreMultiplier;
}
