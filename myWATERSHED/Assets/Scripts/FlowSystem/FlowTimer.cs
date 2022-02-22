using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Controls the rate of data flow.
/// </summary>
public class FlowTimer : MonoBehaviour
{
    // The timer event
    public event Action OnFlowControlTimerTick;

    [SerializeField]
    public float m_flowTime;

    private World m_world;
    private GameManager m_gameManager;

    private VolunteerManager volunteerManager;
    private void Awake()
    {
        // Check that the flowTime variable is greater than 0, stop the program if so.
        Debug.Assert(m_flowTime > 0, "flowTime must be greater than 0.");
        m_gameManager = GameManager.Instance;
        volunteerManager = GameObject.FindObjectOfType<VolunteerManager>();
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
        int sec = 0;
        while (sec < m_world.m_summerLengthInSeconds)
        {
            yield return new WaitForSeconds(1f);
            if (m_gameManager.m_gameState==GameState.Game)
            {
                sec++;
            }
        }
        m_world.ChangeSeason(SeasonState.Winter);
    }

    private IEnumerator FlowControlTimer()
    {
        while (m_world.m_seasonState == SeasonState.Summer)
        {
            yield return new WaitForSeconds(m_flowTime);
            if (m_gameManager.m_gameState == GameState.Game)
            {
                OnFlowControlTimerTick?.Invoke();
                volunteerManager.CallVolunteers();
            }
            //Debug.Log("TICK");
        }
    }

}
