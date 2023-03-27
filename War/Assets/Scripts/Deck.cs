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

    private void Awake()
    {
        deckList = new List<Card>();
    }

    public bool IsDeckEmpty()
    {
        return (deckList.Count == 0);
    }

    public void SetDeckPosition(Vector2 position)
    {
        transform.position = position;
    }

    public int Count()
    {
        return deckList.Count;
    }

    public void Add(Card card)
    {
        deckList.Add(card);
    }

    public void AddToBottom(Card card)
    {
        deckList.Insert(0, card);
    }

    public void GenerateStandardDeck()
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

    public void ShowDeck()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (deckList.Count > 0)
        {
            string filePath = Card.GetBackFilePath(cardBackColor, cardBackType); 
            renderer.sprite = Resources.Load<Sprite>(filePath);
        }
        else
        {
            renderer.sprite = null;
        }
    }

    public void ShowTopCard()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (deckList.Count > 0)
        {
            Card card = deckList.LastOrDefault();

            if (card != null)
            {   
                renderer.sprite = Resources.Load<Sprite>(card.filePath);
            }
        }
        else
        {
            renderer.sprite = null;
        }
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

    public Card Peek()
    {
        return deckList.LastOrDefault();
    }

    public Card Draw()
    {
        Card card = deckList.LastOrDefault();

        if (card != null)
        {
            deckList.Remove(card);
            return card;
        }

        return null;
    }

    public List<Card> GetAllCards()
    {
        return deckList;
    }

    public void RemoveAll()
    {
        deckList.Clear();
    } 
}
