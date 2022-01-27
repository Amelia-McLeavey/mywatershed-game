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
        senderTilePollutionLevel.m_GatheredPolutionValues.Clear();
        receiverTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Add(senderTilePollutionLevel.value);
        // SEWAGE LEVEL
        SewageLevel senderTileSewageLevel = senderTile.GetComponent<SewageLevel>();
        senderTileSewageLevel.m_GatheredSewageValues.Clear();
        receiverTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Add(senderTileSewageLevel.value);
        // WATER TEMPERATURE
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();
        senderTileWaterTemperature.m_GatheredWaterTemperatureValues.Clear();
        receiverTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Add(senderTileWaterTemperature.value);
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
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
        // WATER TEMPERATURE\
        WaterTemperature senderTileWaterTemperature = senderTile.GetComponent<WaterTemperature>();
        if (senderTileWaterTemperature.m_GatheredWaterTemperatureValues.Count != 0)
        {
            senderTileWaterTemperature.value = senderTileWaterTemperature.m_GatheredWaterTemperatureValues.Average();
        }
    }
}
