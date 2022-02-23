using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarGraph : MonoBehaviour
{
    [SerializeField] private Slider redDaceBar;
    [SerializeField] private Slider chubBar;
    [SerializeField] private Slider troutBar;
    
    [SerializeField] private TMP_Text MaxNum;
    [SerializeField] private TMP_Text MidNum;


    public float redDacePop = 0, chubPop = 0, troutPop = 0;
    public void ResetAllValues()
    {
        redDacePop = 0;
        chubPop = 0;
        troutPop = 0;
    }

    public void ShowGraph()
    {
        float max = Mathf.Max(redDacePop, chubPop, troutPop);

        max = ((max % 10) + 1) * 10;

        MaxNum.text = max.ToString();
        MidNum.text = (max / 2).ToString();

        redDaceBar.maxValue = max;
        redDaceBar.value = redDacePop;

        chubBar.maxValue = max;
        chubBar.value = chubPop;

        troutBar.maxValue = max;
        troutBar.value = troutPop;
    }
}
