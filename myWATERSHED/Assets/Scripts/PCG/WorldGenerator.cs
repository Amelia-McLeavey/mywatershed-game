using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WorldSize { ONE, XXS, XS, S, M, L, XL, XXL, XXXL }

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject tile;

    [SerializeField]
    private WorldSize worldSize;

    [Range(0.05f, 0.95f)]
    [SerializeField]
    private float totalWaterPercent;
    [Range(0f, 1f)]
    [SerializeField]
    private float shorlinePercent;
    [Range(0f, 1f)]
    [SerializeField]
    private float deepWaterPercent;
    [Range(0.1f, 0.9f)]
    [SerializeField]
    private float humanPopulation;
    [Range(0,5)]
    [SerializeField]
    private int numberOfCreeks;

    [SerializeField]
    private float randomnessMagnitude = 1.5f;
    [Range(0.01f, 0.99f)]
    [SerializeField]
    private float magnitudeReductionRate = 0.80f;

    [SerializeField]
    private int seed = 0;

    private float size = 0;

    private int columns = 0;
    private int rows = 0;

    private Vector2 tileStep = new Vector2(0.5f, 0.87f);

    private Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();

    private Transform worldHolder;

    public void GenerateWorld()
    {
        DetermineWorldSize();

        HeightmapGenerator.CreateHeightmap((int)worldSize, seed, randomnessMagnitude, magnitudeReductionRate);
        TileTypeAllocator.AllocateTileTypes((int)size, numberOfCreeks, totalWaterPercent, shorlinePercent, deepWaterPercent);

        ResetWorld();
        WorldSetup();
    }

    private void DetermineWorldSize()
    {
        if (worldSize == WorldSize.ONE)           { size = 1.0f; } // DO NOT REMOVE
        else if (worldSize == WorldSize.XXS)       { size = 3.0f; } // DO NOT REMOVE
        else if (worldSize == WorldSize.XS)        { size = 5.0f; }
        else if (worldSize == WorldSize.S)        { size = 9.0f; }
        else if (worldSize == WorldSize.M)        { size = 17.0f; }
        else if (worldSize == WorldSize.L)       { size = 33.0f; }
        else if (worldSize == WorldSize.XL)      { size = 65.0f; }
        else if (worldSize == WorldSize.XXL)     { size = 129.0f; }
        else if (worldSize == WorldSize.XXXL)    { size = 257.0f; }
    }

    private void ResetWorld()
    {
        if (null != worldHolder)
        { Destroy(worldHolder.gameObject); }
        tiles.Clear();
    }

    public void WorldSetup()
    {
        columns = (int)size;
        rows = (int)size;
        
        worldHolder = new GameObject("World").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
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
                cloneTile.GetComponent<Tile>().baseType = TileTypeAllocator.BaseTypeMap[x, y];
                cloneTile.GetComponent<Tile>().wClass = TileTypeAllocator.WaterClassMap[x, y];
                cloneTile.GetComponent<Tile>().SetTypeComponents();
                cloneTile.transform.localScale = new Vector3(1f, HeightmapGenerator.Heightmap[x, y], 1f);
                cloneTile.transform.position = new Vector3(position.x, HeightmapGenerator.Heightmap[x, y] / 4, position.z);

                cloneTile.transform.SetParent(worldHolder);

                //yield return new WaitForSeconds(0.002f);
            }
        }
    }
}
