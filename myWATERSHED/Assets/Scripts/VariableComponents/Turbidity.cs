using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbidity : VariableClass
{
    //public float m_Turbidity;

    [HideInInspector]
    public List<float> m_GatheredTurbidityValues = new List<float>();
    private void Awake()
    {
        variableName = "Turbidity";
    }
}
