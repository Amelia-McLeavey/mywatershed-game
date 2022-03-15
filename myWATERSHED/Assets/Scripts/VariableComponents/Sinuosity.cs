using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinuosity : VariableClass
{
   // public float m_Sinuosity;
    private void Awake()
    {
        variableName = "Sinuosity";
        targetValue = 0.5f;
        moreIsBad = false;
    }
}
