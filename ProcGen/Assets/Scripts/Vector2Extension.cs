using UnityEngine;

public static class Vector2Extension
{
    public static bool IsInBounds(this Vector2 v, RectInt clipRect)
    {
        if (v.x < clipRect.xMin || v.x > clipRect.xMax ||
            v.y < clipRect.yMin || v.y > clipRect.yMax)
        {
            return false;
        }

        return true;
    }
}
