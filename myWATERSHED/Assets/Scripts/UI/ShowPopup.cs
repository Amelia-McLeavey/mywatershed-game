using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ShowPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PopupText popupText;
    [SerializeField] private string popupToDisplay;

    private void Start()
    {
        popupText = GameObject.FindObjectOfType<PopupText>();
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
