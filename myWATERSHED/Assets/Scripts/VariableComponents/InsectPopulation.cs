using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectPopulation : VariableClass
{
    //public int m_InsectPopulation;

    [HideInInspector]
    public List<int> m_GatheredInsectPopulationValues = new List<int>();

    private void Awake()
    {
        variableName = "Insect Population";
        moreIsBad = false;
        wholeNumbers = true;
    }
}
