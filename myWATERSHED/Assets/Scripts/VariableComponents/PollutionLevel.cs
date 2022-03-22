using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionLevel : VariableClass
{
    [HideInInspector]
    public float[] m_gatheredPollutionValues = new float[6];
    [HideInInspector]
    public int m_numGatheredPollutionValues = 0;

    private void Awake()
    {
        variableName = "Pollution Level";
        targetValue = 0.1f;
    }
}
