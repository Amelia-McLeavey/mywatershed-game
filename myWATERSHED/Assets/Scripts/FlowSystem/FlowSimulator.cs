using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sends request to update info on all tiles.
/// </summary>

public class FlowSimulator : MonoBehaviour
{
    private int m_rows;
    private int m_columns;

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

        m_rows = rows;
        m_columns = columns;

        WaterFlowStyle waterFlowStyle = new WaterFlowStyle();
        LandFlowStyle landFlowStyle = new LandFlowStyle();

        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
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
        SendTwoStageFlow(new LandVariableProcessor(), BaseType.Land);
        s_landVariableMarker.End();

        s_waterVariableMarker.Begin();
        SendTwoStageFlow(new WaterVariableProcessor(), BaseType.Water);
        s_waterVariableMarker.End();

        s_landFlowMarker.Begin();
        SendTwoStageFlow(new LandFlowStyle(), BaseType.Land);
        s_landFlowMarker.End();

        s_waterFlowMarker.Begin();
        SendTwoStageFlow(new WaterFlowStyle(), BaseType.Water);
        s_waterFlowMarker.End();

        s_updateFlowMarker.End();
    }



    private void SendTwoStageFlow(FlowStyle flowStyle, BaseType baseType)
    {
        // Reset the list then scatter by adding sender tile current value to the recievers' list
        for (int x = m_rows; x > 0; x--)
        {
            for (int y = m_columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    // Make sure we don't call the wrong flow
                    if (value.GetComponent<Tile>().m_Basetype == baseType)
                    {
                        SendDistributionPulse(value, flowStyle, tileIndex); // scatter
                    }
                }
            }
        }
        // Take the average of the gathered values and update own value
        for (int x = m_rows; x > 0; x--)
        {
            for (int y = m_columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == baseType)
                    {
                        SendProcessingPulse(value, flowStyle, tileIndex);
                    }
                }
            }
        }
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
