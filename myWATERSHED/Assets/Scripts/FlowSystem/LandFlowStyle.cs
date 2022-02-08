using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LandFlowStyle : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        Tile senderTileScript = senderTile.GetComponent<Tile>();
        Tile receiverTileScript = receiverTile.GetComponent<Tile>();

        if (senderTileScript.m_Basetype != BaseType.Land)
        {
            return false;
        }

        // Check if any of the neighbouring tiles are able to recieve data
        if (receiverTileScript.m_Basetype == BaseType.Water)
        {
            return true;
        }
        else if (receiverTileScript.m_Basetype == BaseType.Land)
        {
            return true;
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
        Debug.Assert(senderTile.GetComponent<PollutionLevel>() != null, $"senderTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<PollutionLevel>() != null, $"receiverTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<SewageLevel>() != null, $"senderTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<SewageLevel>() != null, $"receiverTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<WaterTemperature>() != null, $"senderTile is missing a LandTemperature component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<WaterTemperature>() != null, $"recieverTile is missing a LandTemperature component at index {tileIndexForDebugging}");
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
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

        // WATER TEMPERATURE
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();
        WaterTemperature receiverTileWaterTemperature = receiverTile.GetComponent<WaterTemperature>();
        // Add value to the array at specified index and then increment the index
        receiverTileWaterTemperature.m_gatheredWaterTemperatureValues[receiverTileWaterTemperature.m_numGatheredWaterTemperatureValues] = senderTileWaterTemperature.value;
        receiverTileWaterTemperature.m_numGatheredWaterTemperatureValues++;
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // POLLUTION LEVEL
        PollutionLevel senderTilePollutionLevel = senderTile.GetComponent<PollutionLevel>();

        if (senderTilePollutionLevel.m_numGatheredPollutionValues != 0)
        {
            senderTilePollutionLevel.value = MathUtility.FloatArrayAverage(senderTilePollutionLevel.m_gatheredPollutionValues, senderTilePollutionLevel.m_numGatheredPollutionValues);
        }
        // Clear the array for next cycle
        senderTilePollutionLevel.m_numGatheredPollutionValues = 0;

        // SEWAGE LEVEL
        SewageLevel senderTileSewageLevel = senderTile.GetComponent<SewageLevel>();

        if (senderTileSewageLevel.m_numGatheredSewageValues != 0)
        {
            senderTileSewageLevel.value = MathUtility.FloatArrayAverage(senderTileSewageLevel.m_gatheredSewageValues, senderTileSewageLevel.m_numGatheredSewageValues);
        }
        // Clear the array for next cycle
        senderTileSewageLevel.m_numGatheredSewageValues = 0;

        // WATER TEMPERATURE
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();

        if (senderTileWaterTemperature.m_numGatheredWaterTemperatureValues != 0)
        {
            senderTileWaterTemperature.value = MathUtility.FloatArrayAverage(senderTileWaterTemperature.m_gatheredWaterTemperatureValues, senderTileWaterTemperature.m_numGatheredWaterTemperatureValues);
        }
        // Clear the arrays for next cycle
        senderTileWaterTemperature.m_numGatheredWaterTemperatureValues = 0;
    }
}
