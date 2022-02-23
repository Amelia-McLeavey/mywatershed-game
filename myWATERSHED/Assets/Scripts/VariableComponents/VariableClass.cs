using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VariableClass : MonoBehaviour
{
    public string variableName;
    public float value;
    public bool wholeNumbers = false;
    public bool moreIsBad = true;


    public float maxValue = 1f;
    public float minValue = 0f;
    public float targetValue = 0f;

}
