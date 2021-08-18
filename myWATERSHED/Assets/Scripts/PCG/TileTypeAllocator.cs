using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaseType { None, Land, Water }
public enum ClassType { None, Human, Nature }
public enum LandFamilyType { None, Industrial, Infrastructure, Residential, Agriculture, Recreational }
public enum WaterFamilyType { None, Static, Dynamic }

public class TileTypeAllocator : MonoBehaviour
{
    public static Dictionary<Vector2, LandFamilyType> s_LandFamilyTiles = new Dictionary<Vector2, LandFamilyType>();

    [SerializeField]
    private int m_segments;
    [SerializeField]
    private int m_minimumArea;

    private Vector2[] m_points;

    public void AllocateLand(int seed, int rows, int columns)
    {
        Random.InitState(seed);

        SelectPoints();
        IdentifyPoints();
        SetAllTileTypes(rows, columns);
    }

    private void SelectPoints()
    {
        m_points = new Vector2[m_segments];
        Vector2[] tiles = new Vector2[LandGenerator.s_LandTiles.Count];
        List<Vector2> tilesList = new List<Vector2>();

        LandGenerator.s_LandTiles.Keys.CopyTo(tiles, 0);

        foreach (Vector2 tile in tiles)
        {
            tilesList.Add(tile);
        }

        for (int i = 0; i < m_segments; i++)
        {
            if (tilesList.Count < 1) { break; } else
            {
                int index = Random.Range(0, tilesList.Count - 1);
                m_points[i] = tilesList[index];

                for (int x = (int)m_points[i].x - (m_minimumArea / 2); x < (int)m_points[i].x + (m_minimumArea / 2); x++)
                {
                    for (int y = (int)m_points[i].y - (m_minimumArea / 2); y < (int)m_points[i].y + (m_minimumArea / 2); y++)
                    {
                        if (tilesList.Contains(new Vector2(x, y)))
                        {
                            tilesList.Remove(new Vector2(x, y));
                        }
                    }
                }
            }
        }
    }

    private void IdentifyPoints()
    {
        foreach (Vector2 point in m_points)
        {
            int randomType = Random.Range(1, 6);

            if (WorldGenerator.s_TilesDictonary.TryGetValue(new Vector2(point.x, point.y), out GameObject value))
            {
                value.GetComponent<Tile>().SetLandFamilyType(randomType);
            }
        }
    }

    private void SetAllTileTypes(int rows, int columns)
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y > columns; y++)
            {

            }
        }
    }
}
