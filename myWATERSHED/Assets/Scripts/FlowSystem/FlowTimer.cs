using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FlowTimer : MonoBehaviour
{
    // The timer event
    public event Action OnFlowControlTimerTick;

    [SerializeField]
    public float m_flowTime;

    private World m_world;

    private void Awake()
    {
        // Check that the flowTime variable is greater than 0, stop the program if so.
        Debug.Assert(m_flowTime > 0, "flowTime must be greater than 0.");

        m_world = FindObjectOfType<World>();
    }

    private void OnEnable()
    {
        World.OnSeasonChange += CheckSeason;
    }

    private void OnDisable()
    {
        World.OnSeasonChange -= CheckSeason;
    }

    private void CheckSeason(SeasonState season)
    {
        switch (season)
        {
            case SeasonState.Summer:
                StartCoroutine(SummerLengthTimer());
                StartCoroutine(FlowControlTimer());
                break;
            case SeasonState.Winter:
                StopCoroutine(FlowControlTimer());
                break;
            default:
                Debug.LogError("SeasonState did not successfully compare to any of the cases.");
                break;
        }
    }

    private IEnumerator SummerLengthTimer()
    {
        yield return new WaitForSeconds(m_world.m_summerLengthInSeconds);
        m_world.ChangeSeason(SeasonState.Winter);
    }

    private IEnumerator FlowControlTimer()
    {
        while (m_world.m_seasonState == SeasonState.Summer)
        {
            yield return new WaitForSeconds(m_flowTime);
            OnFlowControlTimerTick?.Invoke();
            Debug.Log("TICK");
        }
    }

}
