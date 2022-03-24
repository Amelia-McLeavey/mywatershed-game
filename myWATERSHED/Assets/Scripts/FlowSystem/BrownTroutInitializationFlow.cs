using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownTroutInitializationFlow : FlowStyle
{
    private int m_totalRemaining;
    private int m_desiredPopulation;

    public BrownTroutInitializationFlow(int redDacePopulation)
    {
        m_totalRemaining = Mathf.CeilToInt((float)redDacePopulation / 10.0f);
        m_desiredPopulation = m_totalRemaining;
    }

    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    private float GetTileTypeFactor(GameObject senderTile)
    {
        switch (senderTile.GetComponent<Tile>().m_PhysicalType)
        {
            case PhysicalType.EngineeredReservoir:
                return 1;

            case PhysicalType.EngineeredStream:
                return 1;

            case PhysicalType.NaturalReservoir:
                return 2;

            case PhysicalType.NaturalStream:
                return 2;

            case PhysicalType.Wetland:
                return 1;

            default:
                Debug.Assert(false, "Unhandled tile type in CalculateRedDaceInTile");
                return 0;
        }
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        int averagePerTile = Mathf.CeilToInt((float)m_desiredPopulation / (float)m_cachedUsableTiles.Count);
        int randomFishAmount = Random.Range(0, averagePerTile * 2);

        float tileTypeFactor = GetTileTypeFactor(senderTile);
        int fishInThisTile = Mathf.CeilToInt(randomFishAmount * tileTypeFactor);
        fishInThisTile = Mathf.Clamp(fishInThisTile, 0, m_totalRemaining);

        m_totalRemaining -= fishInThisTile;

        senderTile.GetComponent<BrownTroutPopulation>().value = fishInThisTile;
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }
}
