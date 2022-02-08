using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbidity : VariableClass
{
    [HideInInspector]
    public float[] m_gatheredTurbidityValues = new float[6];
    [HideInInspector]
    public int m_numGatheredTurbidityValues = 0;

    private void Awake()
    {
        variableName = "Turbidity";
    }
}
