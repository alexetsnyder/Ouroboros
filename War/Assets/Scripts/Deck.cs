using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public CardBackColor cardBackColor;
    public CardBackType cardBackType;

    private List<Card> deckList;
    private List<Card> drawPileList;
    private List<Card> discardPileList;

    private Vector2 drawPilePosition;
    private GameObject drawPile;

    private Vector2 discardPilePosition;
    private GameObject discardPile;

    private void Awake()
    {
        deckList = new List<Card>();
        drawPileList = new List<Card>();
        discardPileList = new List<Card>();
        drawPilePosition = transform.position;
        discardPilePosition = transform.position;

        drawPile = null;
        discardPile = null;
        GenerateDeck();
    }

    private void Start()
    {
        ShowDeck();
    }

    private void Update()
    {

    }

    public bool IsDeckEmpty()
    {
        return (deckList.Count == 0);
    }

    public bool IsDrawPileEmpty()
    {
        return (drawPileList.Count == 0);
    }

    public void SetDeckPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetDrawPilePosition(Vector2 position)
    {
        drawPilePosition = position;
    }

    public void SetDiscardPilePosition(Vector2 position)
    {
        discardPilePosition = position;
    }

    private void GenerateDeck()
    {
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            if (suit != CardSuit.NONE)
            {
                foreach (CardType type in Enum.GetValues(typeof(CardType)))
                {
                    if (type != CardType.JOKER)
                    {
                        deckList.Add(new Card(suit, type));
                    }
                }
            }  
        }

        AddJokers(2);
    }

    private void AddJokers(int number)
    {
        for (int i = 0; i < number; i++)
        {
            deckList.Add(new Card(CardSuit.NONE, CardType.JOKER));
        }
    }

    private void ShowDeck()
    {
        string filePath = Card.GetBackFilePath(cardBackColor, cardBackType);
        SpriteRenderer renderer =  gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(filePath);
    }

    public void Shuffle()
    {
        Shuffle(deckList);
    }

    private void Shuffle(List<Card> cardList)
    {
        for (int i = cardList.Count - 1; i > 0; i--)
        {
            int k = Random.Range(0, i + 1);
            Card card = cardList[k];
            cardList[k] = cardList[i];
            cardList[i] = card;
        }
    }

    public void Draw()
    {
        Card card = deckList.FirstOrDefault();

        if (card != null)
        {
            deckList.Remove(card);
            drawPileList.Add(card);
        }

        ShowDrawPile();
    }

    private void ShowDrawPile()
    {
        Card card = drawPileList.LastOrDefault();

        if (card != null)
        {
            if (drawPile == null)
            {
                drawPile = (GameObject)Instantiate(Resources.Load("EmptyCard"), transform);
                drawPile.transform.position = drawPilePosition;
            }

            SpriteRenderer renderer = drawPile.GetComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(card.filePath);
        }
        else
        {
            if (drawPile != null)
            {
                Destroy(drawPile);
            }
        }
    }

    public Card GetLastDrawnCard()
    {
        return drawPileList.LastOrDefault();
    }

    public void MoveToBottomOfDeck()
    {
        Card card = drawPileList.LastOrDefault();
        if (card != null)
        {
            MoveToBottomOfDeck(card);
        }
    }

    public void MoveAllToBottomOfDeck()
    {
        Shuffle(drawPileList);

        foreach (Card card in drawPileList)
        {
            deckList.Add(card); 
        }

        drawPileList.Clear();
        ShowDrawPile();
    }

    public void MoveToBottomOfDeck(Card card)
    {
        drawPileList.Remove(card);
        deckList.Add(card);
    }

    public void Discard()
    {
        Card card = drawPileList.LastOrDefault();
        if (card != null)
        {
            Discard(card);
        }
    }

    public void DiscardAll()
    {
        drawPileList.Reverse();
        foreach (Card card in drawPileList)
        {
            discardPileList.Add(card);
        }

        drawPileList.Clear();
        ShowDrawPile();
        ShowDiscardPile();
    }

    public void Discard(Card card)
    {
        drawPileList.Remove(card);
        discardPileList.Add(card);
        ShowDrawPile();
        ShowDiscardPile();
    }

    private void ShowDiscardPile()
    {
        Card card = discardPileList.LastOrDefault();

        if (card != null)
        {
            if (discardPile == null)
            {
                discardPile = (GameObject)Instantiate(Resources.Load("EmptyCard"), transform);
                discardPile.transform.position = discardPilePosition;
            }

            SpriteRenderer renderer = discardPile.GetComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(card.filePath);
        }
        else
        {
            if (discardPile != null)
            {
                Destroy(discardPile);
            }
        }
    }

    public void ReturnCardsFromDiscard()
    {
        foreach (Card card in discardPileList)
        {
            deckList.Add(card);
        }

        discardPileList.Clear();
    }

    public Card GetCardInDeck(int index)
    {
        return deckList[index];
    }

    public Card GetCardInDeck(Card card)
    {
        return GetCardInDeck(card.type, card.suit);
    }

    public Card GetCardInDeck(CardType type, CardSuit suit)
    {
        foreach (Card card in deckList)
        {
            if (card.suit == suit && card.type == type)
            {
                return card;
            }
        }
        return null;
    }  
}
