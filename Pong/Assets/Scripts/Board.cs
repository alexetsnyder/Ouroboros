using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject dot;

    public Rect Bounds
    {
        get
        {
            Vector2 position = transform.position;
            Vector2 size = transform.localScale;
            return new Rect(new Vector2(position.x - size.x / 2, position.y - size.y / 2), size); 
        }
    }

    private void Start()
    {
        CreateDottedLine();
    }

    private void CreateDottedLine()
    {
        Rect bounds = Bounds;
        float seperation = 0.5f;
        float dotSize = dot.transform.localScale.y;

        int number = Mathf.FloorToInt(bounds.size.y / (seperation + dotSize));
        Vector2 position = new(0.0f, bounds.y + seperation);

        for (int i = 0; i <= number; i++)
        {
            Instantiate(dot, position, Quaternion.identity);
            position.y += seperation + dotSize;
        }
    }
}
