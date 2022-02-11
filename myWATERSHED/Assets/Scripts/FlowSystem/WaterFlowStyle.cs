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
        InsectPopulation receiverTileInsectPopulation = receiverTile.GetComponent<InsectPopulation>();
        // Add value to the array at specified index and then increment the index
        receiverTileInsectPopulation.m_gatheredInsectPopValues[receiverTileInsectPopulation.m_numGatheredInsectPopValues] = senderTileInsectPopulation.value;
        receiverTileInsectPopulation.m_numGatheredInsectPopValues++;

        // POLLUTION LEVEL
        PollutionLevel senderTilePollutionLevel = senderTile.GetComponent<PollutionLevel>();
        PollutionLevel receiverTilePollutionLevel = receiverTile.GetComponent<PollutionLevel>();
        // Add value to the array at specified index and then increment the index
        receiverTilePollutionLevel.m_gatheredPollutionValues[receiverTilePollutionLevel.m_numGatheredPollutionValues] = senderTilePollutionLevel.value;
        receiverTilePollutionLevel.m_numGatheredPollutionValues++;

        // SEWAGE LEVEL
        SewageLevel senderTileSewageLevel = senderTile.GetComponent<SewageLevel>();
        SewageLevel receiverTileSewageLevel = receiverTile.GetComponent<SewageLevel>();
        // Add value to the array at specified index and then increment the index
        receiverTileSewageLevel.m_gatheredSewageValues[receiverTileSewageLevel.m_numGatheredSewageValues] = senderTileSewageLevel.value;
        receiverTileSewageLevel.m_numGatheredSewageValues++;

        // TURBIDITY
        Turbidity senderTileTurbidity = senderTile.GetComponent<Turbidity>();
        Turbidity receiverTileTurbidity = receiverTile.GetComponent<Turbidity>();
        // Add value to the array at specified index and then increment the index
        receiverTileTurbidity.m_gatheredTurbidityValues[receiverTileTurbidity.m_numGatheredTurbidityValues] = senderTileTurbidity.value;
        receiverTileTurbidity.m_numGatheredTurbidityValues++;

        // WATER TEMPERATURE
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();
        WaterTemperature receiverTileWaterTemperature = receiverTile.GetComponent<WaterTemperature>();
        // Add value to the array at specified index and then increment the index
        receiverTileWaterTemperature.m_gatheredWaterTemperatureValues[receiverTileWaterTemperature.m_numGatheredWaterTemperatureValues] = senderTileWaterTemperature.value;
        receiverTileWaterTemperature.m_numGatheredWaterTemperatureValues++;
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // Take the average of the gathered values and update own value

        // INSECT POPULATION
        InsectPopulation senderTileInsectPopulation = senderTile.GetComponent<InsectPopulation>();
        if (senderTileInsectPopulation.m_numGatheredInsectPopValues != 0)
        {
            senderTileInsectPopulation.value = MathUtility.FloatArrayAverage(senderTileInsectPopulation.m_gatheredInsectPopValues, senderTileInsectPopulation.m_numGatheredInsectPopValues);
        }
        senderTileInsectPopulation.m_numGatheredInsectPopValues = 0;

        // POLLUTION LEVEL
        PollutionLevel senderTilePollutionLevel = senderTile.GetComponent<PollutionLevel>();
        if (senderTilePollutionLevel.m_numGatheredPollutionValues != 0)
        {
            senderTilePollutionLevel.value = MathUtility.FloatArrayAverage(senderTilePollutionLevel.m_gatheredPollutionValues, senderTilePollutionLevel.m_numGatheredPollutionValues);
        }
        senderTilePollutionLevel.m_numGatheredPollutionValues = 0;

        // SEWAGE LEVEL
        SewageLevel senderTileSewageLevel = senderTile.GetComponent<SewageLevel>();
        if (senderTileSewageLevel.m_numGatheredSewageValues != 0)
        {
            senderTileSewageLevel.value = MathUtility.FloatArrayAverage(senderTileSewageLevel.m_gatheredSewageValues, senderTileSewageLevel.m_numGatheredSewageValues);
        }
        senderTileSewageLevel.m_numGatheredSewageValues = 0;

        // TURBIDITY
        Turbidity senderTileTurbidity = senderTile.GetComponent<Turbidity>();
        if (senderTileTurbidity.m_numGatheredTurbidityValues != 0)
        {
            senderTileTurbidity.value = MathUtility.FloatArrayAverage(senderTileTurbidity.m_gatheredTurbidityValues, senderTileTurbidity.m_numGatheredTurbidityValues);
        }
        senderTileTurbidity.m_numGatheredTurbidityValues = 0;

        // WATER TEMPERATURE
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();
        if (senderTileWaterTemperature.m_numGatheredWaterTemperatureValues != 0)
        {
            senderTileWaterTemperature.value = MathUtility.FloatArrayAverage(senderTileWaterTemperature.m_gatheredWaterTemperatureValues, senderTileWaterTemperature.m_numGatheredWaterTemperatureValues);
        }
        senderTileWaterTemperature.m_numGatheredWaterTemperatureValues = 0;     
    }
}
