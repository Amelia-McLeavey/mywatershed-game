using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayedCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    [SerializeField] private float m_cardSpeed;
    [SerializeField] private float m_hoverOverYPos;

    private float targetYPos=0f;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(rect.anchoredPosition.y, targetYPos, Time.deltaTime* m_cardSpeed));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovering over");
        targetYPos = m_hoverOverYPos;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("hovering over");
        targetYPos = 0f;
    }

}
