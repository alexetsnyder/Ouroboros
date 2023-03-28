using System.Linq;
using UnityEngine;

public class VoronoiDiagram
{
    private int regions;
    private Vector2Int size;
    private Vector2Int[] vPoints;
    private Color[] vColors;

    public VoronoiDiagram(int regions, Vector2Int size)
    {
        this.regions = regions;
        this.size = size;
        this.vPoints = new Vector2Int[regions];
        this.vColors = new Color[regions];
    }

    public void GeneratePoints()
    {
        for (int i = 0; i < regions; i++)
        {
            float red = Random.Range(0.0f, 1.0f);
            float green = Random.Range(0.0f, 1.0f);
            float blue = Random.Range(0.0f, 1.0f);

            Vector2Int randomPoint = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));

            vPoints[i] = randomPoint;
            vColors[i] = new Color(red, green, blue);

        }
    }

    public Vector2Int[] GetVPoints()
    {
        return vPoints;
    }

    public Color GetColor(Vector2Int point)
    {
        int index = FindNearestVPointIndex(point);
        return vColors[index];
    }

    private int FindNearestVPointIndex(Vector2Int point)
    {
        int index = 0;
        float smallestDst = float.MaxValue;

        for (int i = 0; i < regions; i++)
        {
            float distance = Vector2.Distance(vPoints[i], point);
            if (distance < smallestDst)
            {
                smallestDst = distance;
                index = i;
            }
        }

        return index;
    }
   
}
