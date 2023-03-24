using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed;
    public Board board;
    public Ball ball;
    public bool isAIOn;
    public bool usePhysics;

    private Vector2 direction;
    private Rigidbody2D rigidBody;
    private Vector2 Position { get; set; }
    private string Name { get; set; }
   

    private void Awake()
    {
        Position = transform.position;
        Name = gameObject.name;
        direction = Vector2.zero;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ProcessKeyInput();    
    }

    private void FixedUpdate()
    {
        if (usePhysics)
        {
            ApplyForce();
        }
        else
        {
            Move();
            transform.position = Position;
        }
    }

    private void ProcessKeyInput()
    {
        if (Name == "Player")
        {
            if (Input.GetKey(KeyCode.W))
            {
                direction = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction = Vector2.down;
            }
            else
            {
                direction = Vector2.zero;
            }
        }   
        else if (isAIOn && Name == "Opponent")
        {
            direction = GetBallDirection();
        }
    }

    private void Move()
    {
        Vector2 previousPos = Position;

        Position += speed * Time.deltaTime * direction;     
        if (IsCollision())
        {
            Position = previousPos;
        }
    }

    private bool IsCollision()
    {
        Rect boardBounds = board.Bounds;
        float ySize = transform.localScale.y;

        if (Position.y - ySize / 2 < boardBounds.y ||
            Position.y + ySize / 2 > boardBounds.y + boardBounds.size.y)
        {
            return true;
        }

        return false;
    }

    private void ApplyForce()
    {
        if (direction != Vector2.zero)
        {
            rigidBody.AddForce(speed * direction);
        }      
    }

    private Vector2 GetBallDirection()
    {
        float xVel = (ball.usePhysics) ? ball.Rigidbody.velocity.x : ball.direction.x;
        float yDir = 0.0f;

        if (xVel > 0)
        {
            yDir = ball.transform.position.y - transform.position.y;
        }
        else if (xVel < 0)
        {
            yDir = 0.0f - transform.position.y;
        }

        return (yDir <= -1.0f) ? Vector2.down : (yDir >= 1.0f) ? Vector2.up : Vector2.zero;
    }

    public void Return()
    {
        Vector2 center = new(Position.x, 0.0f);
        Position = center;
        transform.position = center;

        if (usePhysics)
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}
