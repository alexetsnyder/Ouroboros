using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViewerUI : MonoBehaviour
{
    public Image cardView;
    public RectTransform contentPanel;
    public List<Image> cardViews;

    private void Awake()
    {
        cardViews = new List<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void ViewCards(List<Card> cards)
    {
        Show();

        foreach (Card card in cards)
        { 
            Image image = Instantiate(cardView, contentPanel);
            image.sprite = Resources.Load<Sprite>(card.filePath);
            cardViews.Add(image);
        }
    }

    public void Hide()
    {
        foreach (Image image in cardViews)
        {
            Destroy(image.gameObject);
        }

        cardViews.Clear();

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
