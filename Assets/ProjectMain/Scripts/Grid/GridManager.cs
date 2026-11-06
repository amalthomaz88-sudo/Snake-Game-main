using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridConfig gridConfig;

    [Header("Border Appearance")]
    [SerializeField]
    private Color borderColor =
        new Color(0.25f, 0.65f, 1f, 1f);

    [SerializeField] private float borderThickness = 0.06f;

    private void Start()
    {
        if (gridConfig == null)
        {
            Debug.LogError("GridConfig is missing!");
            return;
        }

        CreateSquareBorder();
    }

    private void CreateSquareBorder()
    {
        float width = gridConfig.Width * gridConfig.CellSize;
        float height = gridConfig.Height * gridConfig.CellSize;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        Vector3[] corners =
        {
            new Vector3(-halfWidth, -halfHeight, 0),
            new Vector3(-halfWidth, halfHeight, 0),
            new Vector3(halfWidth, halfHeight, 0),
            new Vector3(halfWidth, -halfHeight, 0),
            new Vector3(-halfWidth, -halfHeight, 0)
        };

        GameObject border = new GameObject("Square Border");
        border.transform.SetParent(transform);
        border.transform.localPosition = Vector3.zero;

        LineRenderer line = border.AddComponent<LineRenderer>();

        line.positionCount = corners.Length;
        line.useWorldSpace = false;
        line.SetPositions(corners);

        line.startWidth = borderThickness;
        line.endWidth = borderThickness;

        line.startColor = borderColor;
        line.endColor = borderColor;

        line.material = new Material(
            Shader.Find("Sprites/Default")
        );

        line.sortingOrder = 10;
        line.numCapVertices = 2;
        line.numCornerVertices = 2;
        line.loop = false;
    }
}