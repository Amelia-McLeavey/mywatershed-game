using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightmapGenerator : MonoBehaviour
{
    public static float[,] s_Heightmap;

    [HideInInspector]
    public static int callCount = 0;

    [SerializeField]
    private float m_maxWaterTileHeight;

    private int m_rows;
    private int m_columns;

    private float[] xHeights;

    public void SetHeights(int rows, int columns, int seed)
    {
        Random.InitState(seed);
        m_rows = rows;
        m_columns = columns;
        s_Heightmap = new float[m_rows, m_columns];
        xHeights = new float[m_rows];

        SetWaterHeights();
    }

    public void SetLandHeights(List<Vector2> definedNeighbours)
    {
        callCount++;

        foreach (Vector2 definedNeighbour in definedNeighbours)
        {
            // Check to make sure the index is within the bounds of the 2D array
            if (definedNeighbour.x > -1 && definedNeighbour.x < m_rows && definedNeighbour.y > -1 && definedNeighbour.y < m_columns)
            {
                // Set the land height using the list of max xHeights increased by callCount
                s_Heightmap[(int)definedNeighbour.x, (int)definedNeighbour.y] = xHeights[(int)definedNeighbour.x] + callCount;
            }
        }
    }

    private void SetWaterHeights()
    {
        for (int x = 0; x < m_rows; x++)
        {
            // Define decriment value so that height gradually gets smaller as x increases
            float minDecriment = x * 0.30f;
            float maxDecriment = x * 0.32f;
            float minHeight = m_maxWaterTileHeight - maxDecriment;
            float maxHeight = m_maxWaterTileHeight - minDecriment;

            // Store the calculated max height in a list to be used to set land heights
            xHeights[x] = maxHeight;

            for (int y = 0; y < m_columns; y++)
            {
                if (WaterGenerator.s_WaterTiles.ContainsKey(new Vector2(x, y)))
                {
                    // Set a random height
                    s_Heightmap[x, y] = Random.Range(minHeight, maxHeight);
                }
                else
                {
                    // Otherwise set a default height
                    s_Heightmap[x, y] = 1f;
                }
            }
        }
    }
}
