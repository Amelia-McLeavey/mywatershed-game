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
    public List<RectTransform> extinctCards = new List<RectTransform>();

    public PlayedCard newestCard; 

    // Start is called before the first frame update
    void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    public void AddNewCard(CardInstance card)
    {  
            GameObject newCard = Instantiate(m_cardPrefab, rect);
            newestCard = newCard.GetComponent<PlayedCard>();
            newestCard.SetUpCard(card);
            RectTransform newCardRect = newCard.GetComponent<RectTransform>();
            newCardRect.anchoredPosition = new Vector3(newCardRect.anchoredPosition.x, -50f);
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
                cards[i].anchoredPosition = new Vector2((cards.Count - 1 - i) * (rect.rect.width / cards.Count), cards[i].anchoredPosition.y);
            }
        }
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].anchoredPosition = new Vector2((cards.Count-1-i) * m_targetSpacing, cards[i].anchoredPosition.y);
            }
        }
        
    }

    public void NewYear()
    {
        foreach (RectTransform card in extinctCards)
        {
            if (cards.Contains(card))
            {
                cards.Remove(card);
            }
        }

        LayoutCards();

        extinctCards.Clear();
    }

    public IEnumerator UpdateValues()
    {
        foreach (RectTransform rect in cards)
        {
            if (rect != null)
            {
                if (rect.gameObject.activeSelf)
                {
                    rect.gameObject.GetComponent<PlayedCard>().StartCoroutine("NewYear");
                }
            }
            yield return new WaitForSeconds(0.8f);
        }

        yield return new WaitForSeconds(1f);

        NewYear();
    }

    //public void RemoveCard(GameObject card)
    //{
    //    if (cards.Contains(card.GetComponent<RectTransform>()))
    //    {
    //        cards.Remove(card.GetComponent<RectTransform>());
    //    }
    //}

    public void PlaceCard()
    {
        newestCard.CardPlaced();
    }
}
