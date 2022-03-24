using System.Collections;
using UnityEngine;

public class CreekChubInitializationFlow : FlowStyle
{
    private int m_totalRemaining;
    private int m_desiredPopulation;

    public CreekChubInitializationFlow(int totalRedDacePopulation)
    {
        m_desiredPopulation = Mathf.CeilToInt((float)totalRedDacePopulation / 10.0f);
        m_totalRemaining = m_desiredPopulation;
    }

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
        int averagePerTile = Mathf.CeilToInt((float)m_desiredPopulation / (float)m_cachedUsableTiles.Count);

        Debug.Assert(senderTile.GetComponent<CreekChubPopulation>() != null);

        int creekChubInThisTile = Random.Range(0, averagePerTile * 2);
        creekChubInThisTile = Mathf.Clamp(creekChubInThisTile, 0, m_totalRemaining);
        m_totalRemaining -= creekChubInThisTile;

        senderTile.GetComponent<CreekChubPopulation>().value = creekChubInThisTile;
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }
}