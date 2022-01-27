using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sends request to update info on all tiles.
/// </summary>

public class FlowSimulator : MonoBehaviour
{
    // Create a single instance of each flow style
    LandVariableProcessor m_landVariableProcessor = new LandVariableProcessor();
    WaterVariableProcessor m_waterVariableProcessor = new WaterVariableProcessor();
    LandFlowStyle m_landFlowStyle = new LandFlowStyle();
    WaterFlowStyle m_waterFlowStyle = new WaterFlowStyle();

    private void OnEnable()
    {
        SystemGenerator.OnSystemGenerationComplete += InitializeFlow;
    }

    private void OnDisable()
    {
        SystemGenerator.OnSystemGenerationComplete -= InitializeFlow;
    }

    // Initializes tiles by finding the nieghbours of each tile that will receive information
    private void InitializeFlow(int rows, int columns)
    {
        //Debug.Log("INITIALIZE FLOW");

        WaterFlowStyle waterFlowStyle = new WaterFlowStyle();
        LandFlowStyle landFlowStyle = new LandFlowStyle();

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    SetNeighbours(value, tileIndex);
                    SetStartingVariableValues(value, tileIndex);

                    List<GameObject> receiverTiles = GetNeighbours(value);

                    foreach (GameObject receiverTile in receiverTiles)
                    {
                        if (waterFlowStyle.CanFlow(value, receiverTile, tileIndex))
                        {
                            waterFlowStyle.VerifyTiles(value, receiverTile, tileIndex);
                        }

                        if (landFlowStyle.CanFlow(value, receiverTile, tileIndex))
                        {
                            landFlowStyle.VerifyTiles(value, receiverTile, tileIndex);
                        }
                    }
                }
            }
        }

        // Cache tiles on each flow style
        m_landVariableProcessor.CacheTiles(rows, columns, BaseType.Land);
        m_waterVariableProcessor.CacheTiles(rows, columns, BaseType.Water);
        m_landFlowStyle.CacheTiles(rows, columns, BaseType.Land);
        m_waterFlowStyle.CacheTiles(rows, columns, BaseType.Water);

    }

    private void SetNeighbours(GameObject tile, Vector2 tileIndex)
    {
        Tile tileScript = tile.GetComponent<Tile>();
        List<Vector2> neighbourIndices = NeighbourUtility.FindAllNeighbours(tileIndex);

        if (tileScript.m_Basetype == BaseType.Water)
        {
            tileScript.m_receiverNeighbours = NeighbourUtility.FindLowestNeighbours(neighbourIndices);
        }

        if (tileScript.m_Basetype == BaseType.Land)
        {
            tileScript.m_receiverNeighbours = NeighbourUtility.FindLowestNeighbour(neighbourIndices, tile);
        }
    }

    private void SetStartingVariableValues(GameObject tile, Vector2 tileIndex)
    {
        SendProcessingPulse(tile, new VariableInitialization(), tileIndex);

    }

    private static Unity.Profiling.ProfilerMarker s_updateFlowMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.UpdateFlow");

    private static Unity.Profiling.ProfilerMarker s_landVariableMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.LandVariableProcessor");
    private static Unity.Profiling.ProfilerMarker s_waterVariableMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.WaterVariableProcessor");
    private static Unity.Profiling.ProfilerMarker s_landFlowMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.LandFlowStyle");
    private static Unity.Profiling.ProfilerMarker s_waterFlowMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.WaterFlowStyle");

    public void UpdateFlow()
    {
        s_updateFlowMarker.Begin();

        s_landVariableMarker.Begin();
        SendTwoStageFlow(m_landVariableProcessor);
        s_landVariableMarker.End();

        s_waterVariableMarker.Begin();
        SendTwoStageFlow(m_waterVariableProcessor);
        s_waterVariableMarker.End();

        s_landFlowMarker.Begin();
        SendTwoStageFlow(m_landFlowStyle);
        s_landFlowMarker.End();

        s_waterFlowMarker.Begin();
        SendTwoStageFlow(m_waterFlowStyle);
        s_waterFlowMarker.End();

        s_updateFlowMarker.End();
    }

    private static Unity.Profiling.ProfilerMarker s_distributeDataMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.SendDistributionPulse");
    private static Unity.Profiling.ProfilerMarker s_processDataMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.SendProcessingPulse");

    private void SendTwoStageFlow(FlowStyle flowStyle)
    {

        s_distributeDataMarker.Begin();

        // Reset the list then scatter by adding sender tile current value to the recievers' list
        foreach (KeyValuePair<Vector2, GameObject> kvp in flowStyle.m_cachedUsableTiles)
        {
            SendDistributionPulse(kvp.Value, flowStyle, kvp.Key); // scatter
        }

        s_distributeDataMarker.End();
        s_processDataMarker.Begin();

        // Take the average of the gathered values and update own value
        foreach (KeyValuePair<Vector2, GameObject> kvp in flowStyle.m_cachedUsableTiles)
        {
            SendProcessingPulse(kvp.Value, flowStyle, kvp.Key);
        }

        s_processDataMarker.End();
    }

    private void SendDistributionPulse(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
    {
        List<GameObject> receiverTiles = GetNeighbours(senderTile);

        foreach (GameObject receiverTile in receiverTiles)
        {
            if (flowStyle.CanFlow(senderTile, receiverTile, indexForDebugging))
            {
                flowStyle.DistrubuteData(senderTile, receiverTile, indexForDebugging);
            }
        }
    }

    private void SendProcessingPulse(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
    {
        flowStyle.ProcessData(senderTile, indexForDebugging);
     
    }

    private List<GameObject> GetNeighbours(GameObject senderTile)
    {
        return senderTile.GetComponent<Tile>().m_receiverNeighbours;
    }
}
