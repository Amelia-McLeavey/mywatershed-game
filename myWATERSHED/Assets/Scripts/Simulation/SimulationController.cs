using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<FlowTimer>().OnFlowControlTimerTick += SimulationTimer;
    }

    private void OnDisable()
    {
        GetComponent<FlowTimer>().OnFlowControlTimerTick -= SimulationTimer;
    }

    private void Update()
    {
        
    }

    private void SimulationTimer()
    {

        GetComponent<FlowSimulator>().UpdateFlow();
        GetComponent<FishSimulator>().UpdateFishPopulations();
    }
}
