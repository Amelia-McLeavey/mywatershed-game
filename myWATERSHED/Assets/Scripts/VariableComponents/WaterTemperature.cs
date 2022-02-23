using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTemperature : VariableClass
{
    [HideInInspector]
    public float[] m_gatheredWaterTemperatureValues = new float[6];
    [HideInInspector]
    public int m_numGatheredWaterTemperatureValues = 0;

    private void Awake()
    {
        variableName = "Temperature";
        moreIsBad = false;
        maxValue = 30f;
        targetValue = 12f;
    }
}
