using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeController : MonoBehaviour
{
    [SerializeField]
    private WorldGenerator worldGeneratorScript;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void GenerateWorldOnClick()
    {
        worldGeneratorScript.GenerateWorld();
    }
}
