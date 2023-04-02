using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Triangle
{
    public Vector2 CircumCenter { get; private set; }
    public float CircumRadius { get; private set; }

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

    public Edge e1
    {
        get
        {
            return edges[0];
        }
    }

    public Edge e2
    {
        get
        {
            return edges[1];
        }
    }

    public Edge e3
    {
        get
        {
            return edges[2];
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
        edges[2] = new Edge(v3, v1);

        CircumCenter = GetCircumscribedCircleCenter();
        CircumRadius = GetCircumscribedCircleRadius();
    }

    public Vector2[] GetVertices()
    {
        return vertices;
    }

    public Edge[] GetEdges()
    {
        return edges;
    }

    public bool Contains(Edge edge)
    {
        return (edge.Equals(e1) || edge.Equals(e2) || edge.Equals(e3));
    }

    public bool Contains(Vector2 point)
    {
        return (point == v1 || point == v2 || point == v3);
    }

    public bool IsPointInsideCircumscribedCircle(Vector2 point)
    {
        ;
        float distance = Vector2.Distance(CircumCenter, point);

        return (distance < CircumRadius);
    }

    public Vector2 GetCircumscribedCircleCenter()
    {
        Line perpLine1 = Line.PerpendicularBisector(v1, v2);
        Line perpLine2 = Line.PerpendicularBisector(v2, v3);
        return perpLine1.Intersect(perpLine2);
    }

    public float GetCircumscribedCircleRadius()
    {
        return Vector2.Distance(CircumCenter, v1);
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

        private set
        {
            vertices[0] = value;
        }
    }

    public Vector2 v2
    {
        get
        {
            return vertices[1];
        }

        private set
        {
            vertices[1] = value;
        }
    }

    public Edge(Vector2 v1, Vector2 v2)
    {
        vertices = new Vector2[2];
        vertices[0] = v1;
        vertices[1] = v2;
    }

    public Edge Clip(RectInt clipRect)
    {
        if (!v1.IsInBounds(clipRect) && !v2.IsInBounds(clipRect))
        {
            return null;
        }

        Edge returnEdge = new Edge(v1, v2);

        if (v1.IsInBounds(clipRect) && !v2.IsInBounds(clipRect))
        {
            Vector2? clipPoint = ClipLine(clipRect, v1, v2);
            if (clipPoint.HasValue)
            {
                returnEdge.v2 = clipPoint.Value;
            }
        }

        if (!v1.IsInBounds(clipRect) && v2.IsInBounds(clipRect))
        {
            Vector2? clipPoint = ClipLine(clipRect, v2, v1);
            if (clipPoint.HasValue)
            {
                returnEdge.v1 = clipPoint.Value;
            }
        }

        return returnEdge;
    }

    private Vector2? ClipLine(RectInt clipRect, Vector2 vIn, Vector2 vOut)
    {
        Line line = new Line(vIn, vOut);

        //Below
        if (vOut.y < clipRect.yMin)
        {
            Line yMinBoundLine = Line.Horizontal(clipRect.yMin);
            Vector2 intersectPoint = line.Intersect(yMinBoundLine);
            if (intersectPoint.IsInBounds(clipRect))
            {
                return intersectPoint;
            }
        }

        //Above
        if (vOut.y > clipRect.yMax)
        {
            Line yMaxBoundLine = Line.Horizontal(clipRect.yMax);
            Vector2 intersectPoint = line.Intersect(yMaxBoundLine);
            if (intersectPoint.IsInBounds(clipRect))
            {
                return intersectPoint;
            }
        }

        //Left
        if (vOut.x < clipRect.xMin)
        {
            Line xMinBoundLine = Line.Vertical(clipRect.xMin);
            Vector2 intersectPoint = line.Intersect(xMinBoundLine);
            if (intersectPoint.IsInBounds(clipRect))
            {
                return intersectPoint;
            }
        }

        //Right
        if (vOut.x > clipRect.xMax)
        {
            Line xMaxBoundLine = Line.Vertical(clipRect.xMax);
            Vector2 intersectPoint = line.Intersect(xMaxBoundLine);
            if (intersectPoint.IsInBounds(clipRect))
            {
                return intersectPoint;
            }
        }

        return null;
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

    public static Line Vertical(float x)
    {
        Vector2 p1 = new Vector2(x, 0);
        Vector2 p2 = new Vector2(x, 1);
        return new Line(p1, p2);
    }

    public static Line Horizontal(float y)
    {
        Vector2 p1 = new Vector2(0, y);
        Vector2 p2 = new Vector2(1, y);
        return new Line(p1, p2);
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
        line.a = -line.b;
        line.b = temp;

        return line;
    }

    public static Vector2 MidPoint(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }
}

public class DelaunayTriangulation
{
    public static List<Triangle> GetTriangulation(Vector2Int[] vPoints, int xMax, int yMax)
    {
        List<Triangle> triangles = new List<Triangle>();
        Vector2 v1 = new Vector2(-50.0f, -50.0f);
        Vector2 v2 = new Vector2(-50.0f, 2 * yMax + 100);
        Vector2 v3 = new Vector2(2 * xMax + 100, -50.0f);
        triangles.Add(new Triangle(v1, v2, v3));

        foreach (Vector2Int point in vPoints)
        {
            Triangulate(triangles, point);
        }

        return triangles;
    }

    public static void Triangulate(List<Triangle> triangles, Vector2 newPoint)
    {
        List<Triangle> badTriangles = new List<Triangle>();
        List<Edge> polygon = new List<Edge>();

        FindBadTriangles(triangles, newPoint, badTriangles);
        GetPolygonFromBadTriangles(badTriangles, polygon);
        RemoveBadTriangles(triangles, badTriangles);
        FillInPolygon(triangles, newPoint, polygon);
    }

    private static void FindBadTriangles(List<Triangle> triangles, Vector2 newPoint, List<Triangle> badTriangles)
    {
        foreach (Triangle triangle in triangles)
        {
            if (triangle.IsPointInsideCircumscribedCircle(newPoint))
            {
                badTriangles.Add(triangle);
            }
        }
    }

    private static void GetPolygonFromBadTriangles(List<Triangle> badTriangles, List<Edge> polygon)
    {
        List<Edge> sharedEdges = new List<Edge>();
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
                    if (!sharedEdges.Contains(edge))
                    {
                        sharedEdges.Add(edge);
                    }
                }
            }
        }

        foreach (var edge in sharedEdges)
        {
            polygon.Remove(edge);
        }
    }

    private static void RemoveBadTriangles(List<Triangle> triangles, List<Triangle> badTriangles)
    {
        foreach (Triangle triangle in badTriangles)
        {
            triangles.Remove(triangle);
        }
    }

    private static void FillInPolygon(List<Triangle> triangles, Vector2 newPoint, List<Edge> polygon)
    {
        foreach (Edge edge in polygon)
        {
            if (edge.v1 != newPoint && edge.v2 != newPoint)
            {
                triangles.Add(new Triangle(newPoint, edge.v1, edge.v2));
            }
        }
    }

    public static void RemoveSuperTriangle(List<Triangle> triangles, Vector2 p1, Vector2 p2, Vector2 p3)
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

    public static void TestTriangulation(List<Triangle> triangles, Vector2Int[] points)
    {
        foreach (var point in points)
        {
            foreach (var triangle in triangles)
            {
                if (!triangle.Contains(point) && triangle.IsPointInsideCircumscribedCircle(point))
                {
                    Debug.Log("Triangulation Failed!!!");
                    return;
                }
            }
        }
    }

}
