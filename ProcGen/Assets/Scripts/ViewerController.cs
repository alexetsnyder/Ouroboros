using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ViewerController : MonoBehaviour
{
    public int regions;
    public Vector2Int imageSize;
    public List<DrawLine> drawList;
    public DrawLine triangleViewer;
    public GameObject dot;
    public GameObject line;
    public GameObject circle;

    public Transform circleTransform;

    private VoronoiDiagram voronoiDiagram;

    private SpriteRenderer spriteRenderer;

    private Triangle triangle;
    private int index;
    private List<Vector2Int> addedPoints;
    private List<Triangle> superTriangleList;
    private List<GameObject> dotVertexList;

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
    }

    private void Start()
    {
        Vector2 v1 = new Vector2(-50.0f, -50.0f);
        Vector2 v2 = new Vector2(-50.0f, 2 * 24 + 100);
        Vector2 v3 = new Vector2(2 * 24 + 100, -50.0f);
        superTriangleList.Add(new Triangle(v1, v2, v3));

        transform.position = new Vector2(12.0f, 12.0f);
        transform.localScale = new Vector2(24.0f, 24.0f);

        Vector2 origin = new Vector2(0.0f, 0.0f);
        Vector2 xPoint = new Vector2(1.0f, 0.0f);
        Vector2 yPoint = new Vector2(0.0f, 1.0f);
        Line verticalLine = Line.Vertical(0.0f);
        Line horizontalLine = Line.Horizontal(0.0f);
        Line intersectLine = new Line(xPoint, yPoint);

        verticalLine.Log();
        horizontalLine.Log();
        intersectLine.Log();

        Debug.Log(intersectLine.Intersect(verticalLine));
        Debug.Log(intersectLine.Intersect(horizontalLine));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            superTriangleList.Clear();
            index = 0;
            addedPoints.Clear();

            Vector2 v1 = new Vector2(-50.0f, -50.0f);
            Vector2 v2 = new Vector2(-50.0f, 2 * 24 + 100);
            Vector2 v3 = new Vector2(2 * 24 + 100, -50.0f);
            superTriangleList.Add(new Triangle(v1, v2, v3));

            ClearAll();
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
            TestGeometry();
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
            DrawLine drawLine = Instantiate(line).GetComponent<DrawLine>();
            drawLine.ClearLines();
            drawLine.AddLines(new Vector2[] { edge.v1, edge.v2 });
            drawList.Add(drawLine);
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
            DrawLine drawLine = Instantiate(line).GetComponent<DrawLine>();
            drawLine.ClearLines();
            drawLine.AddLines(GetTriangleLines(triangle));
            drawList.Add(drawLine);

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

    public void TestGeometry()
    {
        ClearAll();

        if (triangleViewer == null)
        {
            triangleViewer = Instantiate(line).GetComponent<DrawLine>();
        }
        triangleViewer.ClearLines();

        Vector2 p1 = GetRandomVector2(0, 24);
        Vector2 p2 = GetRandomVector2(0, 24);
        Vector2 p3 = GetRandomVector2(0, 24);

        triangle = new Triangle(p1, p2, p3);

        Vector2[] triangleLines = GetTriangleLines(triangle);

        triangleViewer.AddLines(triangleLines);

        Line perpLine1 = Line.PerpendicularBisector(triangle.v1, triangle.v2);
        Line perpLine2 = Line.PerpendicularBisector(triangle.v2, triangle.v3);

        Vector2 center = perpLine1.Intersect(perpLine2);
        Vector2 line1Mid = Line.MidPoint(triangle.v1, triangle.v2);
        Vector2 line2Mid = Line.MidPoint(triangle.v2, triangle.v3);

        triangleViewer.AddLines(new Vector2[] { line1Mid, center, line2Mid, center });

        float radius = triangle.CircumRadius;

        if (circleTransform == null)
        {
            circleTransform = Instantiate(circle).GetComponent<Transform>();
        }

        circleTransform.position = center;
        circleTransform.localScale = new Vector2(2 * radius, 2 * radius);

        Debug.Log("v1: " + triangle.v1 + " v2: " + triangle.v2 + " v3: " + triangle.v3);
        Debug.Log("Center: (" + center.x + ", " + center.y + ")");
    }

    public void ClearAll()
    {
        DestroyDots();
        DestroyLines();

        if (triangleViewer != null)
        {
            Destroy(triangleViewer.gameObject);
        }

        if (circleTransform != null)
        {
            Destroy(circleTransform.gameObject);
        }
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

    public Vector2 GetRandomVector2(int min, int max)
    {
        return new Vector2(Random.Range(min, max), Random.Range(min, max));
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
