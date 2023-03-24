using UnityEngine;
using TMPro;

public enum Side
{
    PLAYER,
    OPPONENT,
}

public class Board : MonoBehaviour
{
    public GameObject dot;
    public Ball ball;
    public Paddle player;
    public Paddle opponent;
    public TMP_Text playerScore;
    public TMP_Text opponentScore;
    public TMP_Text gameText;

    public bool IsRunning { get; set; }

    public Rect Bounds
    {
        get
        {
            Vector2 position = transform.position;
            Vector2 size = transform.localScale;
            return new Rect(new Vector2(position.x - size.x / 2, position.y - size.y / 2), size); 
        }
    }

    private void Awake()
    {
        IsRunning = false;
    }

    private void Start()
    {
        CreateDottedLine();
    }

    private void Update()
    {
        if (!IsRunning && Input.GetKeyDown(KeyCode.Space))
        {
            IsRunning = true;
            Restart();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsRunning = false;
            Restart();
            gameText.text = "Press Space To Start";
            ball.SetUp();
            player.Return();
            opponent.Return();
        }    
    }

    private void CreateDottedLine()
    {
        Rect bounds = Bounds;
        float seperation = 0.5f;
        float dotSize = dot.transform.localScale.y;

        int number = Mathf.FloorToInt(bounds.size.y / (seperation + dotSize));
        Vector2 position = new(0.0f, bounds.y + seperation);

        for (int i = 0; i <= number; i++)
        {
            Instantiate(dot, position, Quaternion.identity);
            position.y += seperation + dotSize;
        }
    }

    public void Restart()
    {
        ClearScore();
        gameText.text = "";
    }

    public void GameOver(Side winner)
    {
        IsRunning = false;

        if (winner == Side.PLAYER)
        {
            gameText.text = "You Win!";
        }
        else
        {
            gameText.text = "You Lose!";
        }

        player.Return();
        opponent.Return();
    }

    public void ClearScore()
    {
        playerScore.text = "0";
        opponentScore.text = "0";
    }

    public void UpdateScore(Side side)
    {
        if (side == Side.PLAYER)
        {
            int score = (int.Parse(playerScore.text) + 1);
            playerScore.text = score.ToString();
            if (score == 5)
            {
                GameOver(Side.PLAYER);
                ball.SetUp();
            }
        }
        else if (side == Side.OPPONENT)
        {
            int score = (int.Parse(opponentScore.text) + 1);
            opponentScore.text = score.ToString();
            if (score == 5)
            {
                GameOver(Side.OPPONENT);
                ball.SetUp();
            }
        }
    }
}
