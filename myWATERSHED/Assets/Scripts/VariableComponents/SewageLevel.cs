using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewageLevel : VariableClass
{
   // public float m_SewageLevel;

    [HideInInspector]
    public List<float> m_GatheredSewageValues = new List<float>();

    private void Awake()
    {
        variableName = "Sewage Level";
    }
}
