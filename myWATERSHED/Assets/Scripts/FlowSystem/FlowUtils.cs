using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlowUtils
{
    private static Unity.Profiling.ProfilerMarker s_distributeDataMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.SendDistributionPulse");
    private static Unity.Profiling.ProfilerMarker s_processDataMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.SendProcessingPulse");

    public static void SendOneStageFlow(FlowStyle flowStyle)
    {
        s_processDataMarker.Begin();
        foreach (KeyValuePair<Vector2, GameObject> kvp in flowStyle.m_cachedUsableTiles)
        {
            SendProcessingPulse(kvp.Value, flowStyle, kvp.Key);
        }

        s_processDataMarker.End();
    }

    public static void SendTwoStageFlow(FlowStyle flowStyle)
    {

        s_distributeDataMarker.Begin();

        // Reset the list then scatter by adding sender tile current value to the recievers' list
        foreach (KeyValuePair<Vector2, GameObject> kvp in flowStyle.m_cachedUsableTiles)
        {
            SendDistributionPulse(kvp.Value, flowStyle, kvp.Key); // scatter
        }

        s_distributeDataMarker.End();

        // Take the average of the gathered values and update own value
        SendOneStageFlow(flowStyle);
    }

    private static void SendDistributionPulse(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
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

    public static void SendProcessingPulse(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
    {
        flowStyle.ProcessData(senderTile, indexForDebugging);
    }

    public static List<GameObject> GetNeighbours(GameObject senderTile)
    {
        return senderTile.GetComponent<Tile>().m_receiverNeighbours;
    }
}
