using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    public Vector2Int spawnOffset;
    public float timeDelay;
    public Tile snakeTile;

    private Tilemap TileMap { get; set; }
    private Board board;
    private Queue<Vector2Int> snakeQueue { get; set; }
    private Vector2Int direction;
    private float moveTime;

    private void Awake()
    {
        TileMap = GetComponentInChildren<Tilemap>();
        board = GetComponent<Board>();
        snakeQueue = new Queue<Vector2Int>();
        direction = Vector2Int.zero;
        moveTime = Time.time + timeDelay;
    }

    private void Start()
    {
        SpawnSnake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2Int.right;
        }

        AutoMoveSnake();      
    }

    private void SpawnSnake()
    {
        Vector2Int snakePosition = new Vector2Int(0, 0);
        snakePosition += spawnOffset;
        snakeQueue.Enqueue(snakePosition);
        TileMap.SetTile((Vector3Int)snakePosition, snakeTile);
    }

    private void AutoMoveSnake()
    { 
        if (Time.time >= moveTime)
        {
            Vector2Int snakeTalePos = snakeQueue.Dequeue();
            Vector2Int newSnakePos = snakeTalePos + direction;

            if (board.IsValidPosition(newSnakePos))
            {
                TileMap.SetTile((Vector3Int)snakeTalePos, null);
                TileMap.SetTile((Vector3Int)newSnakePos, snakeTile);
                snakeQueue.Enqueue(newSnakePos);
            }
            else
            {
                snakeQueue.Enqueue(snakeTalePos);
            }
            
            moveTime = Time.time + timeDelay;
        }     
    }

    private void AddToSnake(Vector2Int position)
    {
        snakeQueue.Enqueue(position);
    }
}
