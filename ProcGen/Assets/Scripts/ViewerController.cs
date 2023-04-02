using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
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
    private SpriteRenderer spriteRenderer;

    private VoronoiDiagram voronoiDiagram;

    private Triangle triangle;

    private int index;
    private List<Vector2Int> addedPoints;
    private List<Triangle> superTriangleList;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        voronoiDiagram = new VoronoiDiagram(regions, imageSize);
        drawList = new List<DrawLine>();
        GeneratePoints();

        index = 0;
        superTriangleList = new List<Triangle>();
        dotVertexList = new List<GameObject>();
        addedPoints = new List<Vector2Int>();

        circleTransforms = new List<Transform>();
    }

    private void Start()
    {
        Vector2 v1 = new Vector2(-50.0f, -50.0f);
        Vector2 v2 = new Vector2(-50.0f, 2 * 24 + 100);
        Vector2 v3 = new Vector2(2 * 24 + 100, -50.0f);
        superTriangleList.Add(new Triangle(v1, v2, v3));

        transform.position = new Vector2(12.0f, 12.0f);
        transform.localScale = new Vector2(24.0f, 24.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            IncrementDelaunayTriangulation();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateDelaunayTriangulation();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            VisualizeCircumscribedCircleOfTriangle();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            GenerateVoronoiDiagram();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ClearAll();
            DrawDiagramWithColors();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            VisualizeVoronoiEdge();
        }
    }

    public void GenerateVoronoiDiagram()
    {
        voronoiDiagram.GenerateDiagram();

        DrawVoronoiDiagram(voronoiDiagram.GetVPoints(), voronoiDiagram.GetEdges());
    }

    public void DrawVoronoiDiagram(Vector2Int[] vPoints, List<Edge> edges)
    {
        ClearAll();

        foreach (var point in vPoints)
        {
            GameObject dotPrefab = Instantiate(dot);
            dotPrefab.transform.position = new Vector3(point.x, point.y, 0.0f);
            dotVertexList.Add(dotPrefab);
        }

        foreach (var edge in edges)
        {
            DrawEdge(edge);
        }
    }

    public void IncrementDelaunayTriangulation()
    {
        Vector2Int[] vPoints = voronoiDiagram.GetVPoints();
        if (index < vPoints.Length)
        {
            Vector2Int nextPoint = vPoints[index];
            addedPoints.Add(nextPoint);
            DelaunayTriangulation.Triangulate(superTriangleList, nextPoint);
            DelaunayTriangulation.TestTriangulation(superTriangleList, addedPoints.ToArray());
            index++;
        }

        DrawDelaunayTriangulation(superTriangleList);

        if (index >= vPoints.Length)
        {
            Debug.Log("Delaunay Triangulation is Complete!");  
        }
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
                    GameObject dotPrefab = Instantiate(dot);
                    dotPrefab.transform.position = v;
                    dotVertexList.Add(dotPrefab);
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

        DelaunayTriangulation.TestTriangulation(new List<Triangle>() { t1, t2 }, new Vector2Int[] { v1, v2, v3, v4 });

        Edge voronoiEdge = new Edge(t1.CircumCenter, t2.CircumCenter);

        DrawTriangle(t1);
        DrawTriangle(t2);

        DrawCircumCircle(t1);
        DrawCircumCircle(t2);

        DrawEdge(voronoiEdge);
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

    private void DrawEdge(Edge edge)
    {
        DrawLine drawLine = Instantiate(line).GetComponent<DrawLine>();
        drawLine.ClearLines();
        drawLine.AddLines(new Vector2[] { edge.v1, edge.v2 });
        drawList.Add(drawLine);
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
        Vector2 line1Mid = Line.MidPoint(triangle.v1, triangle.v2);
        Vector2 line2Mid = Line.MidPoint(triangle.v2, triangle.v3);

        DrawTriangle(triangle);

        DrawEdge(new Edge(line1Mid, center));
        DrawEdge(new Edge(line2Mid, center));

        DrawCircumCircle(triangle);     
    }

    public void ClearAll()
    {
        triangle = null;
        
        DestroyDots();
        DestroyLines();
        DestroyCircles();
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

    public void DrawPoints()
    {
        Color[] pixels = CreatePointPixels();

        spriteRenderer.sprite = Sprite.Create(CreateTexture(pixels), new Rect(0.0f, 0.0f, imageSize.x, imageSize.y), Vector2.one * 0.5f);
    }

    private Color[] CreatePointPixels()
    {
        Color[] pixels = new Color[imageSize.x * imageSize.y];

        foreach (Vector2Int point in voronoiDiagram.GetVPoints())
        {
            int index = point.x * imageSize.x + point.y;
            pixels[index] = Color.black;
        }

        return pixels;
    }

    public void DrawDiagramWithColors()
    {
        Color[] pixels = CreateDiagramPixels();

        spriteRenderer.sprite = Sprite.Create(CreateTexture(pixels), new Rect(0.0f, 0.0f, imageSize.x, imageSize.y), Vector2.one * 0.5f);
    }

    private Color[] CreateDiagramPixels()
    {
        Color[] pixels = new Color[imageSize.x * imageSize.y];

        for (int x = 0; x < imageSize.x; x++)
        {
            for (int y = 0; y < imageSize.y; y++)
            {
                int index = x * imageSize.x + y;
                pixels[index] = voronoiDiagram.GetColor(new Vector2Int(x, y));
            }
        }

        return pixels;
    }

    private Texture2D CreateTexture(Color[] pixels)
    {
        Texture2D texture = new Texture2D(imageSize.x, imageSize.y);

        texture.filterMode = FilterMode.Point;
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

}
