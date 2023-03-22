using UnityEngine;

public class Board : MonoBehaviour
{
    public Rect Bounds
    {
        get
        {
            Vector2 position = transform.position;
            Vector2 size = transform.localScale;
            return new Rect(new Vector2(position.x - size.x / 2, position.y - size.y / 2), size); 
        }
    }
}
