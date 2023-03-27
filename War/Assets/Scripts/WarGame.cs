using UnityEngine;

public class WarGame : MonoBehaviour
{
    public CardViewerUI cardViewerUI;

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
        SetUp();
        Shuffle();
        Deal();
        ShowGame();
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
                Draw();
                hasDrawn = true;
            }
            else
            {
                Evaluate();
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
            GameObject gameObject = hit.transform.gameObject;
            Deck deck = gameObject.GetComponent<Deck>(); 

            if (deck != null && !deck.IsDeckEmpty())
            {
                cardViewerUI.ViewCards(deck.GetAllCards());
            }
        }
    }

    public void SetUp()
    {
        playersHand.SetHandPosition(new Vector2(0.0f, -3.0f));
        playersHand.SetDrawPilePosition(new Vector2(0.0f, -1.0f));

        opponentsHand.SetHandPosition(new Vector2(0.0f, 3.0f));
        opponentsHand.SetDrawPilePosition(new Vector2(0.0f, 1.0f));
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
        playersHand.Draw();
        opponentsHand.Draw();
    }

    public void Evaluate()
    {
        Card playersCard = playersHand.PeekDrawnCard();
        Card opponentsCard = opponentsHand.PeekDrawnCard();

        if (playersCard.value > opponentsCard.value)
        {
            playersHand.UnDraw();
            playersHand.TakeAll(opponentsHand.GetAllDrawnCards());
        }
        else if (playersCard.value < opponentsCard.value)
        {
            opponentsHand.UnDraw();
            opponentsHand.TakeAll(playersHand.GetAllDrawnCards());
        }
        else
        {
            playersHand.Draw();
            opponentsHand.Draw();
        }

        ShowGame();
    }
}
