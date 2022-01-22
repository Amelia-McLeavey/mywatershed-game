using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTemperature : VariableClass
{
    //public float m_waterTemperature;
    
    [HideInInspector]
    public List<float> m_GatheredWaterTemperatureValues = new List<float>();

    private void Awake()
    {
        variableName = "Temperature";
        moreIsBad = false;
    }
}
