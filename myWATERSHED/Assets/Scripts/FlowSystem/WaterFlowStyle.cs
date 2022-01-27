using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This script contains flow logic for all abiotic components pertaining to water tiles

public class WaterFlowStyle : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {

        Tile senderTileScript = senderTile.GetComponent<Tile>();
        Tile receiverTileScript = receiverTile.GetComponent<Tile>();

        // Check if the sender tile is able to run this flow in the first place
        if (senderTileScript.m_Basetype != BaseType.Water)
        {
            return false;
        }

        // Check if any of the neighbouring tiles are able to recieve data
        if (receiverTileScript.m_PhysicalType == PhysicalType.Highway)     
        {
            return true;
        }
        else if (receiverTileScript.m_Basetype == BaseType.Water)
        {
            return true;
        }
        else if (receiverTileScript.m_Basetype == BaseType.Land)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        // DEBUGS BE DEFENSIVE 
        Debug.Assert(senderTile != null, $"senderTile is null at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile != null, $"receiverTile is null at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<InsectPopulation>() != null, $"senderTile is missing an InsectPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<InsectPopulation>() != null, $"receiverTile is missing an InsectPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<PollutionLevel>() != null, $"senderTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<PollutionLevel>() != null, $"receiverTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<SewageLevel>() != null, $"senderTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<SewageLevel>() != null, $"receiverTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<Turbidity>() != null, $"senderTile is missing a Turbidity component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<Turbidity>() != null, $"receiverTile is missing a Turbidity component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<WaterTemperature>() != null, $"senderTile is missing a WaterTemperature component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<WaterTemperature>() != null, $"receiverTile is missing a WaterTemperature component at index {tileIndexForDebugging}");
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        // Reset the list then scatter by adding sender tile current value to the recievers' list

        // INSECT POPULATION
        InsectPopulation senderTileInsectPopulation = senderTile.GetComponent<InsectPopulation>();
        senderTileInsectPopulation.m_GatheredInsectPopulationValues.Clear();
        receiverTile.GetComponent<InsectPopulation>().m_GatheredInsectPopulationValues.Add((int)(senderTileInsectPopulation.value));
        // POLLUTION LEVEL
        PollutionLevel senderTilePollutionLevel = senderTile.GetComponent<PollutionLevel>();
        senderTilePollutionLevel.m_GatheredPolutionValues.Clear();
        receiverTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Add(senderTilePollutionLevel.value);
        // SEWAGE LEVEL
        SewageLevel senderTileSewageLevel = senderTile.GetComponent<SewageLevel>();
        senderTileSewageLevel.m_GatheredSewageValues.Clear();
        receiverTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Add(senderTileSewageLevel.value);
        // TURBIDITY
        Turbidity senderTileTurbidity = senderTile.GetComponent<Turbidity>();
        senderTileTurbidity.m_GatheredTurbidityValues.Clear();
        receiverTile.GetComponent<Turbidity>().m_GatheredTurbidityValues.Add(senderTileTurbidity.value);
        // WATER TEMPERATURE
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();
        senderTileWaterTemperature.m_GatheredWaterTemperatureValues.Clear();
        receiverTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Add(senderTileWaterTemperature.value);
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // Take the average of the gathered values and update own value

        // INSECT POPULATION
        InsectPopulation senderTileInsectPopulation = senderTile.GetComponent<InsectPopulation>();
        if (senderTileInsectPopulation.m_GatheredInsectPopulationValues.Count != 0)
        {
            senderTileInsectPopulation.value = Mathf.RoundToInt((float)senderTileInsectPopulation.m_GatheredInsectPopulationValues.Average());
        }
        // POLLUTION LEVEL
        PollutionLevel senderTilePollutionLevel = senderTile.GetComponent<PollutionLevel>();
        if (senderTilePollutionLevel.m_GatheredPolutionValues.Count != 0)
        {
            senderTilePollutionLevel.value = senderTilePollutionLevel.m_GatheredPolutionValues.Average();
        }
        // SEWAGE LEVEL
        SewageLevel senderTileSewageLevel = senderTile.GetComponent<SewageLevel>();
        if (senderTileSewageLevel.m_GatheredSewageValues.Count != 0)
        {
            senderTileSewageLevel.value = senderTileSewageLevel.m_GatheredSewageValues.Average();
        }
        // TURBIDITY
        Turbidity senderTileTurbidity = senderTile.GetComponent<Turbidity>();
        if (senderTileTurbidity.m_GatheredTurbidityValues.Count != 0)
        {
            senderTileTurbidity.value = senderTileTurbidity.m_GatheredTurbidityValues.Average();
        }
        // WATER TEMPERATURE
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();
        if (senderTileWaterTemperature.m_GatheredWaterTemperatureValues.Count != 0)
        {
            senderTileWaterTemperature.value = senderTileWaterTemperature.m_GatheredWaterTemperatureValues.Average();
        }
        
    }
}
