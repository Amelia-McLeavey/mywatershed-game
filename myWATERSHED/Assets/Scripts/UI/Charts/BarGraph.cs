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
    [SerializeField] private TMP_Text redDaceNum;
    [SerializeField] private TMP_Text chubNum;
    [SerializeField] private TMP_Text troutNum;

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
        Debug.Log(max +" -> "+ max / 10);
        max = (Mathf.FloorToInt(max / 10) + 1) * 10;
        
        MaxNum.text = max.ToString();
        MidNum.text = (max / 2).ToString();

        redDaceBar.maxValue = max;
        redDaceBar.value = redDacePop;
        redDaceNum.text = redDacePop.ToString(); 

        chubBar.maxValue = max;
        chubBar.value = chubPop;
        chubNum.text = chubPop.ToString();

        troutBar.maxValue = max;
        troutBar.value = troutPop;
        troutNum.text = troutPop.ToString();
    }
}
