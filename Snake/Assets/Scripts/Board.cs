using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Vector2Int boardSize;
    public int appleScore;
    public TMP_Text scoreText;
    public TMP_Text gameProgressText;

    private Tilemap tileMap;
    private Snake snake;
    private Apple apple;
    private bool isRunning;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tileMap = GetComponentInChildren<Tilemap>();
        snake = GetComponent<Snake>();
        apple = GetComponent<Apple>();
    }

    private void Start()
    {
        SetUpGame();
    }

    private void Update()
    {
        if (!isRunning && Input.GetKeyDown(KeyCode.Space))
        {
            SetUpGame();
        }
    }

    public bool IsGameRunning()
    {
        return isRunning;
    }

    public void SetUpGame()
    {
        isRunning = true;
        scoreText.text = "0";
        gameProgressText.text = "Paused";

        tileMap.ClearAllTiles();
        snake.SpawnSnake();
        apple.SetAppleSpawn();
    }

    public void StartGame()
    {
        gameProgressText.text = "In Progress";
    }

    public void GameOver()
    {
        isRunning = false;
        gameProgressText.text = "Game Over!";
    }

    public void GameWin()
    {
        isRunning = false;
        snake.StopSnake();
        gameProgressText.text = "You Won!";
    }

    public bool IsValidPosition(Vector2Int position)
    {
        RectInt bounds = Bounds;

        if (bounds.Contains(position) &&
            !tileMap.HasTile((Vector3Int)position))
        {
            return true;
        }

        return false;
    }

    public void UpdateScore()
    {
        scoreText.text = (int.Parse(scoreText.text) + appleScore).ToString();
    }
}
