using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script contains flow logic for all abiotic components pertaining to water tiles

public class WaterFlowStyle : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        Tile senderTileScript = senderTile.GetComponent<Tile>();
        Tile receiverTileScript = receiverTile.GetComponent<Tile>();

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

    public override void Flow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        Debug.Assert(senderTile != null, $"senderTile is null at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile != null, $"receiverTile is null at index {tileIndexForDebugging}");

        Debug.Assert(senderTile.GetComponent<InsectPopulation>() != null, $"senderTile is missing an InsectPopulation component at index {tileIndexForDebugging}");
        senderTile.GetComponent<InsectPopulation>().m_InsectPopulation -= 1;
        Debug.Assert(receiverTile.GetComponent<InsectPopulation>() != null, $"recieverTile is missing an InsectPopulation component at index {tileIndexForDebugging}");
        receiverTile.GetComponent<InsectPopulation>().m_InsectPopulation += 1;

        Debug.Assert(senderTile.GetComponent<PollutionLevel>() != null, $"senderTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        senderTile.GetComponent<PollutionLevel>().m_PolutionLevel -= 1;
        Debug.Assert(receiverTile.GetComponent<PollutionLevel>() != null, $"receiverTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        receiverTile.GetComponent<PollutionLevel>().m_PolutionLevel += 1;

        Debug.Assert(senderTile.GetComponent<SewageLevel>() != null, $"senderTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        senderTile.GetComponent<SewageLevel>().m_SewageLevel -= 1;
        Debug.Assert(receiverTile.GetComponent<SewageLevel>() != null, $"receiverTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        receiverTile.GetComponent<SewageLevel>().m_SewageLevel += 1;

        Debug.Assert(senderTile.GetComponent<Turbidity>() != null, $"senderTile is missing a Turbidity component at index {tileIndexForDebugging}");
        senderTile.GetComponent<Turbidity>().m_Turbidity -= 1;
        Debug.Assert(receiverTile.GetComponent<Turbidity>() != null, $"receiverTile is missing a Turbidity component at index {tileIndexForDebugging}");
        receiverTile.GetComponent<Turbidity>().m_Turbidity += 1;

        Debug.Assert(senderTile.GetComponent<WaterTemperature>() != null, $"senderTile is missing a WaterTemperature component at index {tileIndexForDebugging}");
        senderTile.GetComponent<WaterTemperature>().m_waterTemperature -= 1;
        Debug.Assert(receiverTile.GetComponent<WaterTemperature>() != null, $"receiverTile is missing a WaterTemperature component at index {tileIndexForDebugging}");
        receiverTile.GetComponent<WaterTemperature>().m_waterTemperature += 1;
    }
}
