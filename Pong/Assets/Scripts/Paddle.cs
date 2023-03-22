using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Vector2 velocity;
    public Board board;
    private Vector2 Position { get; set; }
    private string Name { get; set; }
    private Rigidbody2D Rigidbody { get; set; }

    private void Awake()
    {
        Position = transform.position;
        Name = gameObject.name;
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ProcessKeyInput();

        transform.position = Position;
    }

    private void ProcessKeyInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(Vector2.up);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            Move(Vector2.down);
        }
    }

    private void Move(Vector2 direction)
    {
        Position += velocity * Time.deltaTime * direction;
        //Rigidbody.AddForce(velocity * direction, ForceMode2D.Impulse);
    }

    
}
