using UnityEngine;

public class Paddle : MonoBehaviour
{
    public GameObject paddle;
    public Vector2 Velocity;
    private Vector2 Position;

    private void Awake()
    {
        Position = paddle.transform.position;
    }

    private void Update()
    {
        ProcessKeyInput();

        paddle.transform.position = Position;
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
        Position += Velocity * Time.deltaTime * direction;
    }
}
