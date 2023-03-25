using UnityEngine;

public enum Player
{
    NONE,
    ONE,
    TWO,
}

public class CardManager : MonoBehaviour
{
    private Deck player1;
    private Deck player2;

    private void Start()
    {
        //player1 = (Deck)Instantiate(Resources.Load("Deck"), transform);
        //player2 = (Deck)Instantiate(Resources.Load("Deck"), transform);
    }

    public void Shuffle()
    {
        player1.Shuffle();
        player2.Shuffle();
    }

    public void Draw()
    {
        player1.Draw();
        player2.Draw();
    }

    public Player Evaluate()
    {
        Card player1Card = player1.GetDrawnCard();
        Card player2Card = player2.GetDrawnCard();

        if (player1Card.value > player2Card.value)
        {
            return Player.ONE;
        }
        else if (player1Card.value < player2Card.value)
        {
            return Player.TWO;
        }

        return Player.NONE;
    }

    public void Discard(Player loser)
    {
        switch (loser)
        {
            case Player.ONE:
                player1.Discard();
                player2.MoveToBottomOfDeck();
                break;
            case Player.TWO:
                player1.MoveToBottomOfDeck();
                player2.Discard();
                break;
        }
    }
}
