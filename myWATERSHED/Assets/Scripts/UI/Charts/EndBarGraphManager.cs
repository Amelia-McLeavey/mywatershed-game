using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndBarGraphManager : MonoBehaviour
{
    [SerializeField] private TMP_Text yearNum;
    [SerializeField] private TMP_Text redDaceNumber;
    [SerializeField] private TMP_Text chubNumber;
    [SerializeField] private TMP_Text troutNumber;

    [SerializeField] private TMP_Text title;

    [SerializeField] private Slider redDaceSlider;
    [SerializeField] private Slider chubSlider;
    [SerializeField] private Slider troutSlider;

    public void UpdateBarGraph()
    {
        int maxNum = Mathf.Max(Mathf.Max(int.Parse(redDaceNumber.text), int.Parse(chubNumber.text)), int.Parse(troutNumber.text));

        title.text = yearNum.text + " Population Levels";
        
        redDaceSlider.maxValue = maxNum;
        chubSlider.maxValue = maxNum;
        troutSlider.maxValue = maxNum;

        redDaceSlider.value = int.Parse(redDaceNumber.text);
        chubSlider.value = int.Parse(chubNumber.text);
        troutSlider.value = int.Parse(troutNumber.text);
    }
}
