using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandVariableProcessor : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        return false;
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // EROSION RATE
        senderTile.GetComponent<ErosionRate>().value = senderTile.GetComponent<LandHeight>().value / 100;
        // WATER TEMPERATURE
        senderTile.GetComponent<WaterTemperature>().value = senderTile.GetComponent<AsphaltDensity>().value * 30f;
    }
}
