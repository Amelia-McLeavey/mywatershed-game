using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType type;

    [SerializeField]
    private Material mat0;
    [SerializeField]
    private Material mat1;
    [SerializeField]
    private Material mat2;
    [SerializeField]
    private Material mat3;
    [SerializeField]
    private Material mat4;


    public void SetTypeComponents()
    {
        if ((int)type == 0)
        {
            GetComponent<MeshRenderer>().material = mat0;
        }
        else if ((int)type == 1)
        {
            GetComponent<MeshRenderer>().material = mat1;
        }
        else if ((int)type == 2)
        {
            GetComponent<MeshRenderer>().material = mat2;
        } 
        else if ((int)type == 3)
        {
            GetComponent<MeshRenderer>().material = mat3;
        }
        else if ((int)type == 4)
        {
            GetComponent<MeshRenderer>().material = mat4;
        }
    }
}
