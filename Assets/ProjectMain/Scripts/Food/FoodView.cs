
using UnityEngine;

public class FoodView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Blink Settings")]
    [SerializeField] private bool enableBlink = true;
    [SerializeField] private float blinkSpeed = 5f;
    [SerializeField, Range(0f, 1f)] private float minimumAlpha = 0.3f;

    private Color foodColor = Color.white;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);

        if (spriteRenderer == null)
            Debug.LogError("FoodView: SpriteRenderer is missing!");
    }

    public void SetFoodColor(Color color)
    {
        foodColor = color;
        ApplyColor(1f);
    }

    private void Update()
    {
        if (spriteRenderer == null)
            return;

        if (!enableBlink)
        {
            ApplyColor(1f);
            return;
        }

        float alpha = Mathf.Lerp(
            minimumAlpha,
            1f,
            (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f
        );

        ApplyColor(alpha);
    }

    private void ApplyColor(float alpha)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.color = new Color(
            foodColor.r,
            foodColor.g,
            foodColor.b,
            alpha
        );
    }
}
