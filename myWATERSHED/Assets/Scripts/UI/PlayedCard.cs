using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedCard : MonoBehaviour
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

    private void OnMouseEnter()
    {
        targetYPos = m_hoverOverYPos;
    }

    private void OnMouseExit()
    {
        targetYPos = 0f;
    }
}
