using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandFlowStyle : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        // Check if any of the neighbouring tiles are able to recieve data
        return false;
    }

    public override void Flow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
    }
}
