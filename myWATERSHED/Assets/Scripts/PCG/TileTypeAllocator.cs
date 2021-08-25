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
    private int m_minimumGolfArea;
    [Range(0, 5)]
    [SerializeField]
    private int m_maxForestDepthAlongRiver = 4;
    [Range(6, 10)]
    [SerializeField]
    private int m_golfCourseCount = 6;

    private float m_infrastructurePercent = 0.09f;
    private float m_residentialPercent = 0.13f;
    private float m_ruralPercent = 0.43f;
    private float m_naturalPercent = 0.22f;

    private List<Vector2> m_startingForestIndicies = new List<Vector2>();

    public void AllocateTypes(int seed, int rows, int columns)
    {
        Random.InitState(seed);

        AllocateNaturalStreamType();
        AllocateForestTypeAlongRiver(rows, columns);
        AllocateGolfType();
        AllocateTypesWithLargePercentageCover(rows);
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
        Material material = m_TileManager.ReturnTileType(PhysicalType.NaturalStream);

        foreach (KeyValuePair<Vector2, GameObject> tile in WorldGenerator.s_TilesDictonary)
        {
            Tile tileScript = tile.Value.GetComponent<Tile>();

            if (tileScript.m_Basetype == BaseType.Water)
            {
                tileScript.SetMaterial(material);
                tileScript.m_PhysicalType = PhysicalType.NaturalStream;
            }
        }
    }

    private void AllocateForestTypeAlongRiver(int rows, int columns)
    {
        int _rows = rows;
        Material material = m_TileManager.ReturnTileType(PhysicalType.Forest);

        foreach (Vector2 index in m_startingForestIndicies)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(index, out GameObject value))
            {
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

    private void AllocateGolfType()
    {
        // DEFINE TILE SET
        List<Vector2> tileSet = new List<Vector2>();

        foreach (KeyValuePair<Vector2, GameObject> tile in WorldGenerator.s_TilesDictonary)
        {
            if (tile.Value.GetComponent<Tile>().m_PhysicalType == PhysicalType.None)
            {
                if (WorldGenerator.s_TilesDictonary.TryGetValue(new Vector2(tile.Key.x, tile.Key.y - 4), out GameObject valueA))
                {
                    if (valueA.GetComponent<Tile>().m_PhysicalType == PhysicalType.NaturalStream ||
                        valueA.GetComponent<Tile>().m_PhysicalType == PhysicalType.Forest)
                    {
                        tileSet.Add(tile.Key);
                    }
                }
                else if (WorldGenerator.s_TilesDictonary.TryGetValue(new Vector2(tile.Key.x, tile.Key.y + 4), out GameObject valueB))
                {
                    if (valueB.GetComponent<Tile>().m_PhysicalType == PhysicalType.NaturalStream ||
                        valueB.GetComponent<Tile>().m_PhysicalType == PhysicalType.Forest)
                    {
                        tileSet.Add(tile.Key);
                    }
                }
            }
        }
        //Debug.Log($"TTA Vector2 tileSet COUNT = {tileSet.Count}");
        // SELECT INITIAL TILES
        Vector2[] initialTiles = new Vector2[m_golfCourseCount];

        for (int i = 0; i < m_golfCourseCount; i++)
        {
            if (tileSet.Count < 1) { break; } else
            {
                int index = Random.Range(0, tileSet.Count - 1);
                initialTiles[i] = tileSet[index];

                for (int x = (int)initialTiles[i].x - (m_minimumGolfArea / 2); x < (int)initialTiles[i].x + (m_minimumGolfArea / 2); x++)
                {
                    for (int y = (int)initialTiles[i].y - (m_minimumGolfArea / 2); y < (int)initialTiles[i].y + (m_minimumGolfArea / 2); y++)
                    {
                        if (tileSet.Contains(new Vector2(x, y)))
                        {
                            tileSet.Remove(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        //Debug.Log($"TTA Vector2 initialTiles COUNT = {initialTiles.Length}");
        // TYPE TILES
        Material material = m_TileManager.ReturnTileType(PhysicalType.GolfCourse);

        foreach (Vector2 tile in initialTiles)
        {
            // Type initial tiles
            if (WorldGenerator.s_TilesDictonary.TryGetValue(tile, out GameObject value))
            {
                Tile tileScript = value.GetComponent<Tile>();
                tileScript.m_PhysicalType = PhysicalType.GolfCourse;
                tileScript.SetMaterial(material);
            }
            // Type surrounding tiles
            List<Vector2> neighbourIndices = NeighbourUtility.GetNeighbours(tile);

            foreach (Vector2 neighbourIndex in neighbourIndices)
            {
                if (WorldGenerator.s_TilesDictonary.TryGetValue(neighbourIndex, out GameObject nValue))
                {
                    Tile tileScript = nValue.GetComponent<Tile>();
                    if (tileScript.m_PhysicalType == PhysicalType.None || tileScript.m_PhysicalType == PhysicalType.Forest)
                    {
                        tileScript.m_PhysicalType = PhysicalType.GolfCourse;
                        tileScript.SetMaterial(material);
                    } 
                }
            }
            // Type extra tiles
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, neighbourIndices.Count);

                List<Vector2> extraNeighbourIndicies = NeighbourUtility.GetNeighbours(neighbourIndices[index]);

                foreach (Vector2 extraNeighbourIndex in extraNeighbourIndicies)
                {
                    if (WorldGenerator.s_TilesDictonary.TryGetValue(extraNeighbourIndex, out GameObject eValue))
                    {
                        Tile tileScript = eValue.GetComponent<Tile>();
                        if (tileScript.m_PhysicalType == PhysicalType.None || tileScript.m_PhysicalType == PhysicalType.Forest)
                        {
                            tileScript.m_PhysicalType = PhysicalType.GolfCourse;
                            tileScript.SetMaterial(material);
                        }
                    }
                }
                neighbourIndices.RemoveAt(index);
            }
        }
    }

    private void AllocateTypesWithLargePercentageCover(in int rows)
    {
        // DEFINE TILE SET
        List<Vector2> tileSet = new List<Vector2>();

        foreach (KeyValuePair<Vector2, GameObject> tile in WorldGenerator.s_TilesDictonary)
        {
            if (tile.Value.GetComponent<Tile>().m_PhysicalType == PhysicalType.None)
            {
                tileSet.Add(tile.Key);
            }
        }

        // SELECT INITIAL TILES
        Vector2[] initialTiles = new Vector2[m_segments];

        for (int i = 0; i < m_segments; i++)
        {
            if (tileSet.Count < 1) { break; } else
            {
                int index = Random.Range(0, tileSet.Count);
                initialTiles[i] = tileSet[index];

                for (int x = (int)initialTiles[i].x - (m_minimumArea / 2); x < (int)initialTiles[i].x + (m_minimumArea / 2); x++)
                {
                    for (int y = (int)initialTiles[i].y - (m_minimumArea / 2); y < (int)initialTiles[i].y + (m_minimumArea / 2); y++)
                    {
                        if (tileSet.Contains(new Vector2(x, y)))
                        {
                            tileSet.Remove(new Vector2(x, y));
                        }
                    }
                }
            }
        }

        // TYPE TILES
        // Set materials
        Material infrastructureMaterial = m_TileManager.ReturnTileType(PhysicalType.Commercial);
        Material residentialMaterial = m_TileManager.ReturnTileType(PhysicalType.LowMidDensity);
        Material ruralMaterial = m_TileManager.ReturnTileType(PhysicalType.Agriculture);
        Material naturalMaterial = m_TileManager.ReturnTileType(PhysicalType.Successional);

        // Determine number of initial tiles per type
        int infrastructureTileCount = (int)(m_segments * m_infrastructurePercent);
        int residentialTileCount = (int)(m_segments * m_residentialPercent);
        int ruralTileCount = (int)(m_segments * m_ruralPercent);
        int naturalTileCount = (int)(m_segments * m_naturalPercent);

        // Store data
        int[] typeAmounts = new int[] { infrastructureTileCount, residentialTileCount, ruralTileCount, naturalTileCount };
        List<int> tileHat = new List<int>();

        // Populate the list
        while (tileHat.Count < m_segments)
        {
            for (int i = 0; i < typeAmounts.Length; i++)
            {
                for (int a = 0; a < typeAmounts[i]; a++)
                {
                    tileHat.Add(i);
                }
            }
        }

        float infrastructureChance;
        float residentialChance;
        float ruralChance;
        float naturalChance;

        foreach (Vector2 tile in initialTiles)
        {
            if (tile.x < rows / 2)
            {
                infrastructureChance = 0.01f;
                residentialChance = 0.01f;
                ruralChance = 0.55f;
                naturalChance = 0.40f;
            } 
            else if (tile.x > rows / 2)
            {
                infrastructureChance = 0.01f;
                residentialChance = 0.01f;
                ruralChance = 0.55f;
                naturalChance = 0.40f;
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
