using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiparianLevel : VariableClass
{
   // public float m_RiparianLevel;
    private void Awake()
    {
        variableName = "Riparian Level";
        moreIsBad = false;
        targetValue = 1f;
    }
}
