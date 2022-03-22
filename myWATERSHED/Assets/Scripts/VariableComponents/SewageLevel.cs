using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewageLevel : VariableClass
{
    [HideInInspector]
    public float[] m_gatheredSewageValues = new float[6];
    [HideInInspector]
    public int m_numGatheredSewageValues = 0;

    private void Awake()
    {
        variableName = "Sewage Level";
        targetValue = 0.1f;
    }
}
