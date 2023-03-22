using UnityEngine;

public class Ball : MonoBehaviour
{
    public Board board;
    public Vector2 speed;
    public Vector2 ballStart;
    public bool debug;
    private Rigidbody2D Rigidbody { get; set; }
    public bool IsServed { get; set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        IsServed = false;
    }

    private void Update()
    {
        if (!IsServed)
        {
            Restart(-1.0f);
            IsServed = true;
        }

        if (debug && Input.GetKeyDown(KeyCode.Space))
        {
            Restart(-1.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BoundsLeft")
        {
            board.UpdateScore(Side.OPPONENT);
            Restart(1.0f);
        }
        else
        {
            board.UpdateScore(Side.PLAYER);
            Restart(-1.0f);
        }     
    }

    private void Restart(float xDir)
    {
        transform.position = ballStart;
        Rigidbody.velocity = new Vector2(0.0f, 0.0f);
        Vector2 direction = new(-1.0f, Random.Range(-0.5f, 0.5f));
        Rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
