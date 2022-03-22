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
    [SerializeField] private TMP_Text insectNumber;

    [SerializeField] private TMP_Text title;

    [SerializeField] private Slider redDaceSlider;
    [SerializeField] private Slider chubSlider;
    [SerializeField] private Slider troutSlider;
    [SerializeField] private Slider insectSlider;

    public void UpdateBarGraph()
    {
        title.text = yearNum.text + " Population Levels";
        
        redDaceSlider.maxValue = int.Parse(insectNumber.text)/10;
        chubSlider.maxValue = int.Parse(insectNumber.text) / 10;
        troutSlider.maxValue = int.Parse(insectNumber.text) / 10;
        insectSlider.maxValue = int.Parse(insectNumber.text) / 10;

        redDaceSlider.value = int.Parse(redDaceNumber.text);
        chubSlider.value = int.Parse(chubNumber.text);
        troutSlider.value = int.Parse(troutNumber.text);
        insectSlider.value = int.Parse(insectNumber.text) / 10;

    }
}
