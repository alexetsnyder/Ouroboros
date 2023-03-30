using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ViewerController : MonoBehaviour
{
    public int regions;
    public Vector2Int imageSize;
    public DrawLine drawLine;
    public Transform circleTransform;
    public GameObject dot;

    private VoronoiDiagram voronoiDiagram;

    private SpriteRenderer spriteRenderer;

    private Triangle triangle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        voronoiDiagram = new VoronoiDiagram(regions, imageSize);
        GeneratePoints();
        //DrawDiagramWithColors();
    }

    private void Start()
    {
        List<Vector2> vertices = new List<Vector2>();
        List<Triangle> triangles = voronoiDiagram.DelaunayTriangulation();

        drawLine.ClearLines();

        foreach (Triangle triangle in triangles)
        {
            drawLine.AddLines(GetTriangleLines(triangle));
            foreach (Vector2 v in triangle.GetVertices())
            {
                if (!vertices.Contains(v))
                {
                    vertices.Add(v);
                    Instantiate(dot).transform.position = v;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestGeometry();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }
    }

    private void Select()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (triangle != null)
        {
            if (triangle.InCircumCircle(worldPos))
            {
                Debug.Log("In Circum Circle");
            }
        }
    }

    public void TestGeometry()
    {
        drawLine.ClearLines();

        Vector2 p1 = GetRandomVector2(0, 24);
        Vector2 p2 = GetRandomVector2(0, 24);
        Vector2 p3 = GetRandomVector2(0, 24);

        triangle = new Triangle(p1, p2, p3);

        Vector2[] triangleLines = GetTriangleLines(triangle);

        drawLine.AddLines(triangleLines);

        Line perpLine1 = Line.PerpendicularBisector(triangle.v1, triangle.v2);
        Line perpLine2 = Line.PerpendicularBisector(triangle.v2, triangle.v3);

        Vector2 center = perpLine1.Intersect(perpLine2);
        Vector2 line1Mid = Line.MidPoint(triangle.v1, triangle.v2);
        Vector2 line2Mid = Line.MidPoint(triangle.v2, triangle.v3);

        drawLine.AddLines(new Vector2[] { line1Mid, center, line2Mid, center });

        float radius = triangle.CircumRadius(center);

        circleTransform.position = center;
        circleTransform.localScale = new Vector2(2 * radius, 2 * radius);

        Debug.Log("Center: (" + center.x + ", " + center.y + ")");
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
