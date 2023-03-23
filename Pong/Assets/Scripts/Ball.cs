using UnityEngine;

public class Ball : MonoBehaviour
{
    public Board board;
    public Paddle player;
    public Paddle opponent;
    public float speed;
    public float forceMod;
    public Vector2 ballStart;
    public bool usePhysics;

    private Vector2 position;
    public Vector2 direction;
    public Rigidbody2D Rigidbody { get; set; }
    private bool IsServed { get; set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SetUp();
        direction = new(0.0f, 0.0f);
    }

    private void Update()
    {
        if (!IsServed && Input.GetKeyDown(KeyCode.Space))
        {
            Restart(-1.0f);
        }

        if (IsServed && !usePhysics)
        {
            Move();
            transform.position = position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BoundsLeft")
        {
            board.UpdateScore(Side.OPPONENT);
            if (IsServed)
            {
                Restart(1.0f);
            }   
        }
        else
        {
            board.UpdateScore(Side.PLAYER);
            if (IsServed)
            {
                Restart(-1.0f);
            }
        }     
    }

    public void SetUp()
    {
        IsServed = false;
        position = ballStart;
        transform.position = ballStart;

        if (usePhysics)
        {
            Rigidbody.velocity = new Vector2(0.0f, 0.0f);
        }
    }

    private void Restart(float xDir)
    {
        SetUp();
        IsServed = true;
        float yDir = RandomChoice(new float[] { -1.0f, 1.0f }) * Random.Range(0.5f, 1.0f);
        direction = new(xDir, yDir);

        if (usePhysics)
        {  
            Rigidbody.AddForce(direction * forceMod);
        }      
    }

    private float RandomChoice(float[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    private void Move()
    {
        Vector2 previousPos = position;
        position += speed * Time.deltaTime * direction;

        if (ParseCollision(position))
        {
            position = previousPos;
            position += speed * direction * Time.deltaTime;
        }
    }

    private bool ParseCollision(Vector2 position)
    {
        bool hasCollided = false;
        Rect boardBounds = board.Bounds;
        float radius = transform.localScale.x / 2;
        float y = position.y;

        if (y - radius <= boardBounds.y ||
            y + radius >= boardBounds.y + boardBounds.size.y)
        {
            direction.y = -direction.y;
            hasCollided = true;
        }
        else if (IsCollision(position, player) ||
                 IsCollision(position, opponent))
        {
            direction.x = -direction.x;
            hasCollided = true;
        }

        return hasCollided;
    }

    private bool IsCollision(Vector2 ballPosition, Paddle paddle)
    {
        float radius = transform.localScale.x / 2;
        Vector2 paddlePos = paddle.transform.position;
        Vector2 paddleSize = paddle.transform.localScale;

        Vector2 newVector = paddlePos - ballPosition;


        if (Mathf.Abs(newVector.x) <= radius + paddleSize.x / 2 &&
            Mathf.Abs(newVector.y) <= radius + paddleSize.y / 2)
        {
            return true;
        }

        return false;
    }
}
