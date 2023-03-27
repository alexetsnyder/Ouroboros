using UnityEngine;

public enum Player
{
    NONE,
    ONE,
    TWO,
}

public class MouseButton
{
    public const int LEFT = 0;
    public const int RIGHT = 1;
    public const int MIDDLE_CLICK = 2;
}

public class CardManager : MonoBehaviour
{
    private Deck player1;
    private Deck player2;

    private bool hasDrawn;

    private void Awake()
    {
        hasDrawn = false;
    }

    private void Start()
    {
        GameObject goPlayer1 = (GameObject)Instantiate(Resources.Load("Deck"), transform);
        GameObject goPlayer2 = (GameObject)Instantiate(Resources.Load("Deck"), transform);

        player1 = goPlayer1.GetComponent<Deck>();
        player2 = goPlayer2.GetComponent<Deck>();

        SetDeckPosition();

        Shuffle();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(MouseButton.LEFT))
        {
            Select();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!hasDrawn)
            {
                Draw();
                hasDrawn = true;
            }
            else
            {
                PlayWar();
                hasDrawn = false;
            }
        }
    }

    private void Select()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0.0f);

        if (hit)
        {
            Debug.Log(hit.transform.name);
        }
    }

    private void SetDeckPosition()
    {
        Vector2 player1Pos = new Vector2(0.0f, -3f);
        Vector2 player1DrawPos = new Vector2(0.0f, -1.0f);
        Vector2 player1DisPos = new Vector2(-1.5f, -3f);

        player1.SetDeckPosition(player1Pos);
        //player1.SetDrawPilePosition(player1DrawPos);
        //player1.SetDiscardPilePosition(player1DisPos);

        Vector2 player2Pos = new Vector2(0.0f, 3f);
        Vector2 player2DrawPos = new Vector2(0.0f, 1.0f);
        Vector2 player2DisPos = new Vector2(-1.5f, 3f);

        player2.SetDeckPosition(player2Pos);
        //player2.SetDrawPilePosition(player2DrawPos);
        //player2.SetDiscardPilePosition(player2DisPos);
    }

    public void Shuffle()
    {
        player1.Shuffle();
        player2.Shuffle();
    }

    public void PlayWar()
    {
        //Player winner = Evaluate();

        //if (winner != Player.NONE)
        //{
        //    Discard(winner);
        //}

        //if (HasLost(player1))
        //{
        //    Debug.Log("Player 2 has won!");
        //}
        //else if (HasLost(player2))
        //{
        //    Debug.Log("Player 1 has won!");
        //}
    }

    public void Draw()
    {
        player1.Draw();
        player2.Draw();
    }

    public Player Evaluate()
    {
        //Card player1Card = player1.GetLastDrawnCard();
        //Card player2Card = player2.GetLastDrawnCard();

        //if (player1Card.value > player2Card.value)
        //{
        //    return Player.ONE;
        //}
        //else if (player1Card.value < player2Card.value)
        //{
        //    return Player.TWO;
        //}

        return Player.NONE;
    }

    public void Discard(Player winner)
    {
        //switch (winner)
        //{
        //    case Player.ONE:
        //        player1.MoveAllToBottomOfDeck();
        //        player2.DiscardAll();
        //        break;
        //    case Player.TWO:
        //        player1.DiscardAll();
        //        player2.MoveAllToBottomOfDeck();
        //        break;
        //}
    }

    //public bool HasLost(Deck deck)
    //{
    //    return (deck.IsDeckEmpty() && deck.IsDrawPileEmpty());
    //}
}
