using UnityEngine;

public class Ball : MonoBehaviour
{
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
            Restart();
            IsServed = true;
        }

        if (debug && Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Restart();
    }

    private void Restart()
    {
        transform.position = ballStart;
        Rigidbody.velocity = new Vector2(0.0f, 0.0f);
        Vector2 direction = new(-1.0f, Random.Range(-1.0f, 1.0f));
        Rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
