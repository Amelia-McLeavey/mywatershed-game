using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaseType { None, Land, Water }

// LAND TYPES
public enum LandClassType { None, Urban, Rural, Natural }
public enum UrbanFamilyType { None, Infrastructure, Residential, Recreational }


// WATER TYPES
public enum WaterClassType { None, Natural, Human }
public enum WaterFamilyType { None, Static, Dynamic }

// PHYSICAL TYPES
public enum PhysicalType 
{ 
    None, 
    Agriculture, 
    BeachBluff, 
    Cemetary, 
    Commercial,
    EngineeredReservoir, 
    EngineeredStream, 
    EstateResidential,
    Forest,
    GolfCourse,
    HighDensity,
    Highway,
    Industrial,
    Institutional,
    LowMidDensity,
    Meadow,
    NaturalReservoir,
    NaturalStream,
    RecreationCentreSpace,
    Successional,
    UrbanOpenSpace,
    Vacant,
    Wetland
}

public class TileTypeAllocator : MonoBehaviour
{
    public TileManager m_TileManager;

    public static Dictionary<Vector2, UrbanFamilyType> s_LandFamilyTiles = new Dictionary<Vector2, UrbanFamilyType>();

    [SerializeField]
    private int m_segments;
    [SerializeField]
    private int m_minimumArea;
    [SerializeField]
    private int m_maxForestDepthAlongRiver = 4;

    private List<Vector2> m_startingForestIndicies = new List<Vector2>();

    private Vector2[] m_points;


    public void AllocateTypes(int seed, int rows, int columns)
    {
        Random.InitState(seed);

        AllocateNaturalStreamType();
        AllocateForestTypeAlongRiver(rows, columns);
        //SelectPoints();
        //IdentifyPoints();
        //AllocateRemainingTileTypes(rows, columns);
    }

    public void InitializeForestTiles(List<Vector2> indices)
    {
        foreach (Vector2 index in indices)
        {
            m_startingForestIndicies.Add(index);
        }
    }

    private void AllocateNaturalStreamType()
    {
        foreach (KeyValuePair<Vector2, GameObject> tile in WorldGenerator.s_TilesDictonary)
        {
            Tile tileScript = tile.Value.GetComponent<Tile>();

            if (tileScript.m_Basetype == BaseType.Water)
            {
                Material material = m_TileManager.ReturnTileType(PhysicalType.NaturalStream);
                tileScript.SetMaterial(material);
                tileScript.m_PhysicalType = PhysicalType.NaturalStream;
            }
        }
    }

    private void AllocateForestTypeAlongRiver(int rows, int columns)
    {
        int _rows = rows;

        foreach (Vector2 index in m_startingForestIndicies)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(index, out GameObject value))
            {
                Material material = m_TileManager.ReturnTileType(PhysicalType.Forest);
                Tile tileScript = value.GetComponent<Tile>();
                tileScript.m_PhysicalType = PhysicalType.Forest;
                tileScript.SetMaterial(material);
            }
        }

        for (int i = 0; i < m_maxForestDepthAlongRiver; i++)
        {
            Vector2[] forestIndicies = new Vector2[m_startingForestIndicies.Count];
            m_startingForestIndicies.CopyTo(forestIndicies);
            m_startingForestIndicies.Clear();

            foreach (Vector2 index in forestIndicies)
            {
                if (index.x < _rows * 0.30f)
                {
                    // Find all the un-typed neighbours of forest tiles
                    List<Vector2> neighbourIndices = NeighbourUtility.GetNeighbours(index);

                    foreach (Vector2 neighbourIndex in neighbourIndices)
                    {
                        if (WorldGenerator.s_TilesDictonary.TryGetValue(neighbourIndex, out GameObject value))
                        {
                            if (neighbourIndex.x > -1 && neighbourIndex.x < rows && neighbourIndex.y > -1 && neighbourIndex.y < columns)
                            {
                                if (value.GetComponent<Tile>().m_PhysicalType == PhysicalType.None)
                                {
                                    Material material = m_TileManager.ReturnTileType(PhysicalType.Forest);
                                    Tile tileScript = value.GetComponent<Tile>();
                                    tileScript.m_PhysicalType = PhysicalType.Forest;
                                    tileScript.SetMaterial(material);

                                    m_startingForestIndicies.Add(neighbourIndex);
                                }
                            }
                        }
                    }
                } 
            }
            _rows /= 2;
        }
        m_startingForestIndicies.Clear();
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
            int randomType = Random.Range(1, 4);

            if (WorldGenerator.s_TilesDictonary.TryGetValue(new Vector2(point.x, point.y), out GameObject value))
            {
                //value.GetComponent<Tile>().SetLandFamilyType(randomType);
            }
        }
    }



    private void AllocateRemainingTileTypes(int rows, int columns)
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y > columns; y++)
            {

            }
        }
    }
}
