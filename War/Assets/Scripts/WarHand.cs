using System.Collections.Generic;
using UnityEngine;

public class WarHand : MonoBehaviour
{
    private Deck playerHand;
    private Deck drawPile;

    public void Awake()
    {
        playerHand = Factory.CreateNewDeck(transform);
        drawPile = Factory.CreateNewDeck(transform);
    }

    public void ShowHand()
    {
        playerHand.ShowDeck();
        drawPile.ShowTopCard();
    }

    public void SetHandPosition(Vector2 position)
    {
        transform.position = position; 
    }

    public void SetDrawPilePosition(Vector2 position)
    {
        drawPile.SetDeckPosition(position); 
    }

    public void TakeAll(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            Take(card);
        }

        playerHand.ShowDeck();
    }

    public void Take(Card card)
    {
        playerHand.Add(card);
    }

    public void UnDraw()
    {
        foreach (Card card in drawPile.GetAllCards())
        {
            Take(card);
        }

        drawPile.RemoveAll();
    }

    public void Draw()
    {
        Card card = playerHand.Draw();

        if (card != null)
        {
            drawPile.Add(card);
            ShowHand();
        }
    }

    public Card PeekDrawnCard()
    {
        return drawPile.Peek();
    }

    public List<Card> GetAllDrawnCards()
    {
        List<Card> cards = new List<Card>();

        foreach (Card card in drawPile.GetAllCards())
        {
            cards.Add(card);
        }

        drawPile.RemoveAll();
        ShowHand();

        return cards;
    }
}
