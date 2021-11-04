using System.Collections.Generic;
using UnityEngine;

public class NeighbourUtility : MonoBehaviour
{
    /// <summary>
    /// Finds each of a tile's 6 neighbour indices.
    /// </summary>
    /// <param name="baseIndex"></param>
    /// <returns> Vector 2 List </returns>
    public static List<Vector2> FindAllNeighbours(Vector2 baseIndex)
    {
        // Initialize indices for each of a tile's 6 neighbours.
        Vector2 tileUpIndex = new Vector2(baseIndex.x - 1, baseIndex.y);
        Vector2 tileDownIndex = new Vector2(baseIndex.x + 1, baseIndex.y);
        Vector2 tileUpLeftIndex;
        Vector2 tileUpRightIndex;
        Vector2 tileDownLeftIndex;
        Vector2 tileDownRightIndex;

        // If the Y of the index is an even number
        if (baseIndex.y % 2 == 0)
        {
            tileUpLeftIndex = new Vector2(baseIndex.x - 1, baseIndex.y - 1);
            tileUpRightIndex = new Vector2(baseIndex.x - 1, baseIndex.y + 1);
            tileDownLeftIndex = new Vector2(baseIndex.x, baseIndex.y - 1);
            tileDownRightIndex = new Vector2(baseIndex.x, baseIndex.y + 1);
        }
        // If the Y of the index is an odd number
        else
        {
            tileUpLeftIndex = new Vector2(baseIndex.x, baseIndex.y - 1);
            tileUpRightIndex = new Vector2(baseIndex.x, baseIndex.y + 1);
            tileDownLeftIndex = new Vector2(baseIndex.x + 1, baseIndex.y - 1);
            tileDownRightIndex = new Vector2(baseIndex.x + 1, baseIndex.y + 1);
        }

        // Store the indicies in a list
        List<Vector2> neighbourIndexes = new List<Vector2>
        {
            tileUpLeftIndex, //////////// 0
            tileUpIndex, //////////////// 1
            tileUpRightIndex, /////////// 2
            tileDownLeftIndex, ////////// 3
            tileDownIndex, ////////////// 4
            tileDownRightIndex ////////// 5
        };

        return neighbourIndexes;
    }

    public static List<GameObject> FindLowestNeighbours(List<Vector2> neighbourIndices) 
    {
        List<GameObject> receiverNeighbours = new List<GameObject>();

        List<Vector2> neighbourBelowIndices = new List<Vector2>
        {
            neighbourIndices[3], // 0
            neighbourIndices[4], // 1
            neighbourIndices[5]  // 2
        };

        foreach (Vector2 neighbourBelowIndex in neighbourBelowIndices)
        {
            if (TileManager.s_TilesDictonary.TryGetValue(neighbourBelowIndex, out GameObject neighbourBelow))
            {
                if (neighbourBelow.GetComponent<Tile>().m_Basetype == BaseType.Water)
                {
                    receiverNeighbours.Add(neighbourBelow);
                }
            }
        }

        return receiverNeighbours; 
    }

    public static List<GameObject> FindLowestNeighbour(List<Vector2> neighbourIndices, GameObject sourceTile)
    {
        List<GameObject> receiverNeighbours = new List<GameObject>();

        // Select the neighbour with the lowest height
        GameObject lowestNeighbour = sourceTile;

        foreach (Vector2 neighbourIndex in neighbourIndices)
        {
            if (TileManager.s_TilesDictonary.TryGetValue(neighbourIndex, out GameObject value))
            {
                if (value.transform.localScale.z < lowestNeighbour.transform.localScale.z)
                {
                    lowestNeighbour = value;
                }
            }
        }

        // Add the lowest neighbour as a receiver
        receiverNeighbours.Add(lowestNeighbour);

        return receiverNeighbours;
    }
}
