using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaterVariableProcessor : FlowStyle
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
        if (senderTile.GetComponent<InsectPopulation>() == null)
        {
            Debug.LogError("erosion missing");
        }
        if (senderTile.GetComponent<RiparianLevel>() == null)
        {
            Debug.LogError("landheight missing");
        }
        // INSECT POPULATION
        senderTile.GetComponent<InsectPopulation>().value = Mathf.RoundToInt(senderTile.GetComponent<RiparianLevel>().value * 1000);
        // RATE OF FLOW
        senderTile.GetComponent<RateOfFlow>().value = 1 - senderTile.GetComponent<Sinuosity>().value;
        // RIVERBED HEALTH
        senderTile.GetComponent<RiverbedHealth>().value = 
            Mathf.RoundToInt((1 - senderTile.GetComponent<PollutionLevel>().value + (1 - senderTile.GetComponent<SewageLevel>().value) / 2) * 100);
        // SHADE COVERAGE
        senderTile.GetComponent<ShadeCoverage>().value = senderTile.GetComponent<RiparianLevel>().value;
        // TURBIDITY
        senderTile.GetComponent<Turbidity>().value =
            (senderTile.GetComponent<PollutionLevel>().value + senderTile.GetComponent<SewageLevel>().value + senderTile.GetComponent<RateOfFlow>().value) / 3;
        // WATER TEMPERATURE
        senderTile.GetComponent<WaterTemperature>().value =
            (1 - senderTile.GetComponent<ShadeCoverage>().value) * 30f - ((1 - senderTile.GetComponent<WaterDepth>().value) / 10 * 15f);
    }
}
