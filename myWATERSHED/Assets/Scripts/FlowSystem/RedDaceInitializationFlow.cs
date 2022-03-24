using System.Collections;
using UnityEngine;


public class RedDaceInitializationFlow : FlowStyle
{
    private int m_totalRemaining;
    private int m_desiredPopulation;

    public RedDaceInitializationFlow(int redDacePopulation)
    {
        m_desiredPopulation = redDacePopulation;
        m_totalRemaining = redDacePopulation;
    }

    public int GetTotalNumberSpawned()
    {
        return m_desiredPopulation - m_totalRemaining;
    }

    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    // This will give us a multiplier to use when we generate a random number of fish per tile
    // Some tiles have more fish than others and this lets us control that ratio
    private int GetTileTypeFactor(GameObject senderTile)
    {
        switch (senderTile.GetComponent<Tile>().m_PhysicalType)
        {
            case PhysicalType.EngineeredReservoir:
                return 0;

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

        int tileTypeFactor = GetTileTypeFactor(senderTile);
        int fishInThisTile = randomFishAmount * tileTypeFactor;
        fishInThisTile = Mathf.Clamp(fishInThisTile, 0, m_totalRemaining);

        m_totalRemaining -= fishInThisTile;

        senderTile.GetComponent<RedDacePopulation>().value = fishInThisTile;
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }
}