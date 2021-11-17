using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemGenerator : MonoBehaviour
{
    private int m_rows;
    private int m_columns;

    public delegate void SystemGenerationComplete(int rowSize, int columnSize);
    public static event SystemGenerationComplete OnSystemGenerationComplete;

    private void OnEnable()
    {
        WorldGenerator.OnWorldGenerationComplete += AddComponentsToTileObjects;
    }

    private void OnDisable()
    {
        WorldGenerator.OnWorldGenerationComplete -= AddComponentsToTileObjects;
    }

    /// <summary>
    /// Iterates through each tile object to add necessary script Components.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    public void AddComponentsToTileObjects(int rows, int columns)
    {
        m_rows = rows;
        m_columns = columns;

        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    DetermineTileComponents(value, value.GetComponent<Tile>().m_PhysicalType);
                }
            }
        }

        OnSystemGenerationComplete?.Invoke(m_rows, m_columns);
    }

    //// Please search for a tile's implemented set of components using CTRL+I 
    ////    then type in name of physical tile type.
    //// This setup allows for individualization of component setup per tile type.
    /// <summary>
    /// Adds the appropriate script Components depending on the PhysicalType of the current tile.
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="physicalType"></param>
    private void DetermineTileComponents(GameObject currentTile, PhysicalType physicalType)
    {
        Debug.Assert(physicalType != PhysicalType.None, $"There is no PhysicalType associated with tile at index {currentTile.GetComponent<Tile>().m_TileIndex}");

        switch (physicalType)
        {
            case PhysicalType.None:
                Debug.LogError($"There is no PhysicalType associated with tile at index {currentTile.GetComponent<Tile>().m_TileIndex}");
                break;
            case PhysicalType.Agriculture:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Agriculture";
                break;
            case PhysicalType.Commercial:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Commercial";
                break;
            case PhysicalType.EngineeredReservoir:
                currentTile.AddComponent(typeof(BrownTroutPopulation));
                currentTile.AddComponent(typeof(CreekChubPopulation));
                currentTile.AddComponent(typeof(InsectPopulation));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(RateOfFlow));
                currentTile.AddComponent(typeof(RedDacePopulation));
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(RiverbedHealth));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(Sinuosity));
                currentTile.AddComponent(typeof(ShadeCoverage));
                currentTile.AddComponent(typeof(Turbidity));
                currentTile.AddComponent(typeof(WaterDepth));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "EngineeredReservoir";
                break;
            case PhysicalType.EngineeredStream:
                currentTile.AddComponent(typeof(BrownTroutPopulation));
                currentTile.AddComponent(typeof(CreekChubPopulation));
                currentTile.AddComponent(typeof(InsectPopulation));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(RateOfFlow));
                currentTile.AddComponent(typeof(RedDacePopulation));
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(RiverbedHealth));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(Sinuosity));
                currentTile.AddComponent(typeof(ShadeCoverage));
                currentTile.AddComponent(typeof(Turbidity));
                currentTile.AddComponent(typeof(WaterDepth));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "EngineeredStream";
                break;
            case PhysicalType.EstateResidential:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "EstateResidential";
                break;
            case PhysicalType.Forest:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));               
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Forest";
                break;
            case PhysicalType.GolfCourse:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "GolfCourse";
                break;
            case PhysicalType.HighDensity:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));                
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "HighDensity";
                break;
            case PhysicalType.Highway:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                // These variables need to be able to flow through Highway because Highway crosses water.
                currentTile.AddComponent(typeof(BrownTroutPopulation));
                currentTile.AddComponent(typeof(CreekChubPopulation));
                currentTile.AddComponent(typeof(InsectPopulation));
                currentTile.AddComponent(typeof(RateOfFlow));
                currentTile.AddComponent(typeof(RedDacePopulation));
                currentTile.AddComponent(typeof(RiparianLevel)); // How much does Shade effect the Riparian Level?
                currentTile.AddComponent(typeof(RiverbedHealth));
                currentTile.AddComponent(typeof(Sinuosity));
                currentTile.AddComponent(typeof(ShadeCoverage));
                currentTile.AddComponent(typeof(Turbidity));
                currentTile.AddComponent(typeof(WaterDepth));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Highway";
                break;
            case PhysicalType.Industrial:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Industrial";
                break;
            case PhysicalType.Institutional:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Institutional";
                break;
            case PhysicalType.LowMidDensity:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "LowMidDensity";
                break;
            case PhysicalType.Meadow:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Meadow";
                break;
            case PhysicalType.NaturalReservoir:
                currentTile.AddComponent(typeof(BrownTroutPopulation));
                currentTile.AddComponent(typeof(CreekChubPopulation));
                currentTile.AddComponent(typeof(InsectPopulation));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(RateOfFlow));
                currentTile.AddComponent(typeof(RedDacePopulation));
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(RiverbedHealth));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(Sinuosity));
                currentTile.AddComponent(typeof(ShadeCoverage));
                currentTile.AddComponent(typeof(Turbidity));
                currentTile.AddComponent(typeof(WaterDepth));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "NaturalReservoir";
                break;
            case PhysicalType.NaturalStream:
                currentTile.AddComponent(typeof(BrownTroutPopulation));
                currentTile.AddComponent(typeof(CreekChubPopulation));
                currentTile.AddComponent(typeof(InsectPopulation));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(RateOfFlow));
                currentTile.AddComponent(typeof(RedDacePopulation));
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(RiverbedHealth));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(Sinuosity));
                currentTile.AddComponent(typeof(ShadeCoverage));
                currentTile.AddComponent(typeof(Turbidity));
                currentTile.AddComponent(typeof(WaterDepth));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "NaturalStream";
                break;
            case PhysicalType.RecreationCentreSpace:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "RecreationCentreSpace";
                break;
            case PhysicalType.Successional:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Successional";
                break;
            case PhysicalType.UrbanOpenSpace:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "UrbanOpenSpace";
                break;
            case PhysicalType.Vacant:
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Vacant";
                break;
            case PhysicalType.Wetland:
                currentTile.AddComponent(typeof(BrownTroutPopulation));
                currentTile.AddComponent(typeof(CreekChubPopulation));
                currentTile.AddComponent(typeof(InsectPopulation));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(RateOfFlow));
                currentTile.AddComponent(typeof(RedDacePopulation));
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(RiverbedHealth));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(Sinuosity));
                currentTile.AddComponent(typeof(ShadeCoverage));
                currentTile.AddComponent(typeof(Turbidity));
                currentTile.AddComponent(typeof(WaterDepth));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Wetland";
                break;
            default:
                Debug.LogError($"There is no PhysicalType associated with tile at index {currentTile.GetComponent<Tile>().m_TileIndex}");
                break;
        }
    }
}
