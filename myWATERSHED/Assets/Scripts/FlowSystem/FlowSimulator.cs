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

    public void OnSystemGenerationComplete(int rows, int columns)
    {
        InitializeFlow(rows, columns);
    }

    // Initializes tiles by finding the nieghbours of each tile that will receive information
    private void InitializeFlow(int rows, int columns)
    {
        //Debug.Log("INITIALIZE FLOW");

        WaterFlowStyle waterFlowStyle = new WaterFlowStyle();
        LandFlowStyle landFlowStyle = new LandFlowStyle();

        VariableInitialization initFlow = new VariableInitialization();

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    SetNeighbours(value, tileIndex);

                    // Variable Initialization Flow
                    FlowUtils.SendProcessingPulse(value, initFlow, tileIndex);

                    List<GameObject> receiverTiles = FlowUtils.GetNeighbours(value);

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
        m_landVariableProcessor.CacheTiles(rows, columns, BaseType.Land, FlowStyle.FlowDirection.BottomUp);
        m_landFlowStyle.CopyCachedTiles(m_landVariableProcessor);

        m_waterVariableProcessor.CacheTiles(rows, columns, BaseType.Water, FlowStyle.FlowDirection.BottomUp);
        m_waterFlowStyle.CopyCachedTiles(m_waterVariableProcessor);
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

    private static Unity.Profiling.ProfilerMarker s_updateFlowMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.UpdateFlow");

    private static Unity.Profiling.ProfilerMarker s_landVariableMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.LandVariableProcessor");
    private static Unity.Profiling.ProfilerMarker s_waterVariableMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.WaterVariableProcessor");
    private static Unity.Profiling.ProfilerMarker s_landFlowMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.LandFlowStyle");
    private static Unity.Profiling.ProfilerMarker s_waterFlowMarker = new Unity.Profiling.ProfilerMarker("FlowSimulator.WaterFlowStyle");

    public void UpdateFlow(int frameIndex)
    {
        s_updateFlowMarker.Begin();

        // Split processing into seperate frames
        switch(frameIndex)
        {
            case 0:
                s_landVariableMarker.Begin();
                FlowUtils.SendOneStageFlow(m_landVariableProcessor);
                s_landVariableMarker.End();
                break;

            case 1:
                s_waterVariableMarker.Begin();
                FlowUtils.SendOneStageFlow(m_waterVariableProcessor);
                s_waterVariableMarker.End();
                break;

            case 2:
                s_landFlowMarker.Begin();
                FlowUtils.SendTwoStageFlow(m_landFlowStyle);
                s_landFlowMarker.End();
                break;

            case 3:
                s_waterFlowMarker.Begin();
                FlowUtils.SendTwoStageFlow(m_waterFlowStyle);
                s_waterFlowMarker.End();
                break;
        }

        s_updateFlowMarker.End();
    }
}
