using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShowPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PopupText popupText;
    [SerializeField] private string popupToDisplay;

    private void Start()
    {
        popupText = GameObject.FindObjectOfType<PopupText>();
        if (GetComponent<Image>() != null)
        {
            this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.8f;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        popupText.ShowPopUp(popupToDisplay);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        popupText.HidePopUp(popupToDisplay);
    }
}
