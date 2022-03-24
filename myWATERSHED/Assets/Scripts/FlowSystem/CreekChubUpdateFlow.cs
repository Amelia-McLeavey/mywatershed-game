using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreekChubUpdateFlow : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // TODO: Figure out how creek chub spawn
        // TODO: Figure out how creek chub die
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }
}
