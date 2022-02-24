using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayedCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    [SerializeField] private float m_cardSpeed;
    [SerializeField] private float m_hoverOverYPos;

    public CardInstance cardInstance;

    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;

    private float targetYPos=0f;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetUpCard(CardInstance card)
    {
        cardInstance = card;
        cardName.text = cardInstance.cardName;
        cardDescription.text = cardInstance.cardDescription;
    }

    // Update is called once per frame
    void Update()
    {
       rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(rect.anchoredPosition.y, targetYPos, Time.deltaTime * m_cardSpeed));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetYPos = m_hoverOverYPos;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        targetYPos = 0f;
    }

}
