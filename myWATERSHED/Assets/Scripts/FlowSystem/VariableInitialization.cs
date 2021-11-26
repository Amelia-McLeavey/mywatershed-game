using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableInitialization : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        return false;
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        Debug.Assert(senderTile != null, $"senderTile is null at index {tileIndexForDebugging}");

        Tile tileScript = senderTile.GetComponent<Tile>();
        PhysicalType _physicalType = tileScript.m_PhysicalType;

        switch (_physicalType)
        {
            case PhysicalType.None:
                Debug.LogError($"There is no PhysicalType associated with tile at index {tileIndexForDebugging}");
                break;
            case PhysicalType.Agriculture:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Agriculture basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0f, 0.05f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.10f, 0.90f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.1f);
                break;
            case PhysicalType.Commercial:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Commercial basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0.8f, 0.95f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.20f, 0.5f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.1f);
                break;
            case PhysicalType.EngineeredReservoir:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "EngineeredReservoir basetype must be water");
                senderTile.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation = Random.Range(0, 10);
                senderTile.GetComponent<CreekChubPopulation>().m_CreekChubPopulation = Random.Range(0, 10);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.6f, 1f);
                senderTile.GetComponent<RedDacePopulation>().m_RedDacePopulation = Random.Range(0, 10);
                senderTile.GetComponent<RiparianLevel>().m_RiparianLevel = Random.Range(0f, 0.4f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.8f, 1f);
                senderTile.GetComponent<Sinuosity>().m_Sinuosity = Random.Range(0f, 0.2f);
                senderTile.GetComponent<WaterDepth>().m_WaterDepth = Random.Range (1, 6);
                break;
            case PhysicalType.EngineeredStream:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "EngineeredStream basetype must be water");
                senderTile.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation = Random.Range(0, 10);
                senderTile.GetComponent<CreekChubPopulation>().m_CreekChubPopulation = Random.Range(0, 10);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.6f, 1f);
                senderTile.GetComponent<RedDacePopulation>().m_RedDacePopulation = Random.Range(0, 10);
                senderTile.GetComponent<RiparianLevel>().m_RiparianLevel = Random.Range(0f, 0.4f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.8f, 1f);
                senderTile.GetComponent<Sinuosity>().m_Sinuosity = Random.Range(0f, 0.2f);
                senderTile.GetComponent<WaterDepth>().m_WaterDepth = Random.Range(1, 6);
                break;
            case PhysicalType.EstateResidential:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "EstateResidential basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0f, 0.1f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.20f, 0.5f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.2f);
                break;
            case PhysicalType.Forest:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Forest basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0f, 0.05f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0f, 0.1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.2f);
                break;
            case PhysicalType.GolfCourse:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "GolfCourse basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0f, 0.2f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.6f, 0.9f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.1f, 0.3f);
                break;
            case PhysicalType.HighDensity:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "HighDensity basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0.7f, 0.8f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.6f, 0.9f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.7f, 1f);
                break;
            case PhysicalType.Highway:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Highway basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = 1f;
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.9f, 1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.2f);
                break;
            case PhysicalType.Industrial:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Industrial basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = 1f;
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.9f, 1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.4f, 0.8f);
                break;
            case PhysicalType.Institutional:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Institutional basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0.5f, 0.7f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.4f, 1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.4f, 0.8f);
                break;
            case PhysicalType.LowMidDensity:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "LowMidDensity basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0.5f, 0.7f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.7f, 0.9f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.8f, 0.9f);
                break;
            case PhysicalType.Meadow:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Meadow basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0f, 0.1f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0f, 0.1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.01f);
                break;
            case PhysicalType.NaturalReservoir:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "NaturalReservoir basetype must be water");
                senderTile.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation = Random.Range(0, 50);
                senderTile.GetComponent<CreekChubPopulation>().m_CreekChubPopulation = Random.Range(0, 50);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0f, 0.2f);
                senderTile.GetComponent<RedDacePopulation>().m_RedDacePopulation = Random.Range(0, 50);
                senderTile.GetComponent<RiparianLevel>().m_RiparianLevel = Random.Range(0.4f, 1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.2f);
                senderTile.GetComponent<Sinuosity>().m_Sinuosity = Random.Range(0.2f, 0.5f);
                senderTile.GetComponent<WaterDepth>().m_WaterDepth = Random.Range(1, 10);
                break;
            case PhysicalType.NaturalStream:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "NaturalStream basetype must be water");
                senderTile.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation = Random.Range(0, 50);
                senderTile.GetComponent<CreekChubPopulation>().m_CreekChubPopulation = Random.Range(0, 50);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0f, 0.2f);
                senderTile.GetComponent<RedDacePopulation>().m_RedDacePopulation = Random.Range(0, 50);
                senderTile.GetComponent<RiparianLevel>().m_RiparianLevel = Random.Range(0.4f, 1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.2f);
                senderTile.GetComponent<Sinuosity>().m_Sinuosity = Random.Range(0.2f, 1f);
                senderTile.GetComponent<WaterDepth>().m_WaterDepth = Random.Range(1, 6);
                break;
            case PhysicalType.RecreationCentreSpace:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "RecreationCentreSpace basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0.3f, 0.6f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.2f, 0.4f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.4f, 0.5f);
                break;
            case PhysicalType.Successional:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Successional basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0f, 0.05f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0f, 0.1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.2f);
                break;
            case PhysicalType.UrbanOpenSpace:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "UrbanOpenSpace basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0.1f, 0.4f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0.2f, 0.4f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0.4f, 0.5f);
                break;
            case PhysicalType.Vacant:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Vacant basetype must be land");
                senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity = Random.Range(0.85f, 1f);
                senderTile.GetComponent<LandHeight>().m_LandHeight = Mathf.Abs(senderTile.transform.localScale.z);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0f, 0.1f);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.1f);
                break;
            case PhysicalType.Wetland:
                Debug.Assert(senderTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "Wetland basetype must be water" );
                senderTile.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation = Random.Range(0, 25);
                senderTile.GetComponent<CreekChubPopulation>().m_CreekChubPopulation = Random.Range(0, 25);
                senderTile.GetComponent<PollutionLevel>().m_PolutionLevel = Random.Range(0f, 0.2f);
                senderTile.GetComponent<RedDacePopulation>().m_RedDacePopulation = Random.Range(0, 25);
                senderTile.GetComponent<SewageLevel>().m_SewageLevel = Random.Range(0f, 0.2f);
                senderTile.GetComponent<Sinuosity>().m_Sinuosity = Random.Range(0.5f, 0.9f);
                senderTile.GetComponent<WaterDepth>().m_WaterDepth = Random.Range(1, 6);
                break;
            default: 
                Debug.LogError($"There is no PhysicalType associated with tile at index {tileIndexForDebugging}");
                break;


        }
    }
}
