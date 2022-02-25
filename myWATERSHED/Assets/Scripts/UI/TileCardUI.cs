using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    [SerializeField] private float speed;
    [SerializeField] private float smallScale;
    [SerializeField] private float maxScale;
    private Vector3 targetScale;

    void Start()
    {
        targetScale = Vector3.one * smallScale;
        rect = this.GetComponent<RectTransform>();
    }
    void Update()
    {
        rect.localScale = Vector3.Lerp(rect.localScale, targetScale, Time.deltaTime*speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = Vector3.one * maxScale;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = Vector3.one * smallScale;
    }
}
