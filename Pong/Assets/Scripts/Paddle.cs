using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Vector2 velocity;
    public float speed;
    public Board board;
    public Ball ball;
    public bool isAIOn;
    public bool usePhysics;
    private Vector2 Position { get; set; }
    private string Name { get; set; }

    private void Awake()
    {
        Position = transform.position;
        Name = gameObject.name;
    }

    private void Update()
    {
        ProcessKeyInput();

        transform.position = Position;
    }

    private void ProcessKeyInput()
    {
        if (Name == "Player")
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
        else if (isAIOn && Name == "Opponent")
        {
            MoveTowardsBall();
        }
    }

    private void Move(Vector2 direction)
    {
        Rect boardBounds = board.Bounds;
        float ySize = transform.localScale.y;
        Vector2 previousPos = Position;

        Position += velocity * Time.deltaTime * direction;     
        if (Position.y - ySize / 2 < boardBounds.y ||
            Position.y + ySize / 2 > boardBounds.y + boardBounds.size.y)
        {
            Position = previousPos;
        }
    }

    private void MoveTowardsBall()
    {
        float xDir = (ball.usePhysics) ? ball.Rigidbody.velocity.x : ball.direction.x;
        float yDir = 0.0f;

        if (xDir > 0)
        {
            float ballY = ball.transform.position.y;
            yDir = ballY - transform.position.y;
            yDir = (yDir < 0.0f) ? -1.0f : (yDir > 0.0f) ? 1.0f : 0.0f;       
        }
        else if (xDir < 0)
        {
            yDir = 0.0f - transform.position.y;
            yDir = (yDir < 0.0f) ? -1.0f : (yDir > 0.0f) ? 1.0f : 0.0f;
        }

        Move(new Vector2(0.0f, yDir));
    }

    public void Return()
    {
        Vector2 center = new(Position.x, 0.0f);
        Position = center;
        transform.position = center;
    }
}
