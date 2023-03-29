using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ClearLines()
    {
        lineRenderer.positionCount = 0;
    }

    public void SetUpTriangle(Vector2[] positions)
    {
        lineRenderer.positionCount = positions.Length + 1;
        for (int i = 0; i < positions.Length; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
        lineRenderer.SetPosition(positions.Length, positions[0]);
    }

    public void AddLines(Vector2[] positions)
    {
        int startIndex = lineRenderer.positionCount;
        lineRenderer.positionCount += positions.Length;

        for (int i = 0; i < positions.Length; i++)
        {
            lineRenderer.SetPosition(i + startIndex, positions[i]);
        }
    }
}
