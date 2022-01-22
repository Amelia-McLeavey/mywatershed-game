using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreekChubPopulation : VariableClass
{
    //public int m_CreekChubPopulation;
    private void Awake()
    {
        variableName = "Creek Chub Population";
        moreIsBad = false;
        wholeNumbers = true;
    }
}
