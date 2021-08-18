using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourUtility : MonoBehaviour
{
    public static List<Vector2> GetNeighbours(Vector2 baseIndex)
    {
        // Indexes for each of a tile's 6 neighbours
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
}
