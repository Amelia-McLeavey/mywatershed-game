using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlowStyle : FlowStyle
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
        // DEBUGS BE DEFENSIVE 
        Debug.Assert(senderTile != null, $"senderTile is null at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile != null, $"receiverTile is null at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<BrownTroutPopulation>() != null, $"senderTile is missing a BrownTroutPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<BrownTroutPopulation>() != null, $"senderTile is missing a BrownTroutPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<CreekChubPopulation>() != null, $"senderTile is missing a CreekChubPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<CreekChubPopulation>() != null, $"receiverTile is missing a CreekChubPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<RedDacePopulation>() != null, $"senderTile is missing a RedDacePopulation component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<RedDacePopulation>() != null, $"receiverTile is missing a RedDacePopulation component at index {tileIndexForDebugging}");

        // BROWN TROUT POPULATION
        senderTile.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation -= 1;
        receiverTile.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation += 1;
        // CREEK CHUB POPULATION
        senderTile.GetComponent<CreekChubPopulation>().m_CreekChubPopulation -= 1;
        receiverTile.GetComponent<CreekChubPopulation>().m_CreekChubPopulation += 1;
        // RED DACE POPULATION
        senderTile.GetComponent<RedDacePopulation>().m_RedDacePopulation -= 1;
        receiverTile.GetComponent<RedDacePopulation>().m_RedDacePopulation += 1;
    }
}
