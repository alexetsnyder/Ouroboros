using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public CardBackColor cardBackColor;
    public CardBackType cardBackType;

    private List<Card> cardList;
    private List<Card> discardList;

    private Vector2 cardPosition;
    private GameObject cardShowing;

    private void Awake()
    {
        cardList = new List<Card>();
        discardList = new List<Card>();
        cardPosition = transform.position;
        cardPosition.y += 2;
        cardShowing = null;
        GenerateDeck();
    }

    private void Start()
    {
        ShowDeck();
        Shuffle();
        Draw();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToBottomOfDeck();
            Draw();
        }
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
                        cardList.Add(new Card(suit, type));
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
            cardList.Add(new Card(CardSuit.NONE, CardType.JOKER));
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
        Card card = cardList.FirstOrDefault();
        
        if (card != null)
        {
            if (cardShowing == null)
            {
                cardShowing = (GameObject)Instantiate(Resources.Load("EmptyCard"), transform);
                cardShowing.transform.position = cardPosition;
            }

            SpriteRenderer renderer = cardShowing.GetComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(card.filePath);
        }     
    }

    public Card GetDrawnCard()
    {
        return cardList.FirstOrDefault();
    }

    public void MoveToBottomOfDeck()
    {
        Card card = cardList.FirstOrDefault();
        if (card != null)
        {
            MoveToBottomOfDeck(card);
        }
    }

    public void MoveToBottomOfDeck(Card card)
    {
        cardList.Remove(card);
        cardList.Add(card);
    }

    public void Discard()
    {
        Card card = cardList.FirstOrDefault();
        if (card != null)
        {
            Discard(card);
        }
    }

    public void Discard(Card card)
    {
        cardList.Remove(card);
        discardList.Add(card);
    }

    public void ReturnCardsFromDiscard()
    {
        foreach (Card card in discardList)
        {
            cardList.Add(card);
        }

        discardList.Clear();
    }

    public Card GetCardInDeck(int index)
    {
        return cardList[index];
    }

    public Card GetCardInDeck(Card card)
    {
        return GetCardInDeck(card.type, card.suit);
    }

    public Card GetCardInDeck(CardType type, CardSuit suit)
    {
        foreach (Card card in cardList)
        {
            if (card.suit == suit && card.type == type)
            {
                return card;
            }
        }
        return null;
    }  
}
