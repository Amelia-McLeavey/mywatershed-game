using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGenerator : MonoBehaviour
{
    public static float[,] s_LandHeightmap;

    public static Dictionary<Vector2, BaseType> s_LandTiles = new Dictionary<Vector2, BaseType>();

    private int m_rows;
    private int m_columns;

    private List<Vector2> m_definedNeighbourIndices = new List<Vector2>();

    public void CreateLand(int rows, int columns, int seed)
    {
        // INITIALIZATION
        Random.InitState(seed);
        m_rows = rows;
        m_columns = columns;
        m_definedNeighbourIndices.Clear();

        // FIND NEIGHBOURS OF WATER
        FindNeighboursOfWater();
        GetComponent<HeightmapGenerator>().SetLandHeights(m_definedNeighbourIndices);

        // Store the Water Neighours in a list on TileTypeAllocator for later use.
        GetComponent<TileTypeAllocator>().InitializeForestTiles(m_definedNeighbourIndices);

        // FIND NEIGHBOURS OF LAND
        while (WorldGenerator.s_UndefinedTiles.Count > 0)
        {
            FindNeighboursOfLand();
            GetComponent<HeightmapGenerator>().SetLandHeights(m_definedNeighbourIndices);
        }
        HeightmapGenerator.callCount = 0;
    }

    private void FindNeighboursOfWater()
    {
        // Iterate through the entire 2D array
        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                // If the current index can be found in the dictonary of water tiles then find any neighbours of it that have yet to be defined.
                if (WaterGenerator.s_WaterTiles.ContainsKey(new Vector2(x, y)))
                {
                    FindUndefinedNeighbours(new Vector2(x,y));
                }
            }
        }
    }

    private void FindNeighboursOfLand()
    {
        // INITIALIZATION
        // Copy the list to a new one so that we can temporarily preserve it
        Vector2[] definedLandIndexes = new Vector2[m_definedNeighbourIndices.Count];
        m_definedNeighbourIndices.CopyTo(definedLandIndexes);
        m_definedNeighbourIndices.Clear();

        foreach (Vector2 definedLandIndex in definedLandIndexes)
        {
            FindUndefinedNeighbours(definedLandIndex);
        }
    }

    private void FindUndefinedNeighbours(Vector2 index)
    {
        // Find all neighbours
        List<Vector2> neighbourIndices = NeighbourUtility.FindAllNeighbours(index);

        foreach (Vector2 neighbourIndex in neighbourIndices)
        {
            // Check if the tile has been not been assigned a BaseType
            if (!WaterGenerator.s_WaterTiles.ContainsKey(neighbourIndex) && !s_LandTiles.ContainsKey(neighbourIndex))
            {
                // If the tile is within the 2D array bounds
                if (neighbourIndex.x > -1 && neighbourIndex.x < m_rows && neighbourIndex.y > -1 && neighbourIndex.y < m_columns)
                {
                    // Add the index to the list of Land Tiles, type appropriately, and remove from the list of undefined tiles
                    s_LandTiles.Add(neighbourIndex, BaseType.Land);
                    WorldGenerator.s_UndefinedTiles.Remove(neighbourIndex);
                    // Add this index to the list of defined neighbours, to be used in next iteration
                    m_definedNeighbourIndices.Add(neighbourIndex);
                }
            }
        } 
    }
}
