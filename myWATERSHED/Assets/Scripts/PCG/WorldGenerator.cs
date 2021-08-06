using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldSize { ONE, XXS, XS, S, M, L, XL, XXL, XXXL }

public enum BaseType { Land, Water }

public class WorldGenerator : MonoBehaviour
{
    public int Seed;

    public static Dictionary<Vector2, GameObject> s_Tiles = new Dictionary<Vector2, GameObject>();

    public delegate void WorldGenerationComplete(int size);
    public static event WorldGenerationComplete OnWorldGenerationComplete;

    [HideInInspector]
    public float Size = 0;

    [SerializeField]
    private GameObject tile;

    [SerializeField]
    private WorldSize worldSize;

    private int columns = 0;
    private int rows = 0;

    private Vector2 tileStep = new Vector2(0.5f, 0.87f);

    private Transform worldHolder;

    public void GenerateWorld()
    {
        DetermineWorldSize();

        GetComponent<WaterGenerator>().CreateWater((int)Size, Seed);

        ResetWorld();
        WorldSetup();

        OnWorldGenerationComplete?.Invoke((int)Size);
    }

    private void DetermineWorldSize()
    {
        if (worldSize == WorldSize.ONE)       { Size = 1.0f; } // DO NOT REMOVE
        else if (worldSize == WorldSize.XXS)  { Size = 3.0f; } // DO NOT REMOVE
        else if (worldSize == WorldSize.XS)   { Size = 5.0f; }
        else if (worldSize == WorldSize.S)    { Size = 9.0f; }
        else if (worldSize == WorldSize.M)    { Size = 17.0f; }
        else if (worldSize == WorldSize.L)    { Size = 33.0f; }
        else if (worldSize == WorldSize.XL)   { Size = 65.0f; }
        else if (worldSize == WorldSize.XXL)  { Size = 129.0f; }
        else if (worldSize == WorldSize.XXXL) { Size = 257.0f; }
    }

    private void ResetWorld()
    {
        if (null != worldHolder)
        { Destroy(worldHolder.gameObject); }
        s_Tiles.Clear();
    }

    private void WorldSetup()
    {
        rows = (int)Size;
        columns = (int)Size;

        worldHolder = new GameObject("World").transform;

         
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                GameObject cloneTile;
                Vector3 position;

                // Adjust the position to compliment the hex tiles
                if (y % 2 == 0)
                { position = new Vector3(x, 0f, y * tileStep.y); }
                else
                { position = new Vector3(x + tileStep.x, 0f, y * tileStep.y); }

                // Instantiate the tile
                s_Tiles.Add(new Vector2(x, y), cloneTile = Instantiate(tile, position, Quaternion.Euler(-90f, 0f, 0f)));

                // Set the tile height.. Z because of the orientation of the asset
                cloneTile.transform.localScale = new Vector3(1f, 1f, WaterGenerator.StreamHeightmap[x, y]);
                
                Tile tileScript = cloneTile.GetComponent<Tile>();
                tileScript.m_TileIndex = new Vector2(x, y);

                if (WaterGenerator.StreamHeightmap[x,y] > 1)
                {
                    tileScript.SetBaseType(BaseType.Water);
                }
                else
                {
                    tileScript.SetBaseType(BaseType.Land);
                }


                cloneTile.transform.SetParent(worldHolder);
            }
        }
    }
}
