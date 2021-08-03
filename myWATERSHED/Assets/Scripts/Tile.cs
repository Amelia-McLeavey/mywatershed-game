using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //public TileWaterType baseType;
    public WaterTileClass wClass;

    [SerializeField]
    private Material mat0;
    [SerializeField]
    private Material mat1;
    [SerializeField]
    private Material mat2;
    [SerializeField]
    private Material mat3;
    [SerializeField]
    private Material flagMaterial;

    public void Flag()
    {
        GetComponentInChildren<MeshRenderer>().material = flagMaterial;
    }

    public void SetTypeComponents()
    {
        //if ((int)baseType == 0)
        //{
        //    GetComponentInChildren<MeshRenderer>().material = mat1;
        //}
        //else if ((int)baseType == 1)
        //{
        //    GetComponentInChildren<MeshRenderer>().material = mat0;
        //}

        if ((int)wClass == 1) // Shallow
        {
            GetComponentInChildren<MeshRenderer>().material = mat2;
        }
        else if ((int)wClass == 2) // Medium
        {
            GetComponentInChildren<MeshRenderer>().material = mat0;
        }
        else if ((int)wClass == 3) // Deep
        {
            GetComponentInChildren<MeshRenderer>().material = mat3;
        }
        else // None
        {
            GetComponentInChildren<MeshRenderer>().material = mat1;
        }
    }
}
