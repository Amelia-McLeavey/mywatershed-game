using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BaseTileType type;

    [SerializeField]
    private Material mat0;
    [SerializeField]
    private Material mat1;

    public void SetTypeComponents()
    {
        if ((int)type == 0)
        {
            GetComponentInChildren<MeshRenderer>().material = mat0;
        }
        else if ((int)type == 1)
        {
            GetComponentInChildren<MeshRenderer>().material = mat1;
        }
    }
}
