using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeAllocator : MonoBehaviour
{
    public TileManager m_TileManager;

    [SerializeField]
    private int m_segments;
    [SerializeField]
    private int m_minArea;
    [SerializeField]
    private int m_minGolfArea;
    [SerializeField]
    private int m_minCommericalArea;
    [SerializeField]
    private int m_minInstitutionalArea;
    [SerializeField]
    private int m_minVacantArea;
    [Range(0, 5)]
    [SerializeField]
    private int m_maxForestDepthAlongRiver = 4;
    [Range(6, 10)]
    [SerializeField]
    private int m_golfCourseCount = 6;
    [Range(2, 10)]
    [SerializeField]
    private int m_commercialSpaceCount = 6;
    [Range(2, 10)]
    [SerializeField]
    private int m_institutionCount = 6;
    [Range(2, 10)]
    [SerializeField]
    private int m_vacantSpaceCount = 6;

    private readonly float m_infrastructurePercent = 0.10f;
    private readonly float m_residentialPercent = 0.15f;
    private readonly float m_ruralPercent = 0.43f;
    private readonly float m_naturalPercent = 0.33f;

    private List<Vector2> m_startingForestIndicies = new List<Vector2>();

    public void AllocateTypes(int seed, int rows, int columns)
    {
        Random.InitState(seed);

        AllocateNaturalStreamType();
        AllocateForestTypeAlongRiver(rows, columns);
        AllocateGolfType();
        AllocateTypesWithLargePercentageCover(rows);
        AllocateInfrastructureTypes(rows);
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

        // SELECT INITIAL TILES
        Vector2[] initialPoints = SelectInitialTilePoints(tileSet, m_golfCourseCount, m_minGolfArea);

        // TYPE TILES
        Material material = m_TileManager.ReturnTileType(PhysicalType.GolfCourse);

        foreach (Vector2 initialPoint in initialPoints)
        {
            TypeInitialTiles(initialPoint, PhysicalType.GolfCourse, material);

            List<Vector2> neighbourIndices = TypeSurroundingTilesV1(initialPoint, PhysicalType.GolfCourse, material, PhysicalType.None, PhysicalType.Forest);
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
        List<Vector2> tileSet = DefineTileSet(PhysicalType.None);

        // SELECT INITIAL TILE POINTS
        Vector2[] initialPoints = SelectInitialTilePoints(tileSet, m_segments, m_minArea);

        // TYPE INITIAL TILES
        // Set Type 
        PhysicalType[] physicalTypes = new PhysicalType[]
        {
            PhysicalType.Industrial,
            PhysicalType.LowMidDensity,
            PhysicalType.Agriculture,
            PhysicalType.Successional
        };
        // Set materials
        Material[] materials = new Material[]
        {
            m_TileManager.ReturnTileType(PhysicalType.Industrial),
            m_TileManager.ReturnTileType(PhysicalType.LowMidDensity),
            m_TileManager.ReturnTileType(PhysicalType.Agriculture),
            m_TileManager.ReturnTileType(PhysicalType.Successional)
        };
        // Determine number of initial tiles per type
        int[] typeAmounts = new int[] 
        {
            (int)(m_segments * m_infrastructurePercent),
            (int)(m_segments * m_residentialPercent),
            (int)(m_segments * m_ruralPercent),
            (int)(m_segments * m_naturalPercent)
        };
        // Store data
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
        // Create a list for corresponding chances to type
        float[] typeChances = new float[4];

        List<GameObject> initialTiles = new List<GameObject>();

        foreach (Vector2 initialPoint in initialPoints)
        {
            // Determine percent split
            if (initialPoint.x < rows / 2)
            {
                typeChances[0] = 0.02f; // 2 %
                typeChances[1] = 0.04f; // 2 %
                typeChances[2] = 0.65f; // 56 %
                typeChances[3] = 1.00f; // 40 %
            } 
            else if (initialPoint.x > rows / 2)
            {
                typeChances[0] = 0.34f; // 34 %
                typeChances[1] = 0.98f; // 64 %
                typeChances[2] = 0.98f; // 1 %
                typeChances[3] = 1.00f; // 1 %
            }

            float rng = Random.Range(0f, 1f);
            float previousValue = 0;

            for (int i = 0; i < typeChances.Length; i++)
            {
                if (rng > previousValue && rng < typeChances[i])
                {
                    if (tileHat.Contains(i))
                    {
                        if (WorldGenerator.s_TilesDictonary.TryGetValue(initialPoint, out GameObject value))
                        {
                            value.GetComponent<Tile>().SetMaterial(materials[i]);
                            value.GetComponent<Tile>().m_PhysicalType = physicalTypes[i]; 
                            initialTiles.Add(value);
                            tileHat.Remove(i);
                            break;
                        }
                    }
                }
                previousValue = typeChances[i];
            }
        }
        tileHat.Clear();

        // TYPE REMAINING TILES
        // For each tile on the map that is not yet typed
        foreach (KeyValuePair<Vector2, GameObject> currentTile in WorldGenerator.s_TilesDictonary)
        {
            Tile tileScript = currentTile.Value.GetComponent<Tile>();
            if (tileScript.m_PhysicalType == PhysicalType.None)
            {
                float distanceToNearestPoint = 1000f;
                GameObject nearestTile = currentTile.Value;
                // Find the difference between it and each initial point
                foreach (GameObject initialTile in initialTiles)
                {
                    Vector2 index = initialTile.GetComponent<Tile>().m_TileIndex;
                    Vector2 difference = index - currentTile.Key;
                    float distance = difference.magnitude;

                    // Store the point as nearest if it is closer than the last found nearest point
                    if (distance < distanceToNearestPoint)
                    {
                        nearestTile = initialTile;
                        distanceToNearestPoint = distance;
                    }
                }
                tileScript.m_PhysicalType = nearestTile.GetComponent<Tile>().m_PhysicalType;
                tileScript.SetMaterial(m_TileManager.ReturnTileType(tileScript.m_PhysicalType));
            }
        }
        initialTiles.Clear();
    }

    private void AllocateInfrastructureTypes(in int rows)
    {
        // DEFINE TILE SET
        List<Vector2> tileSet = DefineTileSet(PhysicalType.Industrial);

        // SELECT INITIAL TILE POINTS
        List<Vector2[]> arrays = new List<Vector2[]> 
        {
            SelectInitialTilePoints(tileSet, m_commercialSpaceCount, m_minCommericalArea),
            SelectInitialTilePoints(tileSet, m_institutionCount, m_minInstitutionalArea),
            SelectInitialTilePoints(tileSet, m_vacantSpaceCount, m_minVacantArea)

        };

        // TYPE TILES
        // Set Type 
        PhysicalType[] physicalTypes = new PhysicalType[]
        {
            PhysicalType.Commercial,
            PhysicalType.Institutional,
            PhysicalType.Vacant
          
        };
        // Set Materials
        Material[] materials = new Material[] 
        {
            m_TileManager.ReturnTileType(PhysicalType.Commercial),
            m_TileManager.ReturnTileType(PhysicalType.Institutional),
            m_TileManager.ReturnTileType(PhysicalType.Vacant)

        };

        // Iterate through arrays
        for (int i = 0; i < arrays.Count; i++)
        {
            foreach (Vector2 initialPoint in arrays[i])
            {
                TypeInitialTiles(initialPoint, physicalTypes[i], materials[i]);

                TypeSurroundingTilesV1(initialPoint, physicalTypes[i], materials[i], PhysicalType.Industrial, PhysicalType.Forest);
            
                //// HERE ////
            }
        }
    }

    ////////////////////////
    //// HELPER METHODS ////
    ////////////////////////

    /// <summary>
    /// Creates a set of tiles from the Tile Dictonary using a PhysicalType to specify
    /// </summary>
    /// <param name="physicalType"></param>
    /// <returns>Vector2 List</returns>
    private List<Vector2> DefineTileSet(PhysicalType physicalType)
    {
        List<Vector2> tileSet = new List<Vector2>();

        foreach (KeyValuePair<Vector2, GameObject> tile in WorldGenerator.s_TilesDictonary)
        {
            if (tile.Value.GetComponent<Tile>().m_PhysicalType == physicalType)
            {
                tileSet.Add(tile.Key);
            }
        }
        return tileSet;
    }

    /// <summary>
    /// Selects initial points and creates a buffer around them
    /// where no other points can be selected from.
    /// </summary>
    /// <param name="tileSet"></param>
    /// <param name="segmentCount"></param>
    /// <param name="minimumArea"></param>
    /// <returns> Vector2[] Array </returns>
    private Vector2[] SelectInitialTilePoints(List<Vector2> tileSet, int segmentCount, int minimumArea)
    {
        Vector2[] initialPoints = new Vector2[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            if (tileSet.Count < 1) { break; } else
            {
                int index = Random.Range(0, tileSet.Count);
                initialPoints[i] = tileSet[index];

                for (int x = (int)initialPoints[i].x - (minimumArea / 2); x < (int)initialPoints[i].x + (minimumArea / 2); x++)
                {
                    for (int y = (int)initialPoints[i].y - (minimumArea / 2); y < (int)initialPoints[i].y + (minimumArea / 2); y++)
                    {
                        if (tileSet.Contains(new Vector2(x, y)))
                        {
                            tileSet.Remove(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        return initialPoints;
    }

    /// <summary>
    /// Assigns a final type and material to a Tile GameObject at the given position.
    /// </summary>
    /// <param name="initialPoint"></param>
    /// <param name="physicalType"></param>
    /// <param name="material"></param>
    private void TypeInitialTiles(Vector2 initialPoint, PhysicalType physicalType, Material material)
    {
        // Type initial tiles
        if (WorldGenerator.s_TilesDictonary.TryGetValue(initialPoint, out GameObject value))
        {
            Tile tileScript = value.GetComponent<Tile>();
            tileScript.m_PhysicalType = physicalType;
            tileScript.SetMaterial(material);
        }
    }

    /// <summary>
    /// Assigns a final type and material to each of the neighbouring Tile GameObjects of the initial where possible.
    /// </summary>
    /// <param name="initialPoint"></param>
    /// <param name="type"></param>
    /// <param name="material"></param>
    /// <param name="checkAgainstType1"></param>
    /// <param name="checkAgainstType2"></param>
    /// <returns></returns>
    private List<Vector2> TypeSurroundingTilesV1(Vector2 initialPoint, PhysicalType type, Material material, PhysicalType checkAgainstType1, PhysicalType checkAgainstType2)
    {
        List<Vector2> neighbourIndices = NeighbourUtility.GetNeighbours(initialPoint);

        foreach (Vector2 neighbourIndex in neighbourIndices)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(neighbourIndex, out GameObject nValue))
            {
                Tile tileScript = nValue.GetComponent<Tile>();
                if (tileScript.m_PhysicalType == checkAgainstType1 || tileScript.m_PhysicalType == checkAgainstType2)
                {
                    tileScript.m_PhysicalType = type;
                    tileScript.SetMaterial(material);
                }
            }
        }
        return neighbourIndices;
    }

    private List<Vector2> TypeSurroundingTilesV2(Vector2 initialPoint)
    {
        List<Vector2> neighbourIndices = NeighbourUtility.GetNeighbours(initialPoint);
        return neighbourIndices;
    }
}
