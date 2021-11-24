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
        // INSECT POPULATION
        senderTile.GetComponent<InsectPopulation>().m_InsectPopulation = Mathf.RoundToInt(senderTile.GetComponent<RiparianLevel>().m_RiparianLevel * 1000);
        // RATE OF FLOW
        senderTile.GetComponent<RateOfFlow>().m_RateOfFlow = 1 - senderTile.GetComponent<Sinuosity>().m_Sinuosity;
        // RIVERBED HEALTH
        senderTile.GetComponent<RiverbedHealth>().m_RiverBedHealth = 
            Mathf.RoundToInt((1 - senderTile.GetComponent<PollutionLevel>().m_PolutionLevel + (1 - senderTile.GetComponent<SewageLevel>().m_SewageLevel) / 2) * 100);
        // SHADE COVERAGE
        senderTile.GetComponent<ShadeCoverage>().m_ShadeCoverage = senderTile.GetComponent<RiparianLevel>().m_RiparianLevel;
        // TURBIDITY
        senderTile.GetComponent<Turbidity>().m_Turbidity =
            (senderTile.GetComponent<PollutionLevel>().m_PolutionLevel + senderTile.GetComponent<SewageLevel>().m_SewageLevel + senderTile.GetComponent<RateOfFlow>().m_RateOfFlow) / 3;
        // WATER TEMPERATURE
        senderTile.GetComponent<WaterTemperature>().m_waterTemperature =
            (1 - senderTile.GetComponent<ShadeCoverage>().m_ShadeCoverage) * 30f - ((1 - senderTile.GetComponent<WaterDepth>().m_WaterDepth) / 10 * 15f);
    }
}
