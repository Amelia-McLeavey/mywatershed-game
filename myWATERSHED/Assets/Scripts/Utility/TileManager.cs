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

public class TileManager : MonoBehaviour
{
    public static Dictionary<Vector2, GameObject> s_TilesDictonary = new Dictionary<Vector2, GameObject>();

    // References for tile base plane colors
    [SerializeField]
    private List<Color> m_testColors;
    [SerializeField]
    private List<Color> m_baseColors;

    [SerializeField]
    public List<GameObject> m_minatures_1;
    [SerializeField]
    public List<GameObject> m_minatures_2;
    [SerializeField]
    public List<GameObject> m_minatures_3;

    /// <summary>
    /// Finds the corresponding colour given a type.
    /// </summary>
    /// <param name="physicalType"></param>
    /// <returns> Material </returns>
    public Color ReturnTileType(PhysicalType physicalType) => m_testColors[(int)physicalType];
}
