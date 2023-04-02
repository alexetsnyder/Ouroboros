using UnityEngine;

public static class Vector2Extension
{
    public static bool IsInBounds(this Vector2 v, RectInt clipRect)
    {
        float perc = 0.001f;
        if (v.x < clipRect.xMin || v.x - perc > clipRect.xMax ||
            v.y < clipRect.yMin || v.y - perc > clipRect.yMax)
        {
            return false;
        }

        return true;
    }
}
