using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeController : MonoBehaviour
{
    [SerializeField]
    private WorldGenerator worldGeneratorScript;

    public void GenerateWorldOnClick()
    {
        worldGeneratorScript.GenerateWorld();
    }

    public void IncreaseSeedValue()
    {
        worldGeneratorScript.seed++;
    }

    public void DecreaseSeedValue()
    {
        worldGeneratorScript.seed--;
    }
}
