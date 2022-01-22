using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileVariableDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image icon;
    [SerializeField] private Slider slider;

    public VariableClass variableClassToRead;
    public void SetVariableClass(VariableClass varClass)
    {
        variableClassToRead = varClass;
    }

    private void Update()
    {
        if (variableClassToRead != null)
        {
            nameText.text = variableClassToRead.variableName;
        }
    }

}
