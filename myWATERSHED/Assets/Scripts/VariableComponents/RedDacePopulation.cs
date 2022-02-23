using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDacePopulation : VariableClass
{
    //public int m_RedDacePopulation;
    private void Awake()
    {
        variableName = "Red Dace Population";
        moreIsBad = false;
        wholeNumbers = true;
        maxValue = 100;
    }
}
