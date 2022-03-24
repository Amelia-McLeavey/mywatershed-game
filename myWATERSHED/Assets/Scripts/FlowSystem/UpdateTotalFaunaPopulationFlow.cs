using System.Collections;
using UnityEngine;

public class UpdateTotalFaunaPopulationFlow : FlowStyle
{
    private int m_totalDacePop = 0;
    private int m_totalCreekChubPop = 0;
    private int m_totalTroutPop = 0;
    private int m_totalInsectPop = 0;

    public void PrepareForProcessing()
    {
        m_totalDacePop = 0;
        m_totalCreekChubPop = 0;
        m_totalTroutPop = 0;
        m_totalInsectPop = 0;
    }

    public void FinalizeProcessing(World worldScript)
    {
        worldScript.UpdateTotalFaunaPopulations(m_totalDacePop, m_totalCreekChubPop, m_totalTroutPop, m_totalInsectPop);
    }

    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // TODO: Figure out which insect total to calculate/display
        RedDacePopulation redDaceComponent = senderTile.GetComponent<RedDacePopulation>();
        CreekChubPopulation creekChubComponent = senderTile.GetComponent<CreekChubPopulation>();
        BrownTroutPopulation brownTroutComponent = senderTile.GetComponent<BrownTroutPopulation>();
        InsectPopulation insectComponent = senderTile.GetComponent<InsectPopulation>();

        m_totalDacePop += (int)redDaceComponent.value;
        m_totalCreekChubPop += (int)creekChubComponent.value;
        m_totalTroutPop += (int)brownTroutComponent.value;
        m_totalInsectPop += (int)insectComponent.value;
    }
}