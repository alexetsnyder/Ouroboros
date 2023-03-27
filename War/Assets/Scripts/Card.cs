using System;
using UnityEngine;

public enum CardSuit
{
    DIAMONDS,
    HEARTS,
    SPADES,
    CLUBS,
    NONE,
}

public enum CardType
{
    JOKER,
    KING,
    QUEEN,
    JACK,
    TEN,
    NINE,
    EIGHT,
    SEVEN,
    SIX,
    FIVE,
    FOUR,
    THREE,
    TWO,
    ACE,
}

public enum CardBackColor
{
    BLUE,
    GREEN,
    RED,
}

public enum CardBackType
{
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
}

public class Card
{
    public CardSuit suit;
    public CardType type;

    public int value;
    public string filePath;

    public Card(CardSuit suit, CardType card)
    {
        this.suit = suit;
        this.type = card;
        this.value = GetCardValue();
        filePath = GetFilePrefix() + GetFilePostfix();
    }

    public static string GetBackFilePath(CardBackColor color, CardBackType type)
    {
        string filePath = "Cards/cardBack_";

        filePath += Enum.GetName(typeof(CardBackColor), color).ToLower();

        filePath += GetBackTypeString(type);

        return filePath;
    }

    private static string GetBackTypeString(CardBackType type)
    {
        string backType = "";

        switch (type)
        {
            case CardBackType.ONE:
                backType = "1";
                break;
            case CardBackType.TWO:
                backType = "2";
                break;
            case CardBackType.THREE:
                backType = "3";
                break;
            case CardBackType.FOUR:
                backType = "4";
                break;
            case CardBackType.FIVE:
                backType = "5";
                break;
        }

        return backType;
    }

    private int GetCardValue()
    {
        int cardVal = 0;

        switch (type)
        {
            case CardType.JOKER:
                cardVal = 15;
                break;
            case CardType.ACE:
                cardVal = 14;
                break;
            case CardType.KING:
                cardVal = 13;
                break;
            case CardType.QUEEN:
                cardVal = 12;
                break;
            case CardType.JACK:
                cardVal = 11;
                break;
            case CardType.TEN:
                cardVal = 10;
                break;
            case CardType.NINE:
                cardVal = 9;
                break;
            case CardType.EIGHT:
                cardVal = 8;
                break;
            case CardType.SEVEN:
                cardVal = 7;
                break;
            case CardType.SIX:
                cardVal = 6;
                break;
            case CardType.FIVE:
                cardVal = 5;
                break;
            case CardType.FOUR:
                cardVal = 4;
                break;
            case CardType.THREE:
                cardVal = 3;
                break;
            case CardType.TWO:
                cardVal = 2;
                break;  
        }

        return cardVal;
    }

    private string GetFilePrefix()
    {
        string retVal = "Cards/card";

        switch (suit)
        {
            case CardSuit.DIAMONDS:
                retVal += "Diamonds";
                break;
            case CardSuit.HEARTS:
                retVal += "Hearts";
                break;
            case CardSuit.SPADES:
                retVal += "Spades";
                break;
            case CardSuit.CLUBS:
                retVal += "Clubs";
                break;
        }

        return retVal;
    }

    private string GetFilePostfix()
    {
        string retVal = "";

        switch (type)
        {
            case CardType.JOKER:
                retVal = "Joker";
                break;
            case CardType.KING:
                retVal = "K";
                break;
            case CardType.QUEEN:
                retVal = "Q";
                break;
            case CardType.JACK:
                retVal = "J";
                break;
            case CardType.ACE:
                retVal = "A";
                break;
            default:
                retVal = value.ToString();
                break;
        }

        return retVal;
    }
}
