using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldSize { ONE, XXS, XS, S, M, L, XL, XXL, XXXL }

public class WorldGenerator : MonoBehaviour
{
    public int seed;

    [SerializeField]
    private GameObject tile;

    [SerializeField]
    private WorldSize worldSize;

    private float size = 0;

    private int columns = 0;
    private int rows = 0;

    private Vector2 tileStep = new Vector2(0.5f, 0.87f);

    private Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();

    private Transform worldHolder;

    public void GenerateWorld()
    {
        DetermineWorldSize();

        GetComponent<WaterGenerator>().CreateWater((int)size, seed);

        ResetWorld();
        WorldSetup();
    }

    private void DetermineWorldSize()
    {
        if (worldSize == WorldSize.ONE)       { size = 1.0f; } // DO NOT REMOVE
        else if (worldSize == WorldSize.XXS)  { size = 3.0f; } // DO NOT REMOVE
        else if (worldSize == WorldSize.XS)   { size = 5.0f; }
        else if (worldSize == WorldSize.S)    { size = 9.0f; }
        else if (worldSize == WorldSize.M)    { size = 17.0f; }
        else if (worldSize == WorldSize.L)    { size = 33.0f; }
        else if (worldSize == WorldSize.XL)   { size = 65.0f; }
        else if (worldSize == WorldSize.XXL)  { size = 129.0f; }
        else if (worldSize == WorldSize.XXXL) { size = 257.0f; }
    }

    private void ResetWorld()
    {
        if (null != worldHolder)
        { Destroy(worldHolder.gameObject); }
        tiles.Clear();
    }

    private void WorldSetup()
    {
        rows = (int)size;
        columns = (int)size;

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
                tiles.Add(new Vector2(x, y), cloneTile = Instantiate(tile, position, Quaternion.identity));

                // Set the tile type and height 
                cloneTile.transform.localScale = new Vector3(1f, WaterGenerator.StreamHeightmap[x, y], 1f);

                cloneTile.transform.SetParent(worldHolder);
            }
        }
    }
}
