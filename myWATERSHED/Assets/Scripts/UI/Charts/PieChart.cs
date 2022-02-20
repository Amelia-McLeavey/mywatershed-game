using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    [SerializeField] private Image redDacePie;
    [SerializeField] private Image chubPie;
    [SerializeField] private Image troutPie;
    [SerializeField] private Image insectPie;

    public float redDacePop = 0, chubPop = 0, troutPop = 0, insectPop = 0;

    public void ResetAllValues()
    {
        redDacePop = 0;
        chubPop = 0;
        troutPop = 0;
        insectPop = 0;
    }

    public void ShowPie()
    {
        float total = redDacePop + chubPop + troutPop + insectPop;

        if(total == 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            chubPie.fillAmount = (chubPop + troutPop + insectPop) / total;
            troutPie.fillAmount = (troutPop + insectPop) / total;
            insectPie.fillAmount = (insectPop) / total;
        }
       

        
    }

}
