using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private void OnEnable()
    {
        SystemGenerator.OnSystemGenerationComplete += OnSystemGenerationComplete;
        GetComponent<FlowTimer>().OnFlowControlTimerTick += SimulationTimer;
    }

    private void OnDisable()
    {
        SystemGenerator.OnSystemGenerationComplete -= OnSystemGenerationComplete;
        GetComponent<FlowTimer>().OnFlowControlTimerTick -= SimulationTimer;
    }

    private void OnSystemGenerationComplete(int rows, int columns)
    {

        GetComponent<FlowSimulator>().OnSystemGenerationComplete(rows, columns);
        GetComponent<FishSimulator>().OnSystemGenerationComplete(rows, columns);
    }

    private void SimulationTimer(int frameIndex)
    {
        GetComponent<FlowSimulator>().UpdateFlow(frameIndex);

        if (frameIndex == 3)
        {
            GetComponent<FishSimulator>().UpdateFauna();
        }
    }
}
