using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int m_Seed;
    
    public static Dictionary<Vector2, BaseType> s_UndefinedTiles = new Dictionary<Vector2, BaseType>();

    public delegate void WorldGenerationComplete(int rowSize, int columnSize);
    public static event WorldGenerationComplete OnWorldGenerationComplete;

    [SerializeField]
    public int m_rows = 0;
    [SerializeField]
    public int m_columns = 0;

    [SerializeField]
    private GameObject m_tile;

    private Vector2 tileStep = new Vector2(0.5f, 0.87f);

    private Transform worldHolder;

    public void GenerateWorld()
    {
        ResetWorld();

        GetComponent<WaterGenerator>().CreateWater(m_rows, m_columns, m_Seed);
        GetComponent<HeightmapGenerator>().SetHeights(m_rows, m_columns, m_Seed);
        GetComponent<LandGenerator>().CreateLand(m_rows, m_columns, m_Seed);

        WorldSetup();

        GetComponent<TileTypeAllocator>().AllocateTypes(m_Seed, m_rows, m_columns);

        OnWorldGenerationComplete?.Invoke(m_rows, m_columns);
    }

    private void ResetWorld()
    {
        // DESTROY GAMEOBJECTS
        if (null != worldHolder)
        { Destroy(worldHolder.gameObject); }

        // CLEAR DATA
        s_UndefinedTiles.Clear();
        TileManager.s_TilesDictonary.Clear();
        WaterGenerator.s_WaterTiles.Clear();
        LandGenerator.s_LandTiles.Clear();

        // RESETS
        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                s_UndefinedTiles.Add(new Vector2(x, y), BaseType.None);
            }
        }
    }

    private void WorldSetup()
    {
        // Create an object to hold the tiles
        worldHolder = new GameObject("World").transform;

        // Iterate through 2 dimensions
        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                // Initialization
                BaseType type = BaseType.None;
                Vector3 position;
                GameObject cloneTile;

                // Adjust the position of the cloneTile to compliment the hex tiles
                if (y % 2 == 0)
                { position = new Vector3(x, 0f, y * tileStep.y); }
                else
                { position = new Vector3(x + tileStep.x, 0f, y * tileStep.y); }

                // Instantiate the tile
                TileManager.s_TilesDictonary.Add(new Vector2(x, y), cloneTile = Instantiate(m_tile, position, Quaternion.Euler(-90f, 0f, 0f)));

                // Set the tile height.. Z because of the orientation of the asset
                cloneTile.transform.localScale = new Vector3(1f, 1f, HeightmapGenerator.s_Heightmap[x, y]);
                
                // Set a reference for the tile's Index on Tile
                Tile tileScript = cloneTile.GetComponent<Tile>();
                tileScript.m_TileIndex = new Vector2(x, y);

                // Set the tile type
                if (WaterGenerator.s_WaterTiles.TryGetValue(new Vector2(x,y), out BaseType waterType)) 
                {
                    type = waterType;
                }
                else if (LandGenerator.s_LandTiles.TryGetValue(new Vector2(x, y), out BaseType landType))
                {
                    type = landType;
                }
                tileScript.m_Basetype = type;

                // Parent
                cloneTile.transform.SetParent(worldHolder);
            }
        }
    }
}
