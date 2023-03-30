using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    private Vector2[] vertices;
    private Edge[] edges;

    public Vector2 v1
    {
        get
        {
            return vertices[0];
        }
    }

    public Vector2 v2
    {
        get
        {
            return vertices[1];
        }
    }

    public Vector2 v3
    {
        get
        {
            return vertices[2];
        }
    }

    public Triangle(Vector2 v1, Vector2 v2, Vector2 v3)
    {
        vertices = new Vector2[3];
        vertices[0] = v1;
        vertices[1] = v2;
        vertices[2] = v3;

        edges = new Edge[3];
        edges[0] = new Edge(v1, v2);
        edges[1] = new Edge(v2, v3);
        edges[2] = new Edge(v1, v3);
    }

    public Vector2[] GetVertices()
    {
        return vertices;
    }

    public Edge[] GetEdges()
    {
        return edges;
    }

    public bool Contains(Vector2 point)
    {
        return (point == v1 || point == v2 || point == v3);
    }

    public bool InCircumCircle(Vector2 point)
    {
        Vector2 center = CircumCenter();
        float radius = CircumRadius(center);
        float distance = Vector2.Distance(center, point);

        return (distance < radius);
    }

    public Vector2 CircumCenter()
    {
        Line perpLine1 = Line.PerpendicularBisector(v1, v2);
        Line perpLine2 = Line.PerpendicularBisector(v2, v3);
        return perpLine1.Intersect(perpLine2);
    }

    public float CircumRadius(Vector2 center)
    {
        return Vector2.Distance(center, v1);
    }
}

public class Edge : System.IEquatable<Edge>
{
    private Vector2[] vertices;

    public Vector2 v1
    {
        get
        {
            return vertices[0];
        }
    }

    public Vector2 v2
    {
        get
        {
            return vertices[1];
        }
    }

    public float Length
    {
        get
        {
            return Vector2.Distance(v1, v2);
        }
    }

    public Edge(Vector2 v1, Vector2 v2)
    {
        vertices = new Vector2[2];
        vertices[0] = v1;
        vertices[1] = v2;
    }

    public bool IsRightToLeft()
    {
        if (v1.x > v2.x)
        {
            return true;
        }
        return false;
    }

    public void Flip()
    {
        Vector2 temp = vertices[0];
        vertices[0] = vertices[1];
        vertices[1] = temp;
    }

    public bool Equals(Edge other)
    {
        if (other == null)
        {
            return false;
        }
        return ((v1 == other.v1 && v2 == other.v2) || (v1 == other.v2 && v2 == other.v1));
    }

}

public class Line
{
    private float a;
    private float b;
    private float c;

    public float slope
    {
        get
        {
            return (-a / b);
        }
    }

    public float bConstant
    {
        get
        {
            return (-c / b);
        }
    }

    public Line(Vector2 p1, Vector2 p2)
    {
        a = p2.y - p1.y;
        b = p1.x - p2.x;
        c = -(a * p1.x + b * p1.y);
    }

    public void Log()
    {
        Debug.Log("a: " + a + " b: " + b + " c: " + c);
        Debug.Log("y = " + slope + "x + " + bConstant);
    }

    public Vector2 Intersect(Line line)
    {
        float denominator = a * line.b - b * line.a;
        float xIntersect = b * line.c - c * line.b;
        float yIntersect = c * line.a - a * line.c;

        return new Vector2(xIntersect / denominator, yIntersect / denominator); 
    }

    public static Line PerpendicularBisector(Vector2 p1, Vector2 p2)
    {
        Line line = new Line(p1, p2);

        Vector2 midPoint = new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);

        line.c = line.b * midPoint.x - line.a * midPoint.y;

        float temp = line.a;
        line.a = - line.b;
        line.b = temp;

        return line;
    }

    public static Vector2 MidPoint(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }
}

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

    public List<Triangle> DelaunayTriangulation()
    {
        List<Triangle> triangles = new List<Triangle>();
        Vector2 origin = new Vector2(-100.0f, -100.0f);
        Vector2 maxYV = new Vector2(-100.0f, 2 * size.y + 100);
        Vector2 maxXV = new Vector2(2 * size.x + 100, -100.0f);
        triangles.Add(new Triangle(origin, maxYV, maxXV));

        foreach (Vector2Int point in vPoints)
        {
            Triangulate(triangles, point);
        }

        TestTriangulation(triangles);

        RemoveSuperTriangle(triangles, origin, maxYV, maxXV);

        return triangles;
    }

    private void Triangulate(List<Triangle> triangles, Vector2 newPoint)
    {
        List<Triangle> badTriangles = new List<Triangle>();
        List<Edge> polygon = new List<Edge>();

        FindInvalidTriangles(triangles, newPoint, badTriangles, polygon);
        GetPolygonFromBadTriangles(badTriangles, polygon);
        RemoveInvalidTriangles(triangles, badTriangles);
        FillInPolygon(triangles, newPoint, polygon);
    }

    private void FindInvalidTriangles(List<Triangle> triangles, Vector2 newPoint, List<Triangle> badTriangles, List<Edge> polygon)
    {
        foreach (Triangle triangle in triangles)
        {
            if (triangle.InCircumCircle(newPoint))
            {
                badTriangles.Add(triangle); 
            }
        }
    }

    private void GetPolygonFromBadTriangles(List<Triangle> badTriangles, List<Edge> polygon)
    {
        List<Edge> badEdges = new List<Edge>();
        foreach (var triangle in badTriangles)
        {
            foreach (var edge in triangle.GetEdges())
            {
                if (!polygon.Contains(edge))
                {
                    polygon.Add(edge);
                }
                else
                {
                    if (!badEdges.Contains(edge))
                    {
                        badEdges.Add(edge);
                    }
                }
            }
        }

        foreach (var edge in badEdges)
        {
            polygon.Remove(edge);
        }
    }

    private void RemoveInvalidTriangles(List<Triangle> triangles, List<Triangle> badTriangles)
    {
        foreach (Triangle triangle in badTriangles)
        {
            triangles.Remove(triangle);
        }
    }

    private void FillInPolygon(List<Triangle> triangles, Vector2 newPoint, List<Edge> polygon)
    {
        foreach (Edge edge in polygon)
        {
            triangles.Add(new Triangle(edge.v1, edge.v2, newPoint));
        }
    }

    private void RemoveSuperTriangle(List<Triangle> triangles, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        List<Triangle> deleteList = new List<Triangle>();
        foreach (var triangle in triangles)
        {
            if (triangle.Contains(p1) ||
                triangle.Contains(p2) ||
                triangle.Contains(p3))
            {
                deleteList.Add(triangle);
            }
        }

        foreach (var triangle in deleteList)
        {
            triangles.Remove(triangle);
        }
    }

    private void TestTriangulation(List<Triangle> triangles)
    {
        foreach (var triangle in triangles)
        {
            foreach (var point in triangle.GetVertices())
            {
                foreach (var innerTriangle in triangles)
                {
                    if (innerTriangle.InCircumCircle(point))
                    {
                        Debug.Log("Triangulation Failed!!!");
                        return;
                    }
                }
            }
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
