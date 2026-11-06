
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitGrid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridConfig gridConfig;

    [Header("Camera Settings")]
    [SerializeField, Range(0.5f, 1f)]
    private float gridScreenHeight = 0.76f;

    [SerializeField, Min(1f)]
    private float extraPadding = 1.05f;

    private Camera gameCamera;

    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();

        if (gridConfig == null)
        {
            Debug.LogError("CameraFitGrid: GridConfig is missing!");
            enabled = false;
            return;
        }

        if (!gameCamera.orthographic)
        {
            Debug.LogError("CameraFitGrid requires an Orthographic Camera!");
            enabled = false;
            return;
        }

        FitCamera();
    }

    private void Update()
    {
        if (Screen.width != lastScreenWidth ||
            Screen.height != lastScreenHeight)
        {
            FitCamera();
        }
    }

    private void FitCamera()
    {
        float gridWidth =
            gridConfig.Width * gridConfig.CellSize;

        float gridHeight =
            gridConfig.Height * gridConfig.CellSize;

        float aspectRatio =
            (float)Screen.width / Screen.height;

        // Reserve space above and below the play area.
        float requiredHeight =
            gridHeight / gridScreenHeight;

        // Make sure the complete grid width is visible.
        float requiredWidth =
            gridWidth / aspectRatio;

        float requiredSize =
            Mathf.Max(requiredHeight, requiredWidth) * 0.5f;

        gameCamera.orthographicSize =
            requiredSize * extraPadding;

        // Keep the grid centred.
        transform.position = new Vector3(
            0f,
            0f,
            transform.position.z
        );

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
}
