using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectPopulation : VariableClass
{
    [HideInInspector]
    public float[] m_gatheredInsectPopValues = new float[6];
    [HideInInspector]
    public int m_numGatheredInsectPopValues = 0;

    private void Awake()
    {
        variableName = "Insect Population";
        moreIsBad = false;
        wholeNumbers = true;
    }
}
