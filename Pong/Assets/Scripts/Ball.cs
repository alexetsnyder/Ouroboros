using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector2 speed;
    private BoxCollider2D Collider { get; set; }
    private Rigidbody2D Rigidbody { get; set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void Start()
    {
        Vector2 newVelocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Rigidbody.AddForce(newVelocity * speed, ForceMode2D.Impulse);
        //Rigidbody.velocity = newVelocity * speed;
    }
}
