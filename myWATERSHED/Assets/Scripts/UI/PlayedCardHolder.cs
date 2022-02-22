using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayedCardHolder : MonoBehaviour
{
    [SerializeField] private GameObject m_cardPrefab;
    [SerializeField] private float m_targetSpacing;
    private RectTransform rect;

    private List<RectTransform> cards = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    public void AddNewCard(string cardName, string cardDescription)
    {
        GameObject newCard = Instantiate(m_cardPrefab, rect);
        newCard.transform.Find("Card Name").GetComponent<TMP_Text>().text = cardName;
        newCard.transform.Find("Card Description").GetComponent<TMP_Text>().text = cardDescription;
        RectTransform newCardRect = newCard.GetComponent<RectTransform>();
        newCardRect.anchoredPosition = new Vector3(newCardRect.anchoredPosition.x,-50f);
        cards.Add(newCardRect);
        LayoutCards();
    }

    public void DurationExpired() { 
    //pass which card's duration is expired
    //remove it from the cards list
    //somethingorother rect?
    //yes
    }

    private void LayoutCards()
    {
        if (cards.Count * m_targetSpacing > rect.rect.width)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].anchoredPosition = new Vector2(i * (rect.rect.width / cards.Count), cards[i].anchoredPosition.y);
            }
        }
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].anchoredPosition = new Vector2(i * m_targetSpacing, cards[i].anchoredPosition.y);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
