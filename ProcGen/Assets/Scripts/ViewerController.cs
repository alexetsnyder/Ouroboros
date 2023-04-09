using System.Collections.Generic;
using UnityEngine;

public class ViewerController : MonoBehaviour
{
    public int regions;
    public Vector2Int imageSize;
    public GameObject dot;
    public GameObject line;
    public GameObject circle;

    private List<DrawLine> drawList;
    private List<GameObject> dotVertexList;
    private List<Transform> circleTransforms;

    private VoronoiDiagram voronoiDiagram;
    private bool isVoronoiDisplayed;
    private List<DrawLine> voronoiCellDrawList;
    private GameObject centroidDot;

    private Triangle triangle;

    private int index;
    private List<Vector2> addedPoints;
    private List<Triangle> superTriangleList;

    private void Awake()
    {
        voronoiDiagram = new VoronoiDiagram(regions, imageSize);
        drawList = new List<DrawLine>();    
        voronoiCellDrawList = new List<DrawLine>();
        superTriangleList = new List<Triangle>();
        dotVertexList = new List<GameObject>();
        addedPoints = new List<Vector2>();
        circleTransforms = new List<Transform>();

        ResetAll();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }
    }

    public void ResetAll()
    {
        ClearAll();

        index = 0;
        addedPoints.Clear();
        superTriangleList.Clear();

        Vector2 v1 = new Vector2(-50.0f, -50.0f);
        Vector2 v2 = new Vector2(-50.0f, 2 * 24 + 100);
        Vector2 v3 = new Vector2(2 * 24 + 100, -50.0f);
        superTriangleList.Add(new Triangle(v1, v2, v3));

        GeneratePoints();
    }

    public void RelaxVoronoiDiagram()
    {
        if (isVoronoiDisplayed)
        {
            voronoiDiagram.Relax();
            DrawVoronoiDiagram(voronoiDiagram.GetVPoints(), voronoiDiagram.GetEdges());
        }
    }

    public void GenerateVoronoiDiagram()
    {
        voronoiDiagram.GenerateDiagram();

        DrawVoronoiDiagram(voronoiDiagram.GetVPoints(), voronoiDiagram.GetEdges());
    }

    public void DrawVoronoiDiagram(Vector2[] vPoints, List<Edge> edges)
    {
        ClearAll();

        foreach (var point in vPoints)
        {
            DrawDot(point);
        }

        foreach (var edge in edges)
        {
            DrawEdge(edge);
        }

        isVoronoiDisplayed = true;
    }

    public void IncrementDelaunayTriangulation()
    {
        Vector2[] vPoints = voronoiDiagram.GetVPoints();
        if (index < vPoints.Length)
        {
            Vector2 nextPoint = vPoints[index];
            addedPoints.Add(nextPoint);
            DelaunayTriangulation.Triangulate(superTriangleList, nextPoint);
            DelaunayTriangulation.TestTriangulation(superTriangleList, addedPoints.ToArray());
            index++;
        }

        if (index >= vPoints.Length)
        {
            Debug.Log("Delaunay Triangulation is Complete!");
        }

        DrawDelaunayTriangulation(superTriangleList);  
    }

    public void GenerateDelaunayTriangulation()
    {
        List<Triangle> triangles = voronoiDiagram.GenerateTriangulation();

        Vector2 v1 = new Vector2(-50.0f, -50.0f);
        Vector2 v2 = new Vector2(-50.0f, 2 * 24 + 100);
        Vector2 v3 = new Vector2(2 * 24 + 100, -50.0f);

        DelaunayTriangulation.RemoveSuperTriangle(triangles, v1, v2, v3);

        DrawDelaunayTriangulation(triangles);
    }

    public void DrawDelaunayTriangulation(List<Triangle> triangles)
    {
        ClearAll();

        List<Vector2> vertices = new List<Vector2>();
        foreach (Triangle triangle in triangles)
        {
            DrawTriangle(triangle);

            foreach (Vector2 v in triangle.GetVertices())
            {
                if (!vertices.Contains(v))
                {
                    vertices.Add(v);
                    DrawDot(v);
                }
            }
        }
    }

    private void DestroyDots()
    {
        foreach (var dot in dotVertexList)
        {
            Destroy(dot);
        }

        dotVertexList.Clear();
    }

    private void DestroyLines()
    {
        foreach (var line in drawList)
        {
            Destroy(line.gameObject);
        }

        drawList.Clear();
    }

    private void DestroyCircles()
    {
        foreach (var circle in circleTransforms)
        {
            Destroy(circle.gameObject);
        }

        circleTransforms.Clear();
    }

    private void DestroyVoronoiCell()
    {
        foreach (var line in voronoiCellDrawList)
        {
            Destroy(line.gameObject);
        }

        voronoiCellDrawList.Clear();
    }

    private void Select()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (triangle != null)
        {
            if (triangle.IsPointInsideCircumscribedCircle(worldPos))
            {
                Debug.Log("In Circum Circle");
            }
        }
        else if (isVoronoiDisplayed)
        {
            Vector2? seedPoint = voronoiDiagram.FindNearestSeedPoint(worldPos);
            if (seedPoint.HasValue)
            {
                VoronoiCell cell = voronoiDiagram.Cells[seedPoint.Value];
                DrawVoronoiCell(cell, Color.red);
                DrawCentroid(voronoiDiagram.GetCentroid(cell));
            }
        }
    }

    public void DrawVoronoiCell(VoronoiCell cell, Color color)
    {
        DestroyVoronoiCell();
     
        foreach (var edge in cell.Edges)
        {
            DrawVoronoiEdge(edge, color);
        }        
    }

    public void DrawVoronoiEdge(Edge edge, Color color)
    {
        DrawLine drawLine = Instantiate(line).GetComponent<DrawLine>();

        drawLine.ClearLines();
        drawLine.AddLines(new Vector2[] { edge.v1, edge.v2 });
        drawLine.SetColor(color);

        voronoiCellDrawList.Add(drawLine);
    }

    public void DrawCentroid(Vector2 centroid)
    {
        if (centroidDot == null)
        {
            centroidDot = Instantiate(dot);
            centroidDot.GetComponent<SpriteRenderer>().color = Color.red;
        }
        centroidDot.transform.position = centroid;
    }

    public void VisualizeVoronoiEdge()
    {
        ClearAll();

        Vector2Int v1 = new Vector2Int(0, 10);
        Vector2Int v2 = new Vector2Int(10, 0);
        Vector2Int v3 = new Vector2Int(0, 0);
        Vector2Int v4 = new Vector2Int(-10, 0);

        Vector2Int translate = new Vector2Int(12, 6);
        v1 += translate;
        v2 += translate;
        v3 += translate;
        v4 += translate;

        Triangle t1 = new Triangle(v1, v2, v3);
        Triangle t2 = new Triangle(v3, v4, v1);

        DelaunayTriangulation.TestTriangulation(new List<Triangle>() { t1, t2 }, new Vector2[] { v1, v2, v3, v4 });

        Edge voronoiEdge = new Edge(t1.CircumCenter, t2.CircumCenter);

        DrawTriangle(t1);
        DrawTriangle(t2);

        DrawCircumCircle(t1);
        DrawCircumCircle(t2);

        DrawEdge(voronoiEdge, Color.red);
    }

    private void DrawTriangle(Triangle triangle)
    {
        DrawLine drawLine = Instantiate(line).GetComponent<DrawLine>();
        drawLine.ClearLines();
        drawLine.AddLines(GetTriangleLines(triangle));
        drawList.Add(drawLine);
    }

    private void DrawCircumCircle(Triangle triangle)
    {
        Transform circleTransform = Instantiate(circle).GetComponent<Transform>();
        circleTransform.position = triangle.CircumCenter;
        circleTransform.localScale = new Vector2(2 * triangle.CircumRadius, 2 * triangle.CircumRadius);
        circleTransforms.Add(circleTransform);
    }

    private void DrawEdge(Edge edge, Color? color = null)
    {
        DrawLine drawLine = Instantiate(line).GetComponent<DrawLine>();

        if (color.HasValue)
        {
            drawLine.SetColor(color.Value);
        }

        drawLine.ClearLines();
        drawLine.AddLines(new Vector2[] { edge.v1, edge.v2 });
        drawList.Add(drawLine);
    }

    private void DrawDot(Vector2 point)
    {
        GameObject dotPrefab = Instantiate(dot);
        dotPrefab.transform.position = point;
        dotVertexList.Add(dotPrefab);
    }

    public void VisualizeCircumscribedCircleOfTriangle()
    {
        ClearAll();

        Vector2 p1 = new Vector2(-6.0f, 6.0f);
        Vector2 p2 = new Vector2(6.0f, -6.0f);
        Vector2 p3 = new Vector2(-6.0f, -6.0f);

        Vector2 translate = Vector2.one * 12.0f;
        p1 += translate;
        p2 += translate;
        p3 += translate;

        triangle = new Triangle(p1, p2, p3);

        Line perpLine1 = Line.PerpendicularBisector(triangle.v1, triangle.v2);
        Line perpLine2 = Line.PerpendicularBisector(triangle.v2, triangle.v3);

        Vector2 center = perpLine1.Intersect(perpLine2);
        Vector2 line2Mid = Line.MidPoint(triangle.v2, triangle.v3);

        Vector2? line1Pt = perpLine1.Y(center.x - 2);
        Vector2? line1Pt2 = perpLine1.Y(center.x + 2);
        Vector2? line2Pt = perpLine2.X(line2Mid.y - 3);

        DrawTriangle(triangle);

        DrawEdge(new Edge(line1Pt.Value, line1Pt2.Value), Color.red);
        DrawEdge(new Edge(line2Pt.Value, center), Color.blue);

        DrawCircumCircle(triangle);
        DrawDot(center);
    }

    public void ClearAll()
    {
        isVoronoiDisplayed = false;
        triangle = null;

        Destroy(centroidDot);
        DestroyDots();
        DestroyLines();
        DestroyCircles();
        DestroyVoronoiCell();
    }

    private Vector2[] GetTriangleLines(Triangle triangle)
    {
        Vector2[] triangleLines = new Vector2[4];
        Vector2[] triangleVertices = triangle.GetVertices();

        for (int i = 0; i < triangleVertices.Length; i++)
        {
            triangleLines[i] = triangleVertices[i];
        }
        triangleLines[3] = triangleVertices[0];

        return triangleLines;
    }

    public void GeneratePoints()
    {
        voronoiDiagram.GeneratePoints();
    }
}
