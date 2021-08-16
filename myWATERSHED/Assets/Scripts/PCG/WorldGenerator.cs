using System.Collections.Generic;
using UnityEngine;

public enum BaseType { None, Land, Water }

public class WorldGenerator : MonoBehaviour
{
    public int m_Seed;

    public static Dictionary<Vector2, BaseType> s_UndefinedTiles = new Dictionary<Vector2, BaseType>();
    public static Dictionary<Vector2, GameObject> s_TilesDictonary = new Dictionary<Vector2, GameObject>();

    public delegate void WorldGenerationComplete(int rowSize, int columnSize);
    public static event WorldGenerationComplete OnWorldGenerationComplete;

    [SerializeField]
    private GameObject m_tile;

    [SerializeField]
    private int m_rows = 0;
    [SerializeField]
    private int m_columns = 0;


    private Vector2 tileStep = new Vector2(0.5f, 0.87f);

    private Transform worldHolder;

    public void GenerateWorld()
    {
        ResetWorld();

        GetComponent<WaterGenerator>().CreateWater(m_rows, m_columns, m_Seed);
        GetComponent<HeightmapGenerator>().SetHeights(m_rows, m_columns, m_Seed);
        GetComponent<LandGenerator>().CreateLand(m_rows, m_columns, m_Seed);


        WorldSetup();

        OnWorldGenerationComplete?.Invoke(m_rows, m_columns);
    }

    private void ResetWorld()
    {
        // DESTROY GAMEOBJECTS
        if (null != worldHolder)
        { Destroy(worldHolder.gameObject); }

        // CLEAR DATA
        s_UndefinedTiles.Clear();
        s_TilesDictonary.Clear();
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
        worldHolder = new GameObject("World").transform;

        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                BaseType type = BaseType.None;
                Vector3 position;
                GameObject cloneTile;

                // Adjust the position to compliment the hex tiles
                if (y % 2 == 0)
                { position = new Vector3(x, 0f, y * tileStep.y); }
                else
                { position = new Vector3(x + tileStep.x, 0f, y * tileStep.y); }

                // Instantiate the tile
                s_TilesDictonary.Add(new Vector2(x, y), cloneTile = Instantiate(m_tile, position, Quaternion.Euler(-90f, 0f, 0f)));

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
                tileScript.SetBaseType(type);

                // Parent
                cloneTile.transform.SetParent(worldHolder);
            }
        }
    }
}
