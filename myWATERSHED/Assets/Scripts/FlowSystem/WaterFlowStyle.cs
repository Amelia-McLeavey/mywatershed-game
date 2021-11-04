using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script contains flow logic for all abiotic components pertaining to water tiles

public class WaterFlowStyle : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile)
    {
        Tile tileScript = receiverTile.GetComponent<Tile>();

        // Check if any of the neighbouring tiles are able to recieve data
        if (tileScript.m_Basetype == BaseType.Water)
        {
            return true;
        }
        else if (tileScript.m_PhysicalType == PhysicalType.Highway)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Flow(GameObject senderTile, GameObject receiverTile)
    {
        senderTile.GetComponent<PollutionLevel>().m_PolutionLevel -= 1;
        receiverTile.GetComponent<PollutionLevel>().m_PolutionLevel += 1;

        senderTile.GetComponent<SewageLevel>().m_SewageLevel -= 1;
        receiverTile.GetComponent<SewageLevel>().m_SewageLevel += 1;

        senderTile.GetComponent<Turbidity>().m_Turbidity -= 1;
        receiverTile.GetComponent<Turbidity>().m_Turbidity += 1;

        senderTile.GetComponent<WaterTemperature>().m_waterTemperature -= 1;
        receiverTile.GetComponent<WaterTemperature>().m_waterTemperature += 1;
    }
}
