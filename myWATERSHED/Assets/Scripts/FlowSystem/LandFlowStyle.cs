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
        senderTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Clear();
        receiverTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Add(senderTile.GetComponent<PollutionLevel>().value);
        // SEWAGE LEVEL
        senderTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Clear();
        receiverTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Add(senderTile.GetComponent<SewageLevel>().value);
        // WATER TEMPERATURE
        senderTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Clear();
        receiverTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Add(senderTile.GetComponent<WaterTemperature>().value);
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // POLLUTION LEVEL
        if (senderTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Count != 0)
        {
            senderTile.GetComponent<PollutionLevel>().value = senderTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Average();
        }
        // SEWAGE LEVEL
        if (senderTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Count != 0)
        {
            senderTile.GetComponent<SewageLevel>().value = senderTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Average();
        }
        // WATER TEMPERATURE\
        if (senderTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Count != 0)
        {
            senderTile.GetComponent<WaterTemperature>().value = senderTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Average();
        }
    }
}
