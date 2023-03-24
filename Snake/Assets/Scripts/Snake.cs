using UnityEngine;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    private Tilemap TileMap { get; set; }

    private void Awake()
    {
        TileMap = GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {
        SpawnSnake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {

        }

        SnakeMove();
    }

    private void SpawnSnake()
    {

    }

    private void SnakeMove()
    {

    }
}
