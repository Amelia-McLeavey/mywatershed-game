using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeAllocator : MonoBehaviour
{
    public TileManager m_TileManager;

    [SerializeField]
    private int m_segments;

    [Range(0, 5)]
    [SerializeField]
    private int m_maxForestDepthAlongRiver = 4;
    [Range(6, 10)]
    [SerializeField]
    private int m_golfCourseCount = 6;
    [Range(5, 10)]
    [SerializeField]
    private int m_naturalReservoirCount = 5;
    [Range(0, 10)]
    [SerializeField]
    private int m_engineeredReservoirCount = 2;
    [SerializeField]
    private int m_industrialSpaceCount = 100;
    [SerializeField]
    private int m_institutionCount = 20;
    [SerializeField]
    private int m_vacantSpaceCount = 15;
    [SerializeField]
    private int m_urbanHouseSpaceCount = 400;
    [SerializeField]
    private int m_ruralHouseSpaceCount = 4;
    [SerializeField]
    private int m_naturalLandSegmentCount = 80;
    [Range(1, 5)]
    [SerializeField]
    private int m_industrialSpaceSize = 2;
    [Range(1, 5)]
    [SerializeField]
    private int m_urbanHousingSpaceSize = 2;

    private readonly int m_minArea = 3;
    private readonly int m_minGolfArea = 3;
    private readonly int m_minIndustrialArea = 5;
    private readonly int m_minInstitutionalArea = 1;
    private readonly int m_minVacantArea = 1;
    private readonly int m_minUrbanHouseArea = 1;
    private readonly int m_minRuralHouseArea = 7;
    private readonly int m_minNaturalLandArea = 3;
    private readonly int m_minNatResArea = 3;
    private readonly int m_minEngResArea = 3;

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
        AllocateInfrastructureTypes();
        AllocateResidentialTypes();
        AllocateRuralTypes();
        AllocateNaturalLandTypes();
        AllocateReservoirTypes();
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

            List<Vector2> neighbourIndices = TypeNeighbouringTiles(initialPoint, PhysicalType.GolfCourse, material, PhysicalType.None, PhysicalType.Forest, PhysicalType.Forest);
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
            PhysicalType.Commercial,
            PhysicalType.HighDensity,
            PhysicalType.Agriculture,
            PhysicalType.Forest
        };
        // Set materials
        Material[] materials = new Material[]
        {
            m_TileManager.ReturnTileType(PhysicalType.Commercial),
            m_TileManager.ReturnTileType(PhysicalType.HighDensity),
            m_TileManager.ReturnTileType(PhysicalType.Agriculture),
            m_TileManager.ReturnTileType(PhysicalType.Forest)
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
            if (initialPoint.x < rows * 0.70f)
            {
                typeChances[0] = 0.02f; // 2 %
                typeChances[1] = 0.04f; // 2 %
                typeChances[2] = 0.65f; // 56 %
                typeChances[3] = 1.00f; // 40 %
            } 
            else if (initialPoint.x > rows * 0.70f)
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

    private void AllocateInfrastructureTypes()
    {
        // DEFINE TILE SETS
        List<Vector2> comTileSet = DefineTileSet(PhysicalType.Commercial);
        List<Vector2> resTileSet = DefineTileSet(PhysicalType.HighDensity);
        List<Vector2> institutionalTileSet = new List<Vector2>();
        foreach (Vector2 tile in comTileSet)
        {
            institutionalTileSet.Add(tile);
        }
        foreach (Vector2 tile in resTileSet)
        {
            institutionalTileSet.Add(tile);
        }

        // SELECT INITIAL TILE POINTS
        List<Vector2[]> arrays = new List<Vector2[]> 
        {
            SelectInitialTilePoints(comTileSet, m_industrialSpaceCount, m_minIndustrialArea),
            SelectInitialTilePoints(institutionalTileSet, m_institutionCount, m_minInstitutionalArea),
            SelectInitialTilePoints(comTileSet, m_vacantSpaceCount, m_minVacantArea)

        };

        // TYPE TILES
        // Set Type 
        PhysicalType[] physicalTypes = new PhysicalType[]
        {
            PhysicalType.Industrial,
            PhysicalType.Institutional,
            PhysicalType.Vacant
          
        };
        // Set Materials
        Material[] materials = new Material[] 
        {
            m_TileManager.ReturnTileType(PhysicalType.Industrial),
            m_TileManager.ReturnTileType(PhysicalType.Institutional),
            m_TileManager.ReturnTileType(PhysicalType.Vacant)

        };

        // Iterate through arrays
        for (int i = 0; i < arrays.Count; i++)
        {
            foreach (Vector2 initialPoint in arrays[i])
            {
                TypeInitialTiles(initialPoint, physicalTypes[i], materials[i]);

                switch (i)
                {
                    case 0: // COMMERCIAL
                        TypeSurroundingTilesV2(m_industrialSpaceSize, initialPoint, physicalTypes[i], materials[i], PhysicalType.Commercial, PhysicalType.Commercial);
                        break;
                    case 1: // INSTITUTIONAL
                        float rng1 = Random.Range(0f, 1f);
                        if (rng1 > 0.3f)
                            TypeNeighbouringTiles(initialPoint, physicalTypes[i], materials[i], PhysicalType.Commercial, PhysicalType.Industrial, PhysicalType.HighDensity);
                            break;
                    case 2: // VACANT
                        float rng2 = Random.Range(0f, 1f);
                        if (rng2 > 0.4f)
                            TypeNeighbouringTiles(initialPoint, physicalTypes[i], materials[i], PhysicalType.Commercial, PhysicalType.Industrial, PhysicalType.Industrial);
                        break;
                    default: Debug.LogWarning("Value 'i' did not successfully compare to a case.");
                        break;
                }
            }
        }

        comTileSet.Clear();
        resTileSet.Clear();
        institutionalTileSet.Clear();
    }

    private void AllocateResidentialTypes()
    {
        // DEFINE TILE SET
        List<Vector2> resTileSet = DefineTileSet(PhysicalType.HighDensity);

        // SELECT INITIAL TILE POINTS 
        Vector2[] initialTilePoints = SelectInitialTilePoints(resTileSet, m_urbanHouseSpaceCount, m_minUrbanHouseArea);

        // TYPE TILES
        Material material = m_TileManager.ReturnTileType(PhysicalType.LowMidDensity);
        foreach (Vector2 initialPoint in initialTilePoints)
        {
            TypeInitialTiles(initialPoint, PhysicalType.LowMidDensity, material);
            TypeSurroundingTilesV2(m_urbanHousingSpaceSize, initialPoint, PhysicalType.LowMidDensity, material, PhysicalType.HighDensity, PhysicalType.Forest);
        }

        resTileSet.Clear();
    }

    private void AllocateRuralTypes()
    {
        // DEFINE TILE SET
        List<Vector2> ruralTileSet = DefineTileSet(PhysicalType.Agriculture);

        // SELECT INITIAL TILE POINTS 
        Vector2[] initialTilePoints = SelectInitialTilePoints(ruralTileSet, m_ruralHouseSpaceCount, m_minRuralHouseArea);

        // TYPE TILES
        Material material = m_TileManager.ReturnTileType(PhysicalType.EstateResidential);
        foreach (Vector2 initialPoint in initialTilePoints)
        {
            TypeInitialTiles(initialPoint, PhysicalType.EstateResidential, material);
            TypeSurroundingTilesV4(initialPoint, PhysicalType.EstateResidential, material, PhysicalType.Agriculture);
        }

        ruralTileSet.Clear();
    }

    private void AllocateNaturalLandTypes()
    {
        // DEFINE TILE SET
        List<Vector2> naturalLandlTileSet = DefineTileSet(PhysicalType.Forest);

        // SELECT INITIAL TILE POINTS 
        Vector2[] initialTilePoints = SelectInitialTilePoints(naturalLandlTileSet, m_naturalLandSegmentCount, m_minNaturalLandArea);

        // TYPE TILES
        Material material = m_TileManager.ReturnTileType(PhysicalType.Meadow);
        foreach (Vector2 initialPoint in initialTilePoints)
        {
            TypeInitialTiles(initialPoint, PhysicalType.Meadow, material);
            TypeSurroundingTilesV4(initialPoint, PhysicalType.Meadow, material, PhysicalType.Forest);
        }

        naturalLandlTileSet.Clear();

        // DEFINE TILE SET FOR SUCCESSIONAL
        List<Vector2> successionalLandlTileSet = DefineTileSet(PhysicalType.Meadow);

        Material successionalMaterial = m_TileManager.ReturnTileType(PhysicalType.Successional);

        foreach (Vector2 tilePoint in successionalLandlTileSet)
        {
            float rng = Random.Range(0f, 1f);
            if (rng < 0.02)
            {
                // Type initial tiles
                if (WorldGenerator.s_TilesDictonary.TryGetValue(tilePoint, out GameObject value))
                {
                    Tile tileScript = value.GetComponent<Tile>();
                    tileScript.m_PhysicalType = PhysicalType.Successional;
                    tileScript.SetMaterial(successionalMaterial);
                }
            }
        }
    }

    private void AllocateReservoirTypes()
    {
        // DEFINE NATURAL RESERVOIR TILE SET
        List<Vector2> tileSet1 = DefineTileSet(PhysicalType.Forest);
        List<Vector2> tileSet2 = DefineTileSet(PhysicalType.Meadow);
        List<Vector2> reservoirTileSet = new List<Vector2>();
        foreach (Vector2 tile in tileSet1)
        {
            reservoirTileSet.Add(tile);
        }
        foreach (Vector2 tile in tileSet2)
        {
            reservoirTileSet.Add(tile);
        }
        tileSet1.Clear();
        tileSet2.Clear();

        // SELECT INITIAL TILE POINTS
        Vector2[] initialTilePoints1 = SelectInitialTilePoints(reservoirTileSet, m_naturalReservoirCount, m_minNatResArea);

        // TYPE TILES
        Material material = m_TileManager.ReturnTileType(PhysicalType.NaturalReservoir);

        foreach (Vector2 initialPoint in initialTilePoints1)
        {
            TypeInitialTiles(initialPoint, PhysicalType.NaturalReservoir, material);
            TypeSurroundingTilesV3(initialPoint, PhysicalType.NaturalReservoir, material, PhysicalType.Forest, PhysicalType.Meadow);
        }

        reservoirTileSet.Clear();

        // DEFINE ENGINEERED RESERVOIR TILE SET
        reservoirTileSet = DefineTileSet(PhysicalType.Agriculture);

        // SELECT INITIAL TILE POINTS
        Vector2[] initialTilePoints2 = SelectInitialTilePoints(reservoirTileSet, m_engineeredReservoirCount, m_minEngResArea);

        // TYPE TILES
        material = m_TileManager.ReturnTileType(PhysicalType.EngineeredReservoir);

        foreach (Vector2 initialPoint in initialTilePoints2)
        {
            TypeInitialTiles(initialPoint, PhysicalType.EngineeredReservoir, material);
            TypeSurroundingTilesV3(initialPoint, PhysicalType.EngineeredReservoir, material, PhysicalType.Agriculture, PhysicalType.LowMidDensity);
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    //// HELPER METHODS ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////

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
    private Vector2[] SelectInitialTilePoints(in List<Vector2> tileSet, int segmentCount, in int minimumArea)
    {
        List<Vector2> _tileSet = new List<Vector2>();
        foreach (Vector2 tile in tileSet)
        {
            _tileSet.Add(tile);
        }

        Vector2[] initialPoints = new Vector2[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            if (_tileSet.Count < 1) { break; } else
            {
                int index = Random.Range(0, _tileSet.Count);
                initialPoints[i] = _tileSet[index];

                for (int x = (int)initialPoints[i].x - (minimumArea / 2); x < (int)initialPoints[i].x + (minimumArea / 2); x++)
                {
                    for (int y = (int)initialPoints[i].y - (minimumArea / 2); y < (int)initialPoints[i].y + (minimumArea / 2); y++)
                    {
                        if (_tileSet.Contains(new Vector2(x, y)))
                        {            
                            _tileSet.Remove(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        _tileSet.Clear();
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
    private List<Vector2> TypeNeighbouringTiles(Vector2 initialPoint, PhysicalType type, Material material, PhysicalType checkAgainstType1, PhysicalType checkAgainstType2, PhysicalType checkAgainstType3)
    {
        List<Vector2> neighbourIndices = NeighbourUtility.GetNeighbours(initialPoint);

        foreach (Vector2 neighbourIndex in neighbourIndices)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(neighbourIndex, out GameObject nValue))
            {
                Tile tileScript = nValue.GetComponent<Tile>();
                if (tileScript.m_PhysicalType == checkAgainstType1 || tileScript.m_PhysicalType == checkAgainstType2 || tileScript.m_PhysicalType == checkAgainstType3)
                {
                    tileScript.m_PhysicalType = type;
                    tileScript.SetMaterial(material);
                }
            }
        }
        return neighbourIndices;
    }

    /// <summary>
    /// Assigns a final type and material where possible to the Tile GameObjects below the initial for a given length, then does the same for extra surrounding tiles.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="initialPoint"></param>
    /// <param name="type"></param>
    /// <param name="material"></param>
    /// <param name="checkAgainstType1"></param>
    /// <param name="checkAgainstType2"></param>
    private void TypeSurroundingTilesV2(int size, Vector2 initialPoint, PhysicalType type, Material material, PhysicalType checkAgainstType1, PhysicalType checkAgainstType2)
    {
        List<Vector2> additionalTileIndicies = new List<Vector2>();

        // Return the tile below [4] the initial point
        additionalTileIndicies.Add(NeighbourUtility.GetNeighbours(initialPoint)[4]);

        Vector2 neighbouBelowIndex = additionalTileIndicies[0];

        for (int i = 0; i < size - 1; i++)
        {
            neighbouBelowIndex = NeighbourUtility.GetNeighbours(neighbouBelowIndex)[4];
            additionalTileIndicies.Add(neighbouBelowIndex);
        }

        foreach (Vector2 index in additionalTileIndicies)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(index, out GameObject nValue))
            {
                Tile tileScript = nValue.GetComponent<Tile>();
                if (tileScript.m_PhysicalType == checkAgainstType1 || tileScript.m_PhysicalType == checkAgainstType2)
                {
                    tileScript.m_PhysicalType = type;
                    tileScript.SetMaterial(material);
                }
            }
        }

        // Type extra tiles
        for (int i = 0; i < 2; i++)
        {
            int index = Random.Range(0, additionalTileIndicies.Count);

            List<Vector2> extraNeighbourIndicies = NeighbourUtility.GetNeighbours(additionalTileIndicies[index]);

            foreach (Vector2 extraNeighbourIndex in extraNeighbourIndicies)
            {
                if (WorldGenerator.s_TilesDictonary.TryGetValue(extraNeighbourIndex, out GameObject eValue))
                {
                    Tile tileScript = eValue.GetComponent<Tile>();
                    if (tileScript.m_PhysicalType == checkAgainstType1 || tileScript.m_PhysicalType == checkAgainstType2)
                    {
                        tileScript.m_PhysicalType = type;
                        tileScript.SetMaterial(material);
                    }
                }
            }
            additionalTileIndicies.RemoveAt(index);
        }
    }

    // USE FOR RESERVOIRS
    /// <summary>
    /// Assigns a final type and material where possible to the Tile Objects surrounding the initial tile, then does the same for extra surrounding tiles.
    /// </summary>
    /// <param name="initialPoint"></param>
    /// <param name="type"></param>
    /// <param name="material"></param>
    /// <param name="checkAgainstType1"></param>
    private void TypeSurroundingTilesV3(Vector2 initialPoint, PhysicalType type, Material material, PhysicalType checkAgainstType1, PhysicalType checkAgainstType2)
    {
        List<Vector2> neighbourIndices = TypeNeighbouringTiles(initialPoint, type, material, checkAgainstType1, checkAgainstType2, checkAgainstType2);

        // Type extra tiles
        for (int i = 0; i < 2; i++)
        {
            int index = Random.Range(0, neighbourIndices.Count);

            List<Vector2> extraNeighbourIndicies = NeighbourUtility.GetNeighbours(neighbourIndices[index]);

            foreach (Vector2 extraNeighbourIndex in extraNeighbourIndicies)
            {
                if (WorldGenerator.s_TilesDictonary.TryGetValue(extraNeighbourIndex, out GameObject eValue))
                {
                    Tile tileScript = eValue.GetComponent<Tile>();
                    if (tileScript.m_PhysicalType == checkAgainstType1)
                    {
                        tileScript.m_PhysicalType = type;
                        tileScript.SetMaterial(material);
                    }
                }
            }
            neighbourIndices.RemoveAt(index);
        }
    }

    /// <summary>
    /// Assigns a final type and material where possible to the Tile Objects surrounding the tile at random for some measure of growth.
    /// </summary>
    /// <param name="initialPoint"></param>
    /// <param name="type"></param>
    /// <param name="material"></param>
    /// <param name="checkAgainstType1"></param>
    private void TypeSurroundingTilesV4(Vector2 initialPoint, PhysicalType type, Material material, PhysicalType checkAgainstType1)
    {
        List<Vector2> initialNeighbourIndices = TypeNeighbouringTiles(initialPoint, type, material, checkAgainstType1, checkAgainstType1, checkAgainstType1);
        List<Vector2> nextNeighbourIndicies = initialNeighbourIndices;

        int growthSize = 8;

        for (int i = 0; i < growthSize; i++)
        {
            // Type all neighbours of some of the initial neighbour indicies
            int someNeighbourAmount = 3;

            for (int s = 0; s < someNeighbourAmount - 1; s++)
            {
                if (nextNeighbourIndicies.Count <= 0) break; else
                {
                    int randomInitialNeighbourIndex = Random.Range(0, nextNeighbourIndicies.Count);

                    List<Vector2> currentNeighbourIndicies = NeighbourUtility.GetNeighbours(nextNeighbourIndicies[randomInitialNeighbourIndex]);

                    foreach (Vector2 currentNeighbourIndex in currentNeighbourIndicies)
                    {
                        if (WorldGenerator.s_TilesDictonary.TryGetValue(currentNeighbourIndex, out GameObject eValue))
                        {
                            Tile tileScript = eValue.GetComponent<Tile>();
                            if (tileScript.m_PhysicalType == checkAgainstType1)
                            {
                                tileScript.m_PhysicalType = type;
                                tileScript.SetMaterial(material);
                                nextNeighbourIndicies.Add(currentNeighbourIndex);
                            }
                        }
                    }
                    // So that the same index is not chosen in the next iteration
                    nextNeighbourIndicies.RemoveAt(randomInitialNeighbourIndex);
                }    
            }
        }
        nextNeighbourIndicies.Clear();
        initialNeighbourIndices.Clear();
    }
}
