using UnityEngine;

public class Factory : MonoBehaviour
{
    public static WarHand CreateNewWarHand(Transform parent)
    {
        GameObject goDeck = (GameObject)Instantiate(Resources.Load("WarHand"), parent);
        return goDeck.GetComponent<WarHand>();
    }

    public static Deck CreateNewDeck(Transform parent)
    {
        GameObject goDeck = (GameObject)Instantiate(Resources.Load("Deck"), parent);
        return goDeck.GetComponent<Deck>();
    }
}
