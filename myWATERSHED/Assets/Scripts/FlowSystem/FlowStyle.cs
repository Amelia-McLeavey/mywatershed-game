using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class that defines the methods called on different flow styles.
/// </summary>
// This abstract class is only defining the methods that we want to be able to call on different flow styles

public abstract class FlowStyle
{
    public List<KeyValuePair<Vector2, GameObject>> m_cachedUsableTiles = new List<KeyValuePair<Vector2, GameObject>>();

    public abstract bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging);
    public abstract void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging);
    public abstract void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging);
    public abstract void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging);

    public void CacheTiles(int rows, int columns, BaseType baseType)
    {
        for (int x = rows; x > 0; x--)
        {
            for (int y = columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    // Make sure we don't call the wrong flow
                    if (value.GetComponent<Tile>().m_Basetype == baseType)
                    {
                        m_cachedUsableTiles.Add(new KeyValuePair<Vector2, GameObject>(tileIndex, value));
                    }
                }
            }
        }
    }
}
