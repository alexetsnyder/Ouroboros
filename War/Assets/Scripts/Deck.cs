using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        ShowCard();
    }

    private void GenerateDeck()
    {
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardType type in Enum.GetValues(typeof(CardType)))
            {
                if (type != CardType.JOKER)
                {
                    cardList.Add(new Card(suit, type));
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

    public void ShowCard()
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


    public void Discard()
    {

    }

    public void Shuffle()
    {

    }
}
