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

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // Cache references for performance optimization 
        InsectPopulation insectPopulation = senderTile.GetComponent<InsectPopulation>();
        RiparianLevel riparianLevel = senderTile.GetComponent<RiparianLevel>();
        RateOfFlow rateOfFlow = senderTile.GetComponent<RateOfFlow>();
        PollutionLevel pollutionLevel = senderTile.GetComponent<PollutionLevel>();
        SewageLevel sewageLevel = senderTile.GetComponent<SewageLevel>();
        ShadeCoverage shadeCoverage = senderTile.GetComponent<ShadeCoverage>();
        Sinuosity sinuosity = senderTile.GetComponent<Sinuosity>();
        RiverbedHealth riverbedHealth = senderTile.GetComponent<RiverbedHealth>();
        Turbidity turbidity = senderTile.GetComponent<Turbidity>();
        WaterTemperature waterTemperature = senderTile.GetComponent<WaterTemperature>();
        WaterDepth waterDepth = senderTile.GetComponent<WaterDepth>();
        
        // TODO: Check why this is like this
        // EROSION RATE
        if (insectPopulation == null)
        {
            Debug.LogError("insectpopulation missing");
        }
        if (riparianLevel == null)
        {
            Debug.LogError("riparianlevel missing");
        }

        // INSECT POPULATION
        insectPopulation.value = Mathf.RoundToInt(riparianLevel.value * 1000);
        // RATE OF FLOW
        rateOfFlow.value = 1 - sinuosity.value;
        // RIVERBED HEALTH
        riverbedHealth.value = 
            Mathf.RoundToInt((1 - pollutionLevel.value + (1 - sewageLevel.value) / 2) * 100);
        // SHADE COVERAGE
        shadeCoverage.value = riparianLevel.value;
        // TURBIDITY
        turbidity.value =
            (pollutionLevel.value + sewageLevel.value + rateOfFlow.value) / 3;
        // WATER TEMPERATURE
        waterTemperature.value =
            (1 - shadeCoverage.value) * 30f - ((1 - waterDepth.value) / 10 * 15f);
    }
}
