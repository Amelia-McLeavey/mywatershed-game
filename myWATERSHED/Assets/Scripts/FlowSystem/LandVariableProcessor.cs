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

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // EROSION RATE
        senderTile.GetComponent<ErosionRate>().m_ErosionRate = senderTile.GetComponent<LandHeight>().m_LandHeight / 100;
        // WATER TEMPERATURE
        senderTile.GetComponent<WaterTemperature>().m_waterTemperature = senderTile.GetComponent<AsphaltDensity>().m_AsphaltDensity * 30f;
    }
}
