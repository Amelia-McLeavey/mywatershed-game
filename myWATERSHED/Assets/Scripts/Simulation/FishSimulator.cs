using System.Collections.Generic;
using UnityEngine;

// TODO: Rename this to 'FaunaSimulator'
public class FishSimulator : MonoBehaviour
{
    public World m_worldScript;

    [SerializeField]
    private RedDaceUpdateFlow m_redDaceUpdateFlow = new RedDaceUpdateFlow();
    [SerializeField]
    private BrownTroutUpdateFlow m_brownTroutUpdateFlow = new BrownTroutUpdateFlow();
    [SerializeField]
    private CreekChubUpdateFlow m_creekChubUpdateFlow = new CreekChubUpdateFlow();

#if UNITY_EDITOR
    private FaunaPopulationDebugDrawFlow m_populationDebugDraw = new FaunaPopulationDebugDrawFlow();
#endif // #if UNITY_EDITOR

    private UpdateTotalFaunaPopulationFlow m_updateTotalFaunaPopulationFlow = new UpdateTotalFaunaPopulationFlow();

    [SerializeField]
    private int m_startingRedDacePopulation = 1000;

    public void OnSystemGenerationComplete(int rows, int columns)
    {
        CreateFaunaFlows(rows, columns);
        InitializeFaunaDistribution(rows, columns);
        UpdateTotalFaunaPopulation();
    }

    private void CreateFaunaFlows(int rows, int columns)
    {
        m_redDaceUpdateFlow.CacheTiles(rows, columns, BaseType.Water, FlowStyle.FlowDirection.TopDown);
        m_brownTroutUpdateFlow.CopyCachedTiles(m_redDaceUpdateFlow);
        m_creekChubUpdateFlow.CopyCachedTiles(m_redDaceUpdateFlow);

        m_updateTotalFaunaPopulationFlow.CopyCachedTiles(m_redDaceUpdateFlow);

#if UNITY_EDITOR
        m_populationDebugDraw.CopyCachedTiles(m_redDaceUpdateFlow);
#endif // #if UNITY_EDITOR
    }

    private void InitializeFaunaDistribution(int rows, int columns)
    {
        RedDaceInitializationFlow redDaceInitializationFlow = new RedDaceInitializationFlow(m_startingRedDacePopulation);
        redDaceInitializationFlow.CopyCachedTiles(m_redDaceUpdateFlow);
        FlowUtils.SendOneStageFlow(redDaceInitializationFlow);
        
        int totalRedDaceSpawned = redDaceInitializationFlow.GetTotalNumberSpawned();

        CreekChubInitializationFlow creekChubFlow = new CreekChubInitializationFlow(totalRedDaceSpawned);
        creekChubFlow.CopyCachedTiles(m_redDaceUpdateFlow);
        FlowUtils.SendOneStageFlow(creekChubFlow);

        BrownTroutInitializationFlow brownTrouInitializationFlow = new BrownTroutInitializationFlow(totalRedDaceSpawned);
        brownTrouInitializationFlow.CopyCachedTiles(m_redDaceUpdateFlow);
        FlowUtils.SendOneStageFlow(brownTrouInitializationFlow);
    }

    public void UpdateFauna()
    {
        UpdateTileFaunaPopulation();
        UpdateTotalFaunaPopulation();
    }

    private static Unity.Profiling.ProfilerMarker s_updateFaunaCountPopMarker = new Unity.Profiling.ProfilerMarker("FishSimulator.UpdateTotalFaunaPopulation");
    private void UpdateTotalFaunaPopulation()
    {
        s_updateFaunaCountPopMarker.Begin();

        m_updateTotalFaunaPopulationFlow.PrepareForProcessing();

        FlowUtils.SendOneStageFlow(m_updateTotalFaunaPopulationFlow);

        m_updateTotalFaunaPopulationFlow.FinalizeProcessing(m_worldScript);

        s_updateFaunaCountPopMarker.End();
    }

    private static Unity.Profiling.ProfilerMarker s_updateFaunaPopMarker = new Unity.Profiling.ProfilerMarker("FishSimulator.UpdateTileFaunaPopulation");
    private void UpdateTileFaunaPopulation()
    {
        s_updateFaunaPopMarker.Begin();

        FlowUtils.SendOneStageFlow(m_redDaceUpdateFlow);
        FlowUtils.SendOneStageFlow(m_brownTroutUpdateFlow);
        FlowUtils.SendOneStageFlow(m_creekChubUpdateFlow);

        s_updateFaunaPopMarker.End();
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        FlowUtils.SendOneStageFlow(m_populationDebugDraw);
#endif // #if UNITY_EDITOR
    }
}
