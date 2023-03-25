using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    public Vector2Int spawnOffset;
    public float timeDelay;
    public Tile snakeTile;

    private Tilemap tileMap;
    private Board board;
    private Apple apple;
    private Queue<Vector2Int> snakeQueue;
    private Vector2Int direction;
    private float moveTime;
    private Vector2Int snakeHeadPos;

    private void Awake()
    {
        tileMap = GetComponentInChildren<Tilemap>();
        board = GetComponent<Board>();
        apple = GetComponent<Apple>();
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
        if (board.IsGameRunning())
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                CheckForGameStart();
                direction = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                CheckForGameStart();
                direction = Vector2Int.down;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CheckForGameStart();
                direction = Vector2Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                CheckForGameStart();
                direction = Vector2Int.right;
            }

            AutoMoveSnake();
        }          
    }

    public void StopSnake()
    {
        direction = Vector2Int.zero;
    }

    public void SpawnSnake()
    {
        snakeQueue.Clear();

        Vector2Int snakePosition = new Vector2Int(0, 0);
        snakePosition += spawnOffset;

        AddToSnake(snakePosition);
    }

    private void CheckForGameStart()
    {
        if (direction == Vector2Int.zero)
        {
            moveTime = Time.time + timeDelay;
            board.StartGame();
        }
    }

    private void AutoMoveSnake()
    { 
        if (Time.time >= moveTime && direction != Vector2Int.zero)
        {
            Vector2Int newSnakePos = snakeHeadPos + direction;

            if (apple.IsApple(newSnakePos))
            {
                board.UpdateScore();
                AddToSnake(newSnakePos);
                apple.SetAppleSpawn();
            }
            else if (board.IsValidPosition(newSnakePos))
            {
                RemoveTail();
                AddToSnake(newSnakePos);
            }
            else
            {
                StopSnake();
                board.GameOver();
            }
            
            moveTime = Time.time + timeDelay;
        }     
    }

    private void AddToSnake(Vector2Int newSnakePos)
    {
        snakeHeadPos = newSnakePos;
        tileMap.SetTile((Vector3Int)newSnakePos, snakeTile);
        snakeQueue.Enqueue(newSnakePos);
    }

    private void RemoveTail()
    {
        Vector2Int snakeTalePos = snakeQueue.Dequeue();
        tileMap.SetTile((Vector3Int)snakeTalePos, null);
    }
}
