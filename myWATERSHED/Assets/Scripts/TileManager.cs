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
    [SerializeField]
    private List<Material> m_testMaterials;

    [SerializeField]
    private List<Material> m_baseMaterials;

    public Material ReturnTileType(PhysicalType physicalType) => m_testMaterials[(int)physicalType];
}
