using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupText : MonoBehaviour
{
    private RectTransform rt;
    private TMP_Text text;

    public Vector2 offset;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (text.enabled)
        {
            rt.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset;
        }
    }
    public void ShowPopUp(string textToDisplay)
    {
        text.text = textToDisplay;
        text.enabled = true;
    }

    public void HidePopUp(string textToDisplay)
    {
        if (text.text == textToDisplay)
        {
            text.enabled = false;
        }
    }
}
