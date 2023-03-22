using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Vector2 velocity;
    public Board board;
    public Ball ball;
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
        else
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
        float ballY = ball.transform.position.y;
        float yDir = (ballY - transform.position.y < 0) ? -1 : 1;

        Vector2 direction = new(0.0f, yDir);
        Move(direction);
    }
}
