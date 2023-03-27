using System;
using TMPro;
using UnityEngine;

public class MouseButton
{
    public const int LEFT = 0;
    public const int RIGHT = 1;
    public const int MIDDLE_CLICK = 2;
}

public class WarGame : MonoBehaviour
{
    public CardViewerUI cardViewerUI;
    
    public TMP_Text gameStatus;

    public TMP_Text playerStatus;
    public TMP_Text playerDeckCount;

    public TMP_Text opponentStatus;
    public TMP_Text opponentDeckCount;

    private Deck playingCards;
    private WarHand playersHand;
    private WarHand opponentsHand;

    private bool isGameOver;
    private bool hasDrawn;

    private void Awake()
    {
        playingCards = Factory.CreateNewDeck(transform);
        playingCards.GenerateStandardDeck();

        playersHand = Factory.CreateNewWarHand(transform);
        opponentsHand = Factory.CreateNewWarHand(transform);

        isGameOver = false;
        hasDrawn = false;
    }

    public void Start()
    {
        SetPositions();
        Shuffle();
        Deal();
        ShowGame();
        SetUp();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(MouseButton.LEFT))
        {
            Select();
        }

        if (!isGameOver && Input.GetKeyUp(KeyCode.Space))
        {
            if (!hasDrawn)
            {
                ClearWinner();
                Draw();
                hasDrawn = true;
                gameStatus.text = "Evaluate!";
                UpdateDeckCount();
            }
            else
            {
                Evaluate();
                hasDrawn = false;
                gameStatus.text = "Draw!";
                UpdateDeckCount();
            }
        }
    }

    private void Select()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0.0f);

        if (hit)
        {
            GameObject gameObject = hit.transform.gameObject;
            Deck deck = gameObject.GetComponent<Deck>(); 

            if (deck != null && !deck.IsDeckEmpty())
            {
                cardViewerUI.ViewCards(deck.GetAllCards());
            }
        }
    }

    public void SetPositions()
    {
        playersHand.SetHandPosition(new Vector2(0.0f, -5.0f));
        playersHand.SetDrawPilePosition(new Vector2(0.0f, -2.0f));

        opponentsHand.SetHandPosition(new Vector2(0.0f, 5.0f));
        opponentsHand.SetDrawPilePosition(new Vector2(0.0f, 2.0f));
    }

    public void SetUp()
    {
        playerStatus.text = "";
        opponentStatus.text = "";

        UpdateDeckCount();

        gameStatus.text = "Begin!";
    }

    public void Shuffle()
    {
        playingCards.Shuffle();
    }

    public void Deal()
    {
        int n = 2;
        Card card = playingCards.Draw();

        while (card != null)
        {
            if (n % 2 == 0)
            {
                playersHand.Take(card);
            }
            else
            {
                opponentsHand.Take(card);
            }
            n++;
            card = playingCards.Draw();
        }
    }

    public void ShowGame()
    {
        playersHand.ShowHand();
        opponentsHand.ShowHand();
    }

    public void Draw()
    {
        if(IsGameOver())
        {
            isGameOver = true;
            GameOver();
        }

        playersHand.Draw();
        opponentsHand.Draw();
    }

    private void GameOver()
    {
        gameStatus.text = "Game Over!";
    }

    public void Evaluate()
    {
        Card playersCard = playersHand.PeekDrawnCard();
        Card opponentsCard = opponentsHand.PeekDrawnCard();

        if (playersCard.value > opponentsCard.value)
        {
            playersHand.UnDraw();
            playersHand.TakeAll(opponentsHand.GetAllDrawnCards());
            UpdatePlayerWin();
        }
        else if (playersCard.value < opponentsCard.value)
        {
            opponentsHand.UnDraw();
            opponentsHand.TakeAll(playersHand.GetAllDrawnCards());
            UpdateOpponentWin();
        }
        else
        {
            Draw();
            if (IsGameOver())
            {
                isGameOver = true;
                GameOver();
            }
        }

        ShowGame();
    }

    public void UpdateDeckCount()
    {
        playerDeckCount.text = playersHand.HandCount().ToString();
        opponentDeckCount.text = opponentsHand.HandCount().ToString();
    }

    public void ClearWinner()
    {
        playerStatus.text = "";
        opponentStatus.text = "";
    }

    public void UpdatePlayerWin()
    {
        playerStatus.text = "Winner!";
        opponentStatus.text = "Loser!";
    }

    public void UpdateOpponentWin()
    {
        playerStatus.text = "Loser!";
        opponentStatus.text = "Winner!";
    }

    public bool IsGameOver()
    {
        if (playersHand.HandCount() == 0 || opponentsHand.HandCount() == 0)
        {
            return true;
        }
        return false;
    }
}
