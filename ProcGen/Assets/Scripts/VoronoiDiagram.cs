using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell
{
    public Vector2 SeedPoint { get; set; }   
    public List<Edge> Edges { get; set; }

    public VoronoiCell(Vector2 seedPoint)
    {
        SeedPoint = seedPoint;
        Edges = new List<Edge>();
    }
}

public class VoronoiDiagram
{
    private int regions;

    private Vector2Int size;
    private Vector2Int[] vPoints;
    private Color[] vColors;

    private List<Edge> edges;

    public Dictionary<Vector2, VoronoiCell> Cells{ get; private set; }
    

    public VoronoiDiagram(int regions, Vector2Int size)
    {
        this.regions = regions;
        this.size = size;
        this.vPoints = new Vector2Int[regions];
        this.vColors = new Color[regions];
        this.edges = new List<Edge>();
        this.Cells = new Dictionary<Vector2, VoronoiCell>();
    }

    public void GeneratePoints()
    {
        for (int i = 0; i < regions; i++)
        {
            float red = Random.Range(0.0f, 1.0f);
            float green = Random.Range(0.0f, 1.0f);
            float blue = Random.Range(0.0f, 1.0f);

            Vector2Int randomPoint = new Vector2Int(Random.Range(1, size.x - 1), Random.Range(1, size.y - 1));

            vPoints[i] = randomPoint;
            vColors[i] = new Color(red, green, blue);
        }
    }

    public void GenerateDiagram()
    {
        edges.Clear();

        List<Triangle> triangles = GenerateTriangulation();
        List<Edge> visited = new List<Edge>();

        for (int i = 0; i < triangles.Count - 1; i++)
        {
            var triangle = triangles[i];
            foreach (var edge in triangle.GetEdges())
            {
                if (!visited.Contains(edge))
                {
                    visited.Add(edge);
                    for (int j = i; j < triangles.Count; j++)
                    {
                        var otherTriangle = triangles[j];
                        if (otherTriangle.Contains(edge))
                        {
                            RectInt clipRect = new RectInt(0, 0, size.x, size.y);
                            Edge voronoiEdge = new Edge(triangle.CircumCenter, otherTriangle.CircumCenter).Clip(clipRect);

                            if (voronoiEdge != null)
                            {
                                VoronoiCell cell1 = CreateGetVoronoiCell(edge.v1);
                                VoronoiCell cell2 = CreateGetVoronoiCell(edge.v2);

                                cell1.Edges.Add(voronoiEdge);
                                cell2.Edges.Add(voronoiEdge);
                                edges.Add(voronoiEdge);
                            }
                        }
                    }
                }
            }
        }

    }

    private VoronoiCell CreateGetVoronoiCell(Vector2 seedPoint)
    {
        if (!Cells.ContainsKey(seedPoint))
        {
            Cells.Add(seedPoint, new VoronoiCell(seedPoint));
        }

        return Cells[seedPoint];
    }

    public List<Triangle> GenerateTriangulation()
    {
        return DelaunayTriangulation.GetTriangulation(vPoints, size.x, size.y);
    }

    public Vector2Int[] GetVPoints()
    {
        return vPoints;
    }

    public List<Edge> GetEdges()
    {
        return edges;
    }

    public Color GetColor(Vector2Int point)
    {
        int index = FindNearestVPointIndex(point);
        return vColors[index];
    }

    public Vector2? FindNearestSeedPoint(Vector2 point)
    {
        Vector2? seedPoint = null;
        float smallestDst = float.MaxValue;

        foreach (var keyPoint in Cells.Keys)
        {
            float distance = Vector2.Distance(keyPoint, point);
            if (distance < smallestDst)
            {
                smallestDst = distance;
                seedPoint = keyPoint;
            }
        }

        return seedPoint;
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
