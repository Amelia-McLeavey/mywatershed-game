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
        // TODO: Finish migrating cached scripts up
        InsectPopulation senderTileInsectPopulation = senderTile.GetComponent<InsectPopulation>();
        RiparianLevel senderTileRiparianLevel = senderTile.GetComponent<RiparianLevel>();
        RateOfFlow senderTileRateOfFlow = senderTile.GetComponent<RateOfFlow>();
        PollutionLevel senderTilePollutionLevel = senderTile.GetComponent<PollutionLevel>();
        SewageLevel senderTileSewageLevel = senderTile.GetComponent<SewageLevel>();
        ShadeCoverage senderTileShadeCoverage = senderTile.GetComponent<ShadeCoverage>();

        // TODO: Check why this is like this
        // EROSION RATE
        if (senderTileInsectPopulation == null)
        {
            Debug.LogError("insectpopulation missing");
        }
        if (senderTileRiparianLevel == null)
        {
            Debug.LogError("riparianlevel missing");
        }

        // INSECT POPULATION
        senderTileInsectPopulation.value = Mathf.RoundToInt(senderTileRiparianLevel.value * 1000);
        // RATE OF FLOW
        senderTileRateOfFlow.value = 1 - senderTile.GetComponent<Sinuosity>().value;
        // RIVERBED HEALTH
        senderTile.GetComponent<RiverbedHealth>().value = 
            Mathf.RoundToInt((1 - senderTilePollutionLevel.value + (1 - senderTileSewageLevel.value) / 2) * 100);
        // SHADE COVERAGE
        senderTileShadeCoverage.value = senderTileRiparianLevel.value;
        // TURBIDITY
        senderTile.GetComponent<Turbidity>().value =
            (senderTilePollutionLevel.value + senderTileSewageLevel.value + senderTileRateOfFlow.value) / 3;
        // WATER TEMPERATURE
        senderTile.GetComponent<WaterTemperature>().value =
            (1 - senderTileShadeCoverage.value) * 30f - ((1 - senderTile.GetComponent<WaterDepth>().value) / 10 * 15f);
    }
}
