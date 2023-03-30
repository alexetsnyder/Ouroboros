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
