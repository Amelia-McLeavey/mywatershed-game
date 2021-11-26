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
        WorldGenerator.OnWorldGenerationComplete += GenerateSystem;
    }

    private void OnDisable()
    {
        WorldGenerator.OnWorldGenerationComplete -= GenerateSystem;
    }

    public void GenerateSystem(int rows, int columns)
    {
        m_rows = rows;
        m_columns = columns;

        AddComponentsToTileObjects();
        InitializeSenderOnlyRiverTiles();

        OnSystemGenerationComplete?.Invoke(m_rows, m_columns);
    }

    /// <summary>
    /// Iterates through each tile object to add necessary script Components.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    private void AddComponentsToTileObjects()
    {
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
    }

    private void InitializeSenderOnlyRiverTiles()
    {
        // Find all water Tiles in row 0
        for (int y = 0; y < m_columns; y++)
        {
            Vector2 tileIndex = new Vector2(0, y);

            if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
            {
                Tile tileScript = value.GetComponent<Tile>();

                if (tileScript.m_Basetype == BaseType.Water)
                {
                    tileScript.m_isStateSpawner = true;
                }
            }
        }

        for (int x = 0; x < m_rows; x++)
        {
            Vector2 tileIndexLeft = new Vector2(x, 0);
            Vector2 tileIndexRight = new Vector2(x, m_columns - 1);

            if (TileManager.s_TilesDictonary.TryGetValue(tileIndexLeft, out GameObject valueL))
            {
                Tile tileScript = valueL.GetComponent<Tile>();

                if (tileScript.m_Basetype == BaseType.Water)
                {
                    tileScript.m_isStateSpawner = true;
                }
            }

            if (TileManager.s_TilesDictonary.TryGetValue(tileIndexRight, out GameObject valueR))
            {
                Tile tileScript = valueR.GetComponent<Tile>();

                if (tileScript.m_Basetype == BaseType.Water)
                {
                    tileScript.m_isStateSpawner = true;
                }
            }
        }
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
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Agriculture tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Agriculture";
                break;
            case PhysicalType.Commercial:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Commercial tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Commercial";
                break;
            case PhysicalType.EngineeredReservoir:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "EngineeredReservoir tiles are expected to be water type");
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
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "EngineeredStream tiles are expected to be water type");
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
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "EstateResidential tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "EstateResidential";
                break;
            case PhysicalType.Forest:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Forest tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));               
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Forest";
                break;
            case PhysicalType.GolfCourse:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "GolfCourse tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "GolfCourse";
                break;
            case PhysicalType.HighDensity:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "HighDensity tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));                
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "HighDensity";
                break;
            case PhysicalType.Highway:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Highway tiles are expected to be land type");
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
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(RiverbedHealth));
                currentTile.AddComponent(typeof(Sinuosity));
                currentTile.AddComponent(typeof(ShadeCoverage));
                currentTile.AddComponent(typeof(Turbidity));
                currentTile.AddComponent(typeof(WaterDepth));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Highway";
                break;
            case PhysicalType.Industrial:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Industrial tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Industrial";
                break;
            case PhysicalType.Institutional:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Institutional tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Institutional";
                break;
            case PhysicalType.LowMidDensity:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "LowMidDensity tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "LowMidDensity";
                break;
            case PhysicalType.Meadow:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Meadow tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(RiparianLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Meadow";
                break;
            case PhysicalType.NaturalReservoir:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "NaturalReservoir tiles are expected to be water type");
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
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "NaturalStream tiles are expected to be water type");
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
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "RecreationCentreSpace tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "RecreationCentreSpace";
                break;
            case PhysicalType.Successional:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Successional tiles are suspected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Successional";
                break;
            case PhysicalType.UrbanOpenSpace:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "UrbanOpenSpace tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "UrbanOpenSpace";
                break;
            case PhysicalType.Vacant:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Land, "Vacant tiles are expected to be land type");
                currentTile.AddComponent(typeof(AsphaltDensity));
                currentTile.AddComponent(typeof(ErosionRate));
                currentTile.AddComponent(typeof(LandHeight));
                currentTile.AddComponent(typeof(PollutionLevel));
                currentTile.AddComponent(typeof(SewageLevel));
                currentTile.AddComponent(typeof(WaterTemperature));
                currentTile.tag = "Vacant";
                break;
            case PhysicalType.Wetland:
                Debug.Assert(currentTile.GetComponent<Tile>().m_Basetype == BaseType.Water, "Wetland tiles are expected to be water type");
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
