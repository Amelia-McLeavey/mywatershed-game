using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileVariableDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image icon;
    [SerializeField] private Slider slider;
    [SerializeField] private RectTransform targetArea;

    [SerializeField] private Image sliderFill;

    public VariableClass variableClassToRead;


    public bool abiotic = true;
    public void SetVariableClass(VariableClass varClass)
    {
        variableClassToRead = varClass;
    }

    private void Update()
    {
        if (variableClassToRead != null)
        {
            if (variableClassToRead.wholeNumbers)
            {
                nameText.text = variableClassToRead.variableName;// + " : " + variableClassToRead.value;
                slider.maxValue = 50;
            }
            else
            {
                nameText.text = variableClassToRead.variableName;// + " : " + variableClassToRead.value.ToString("F3");
                slider.maxValue = 1;
            }
            slider.maxValue = variableClassToRead.maxValue;
            slider.value = variableClassToRead.value;

            float targetPos = (variableClassToRead.targetValue / variableClassToRead.maxValue) * slider.GetComponent<RectTransform>().rect.width;
            targetArea.anchoredPosition = new Vector2(Mathf.Clamp(targetPos, targetArea.rect.width/2f, slider.GetComponent<RectTransform>().rect.width - targetArea.rect.width / 2f), 0f);
            if (variableClassToRead.moreIsBad)
            {
                sliderFill.color = new Color((1f* (slider.value / slider.maxValue)), (1f* (1f- (slider.value / slider.maxValue))), 0f);
            }
            else
            {
                sliderFill.color = new Color((1f * (1f - (slider.value / slider.maxValue))), (1f * (slider.value / slider.maxValue)), 0f);
            }
        }
    }

}
