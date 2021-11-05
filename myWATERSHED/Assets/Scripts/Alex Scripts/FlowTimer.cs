using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlowTimer : MonoBehaviour
{
    public float m_flowTime;

    private static FlowTimer s_instance;

    private void Awake()
    {
        // Check if more than one instance of FlowTimer has been created, stop the program if so.
        Debug.Assert(s_instance == null, "There is more than one FlowTimer instance.");

        // Store the static variable
        s_instance = this;
    }

    // TODO: We may need to change when the Coroutine is started and stopped.
    void Start()
    {
        StartCoroutine("Timer");
    }

    // Create a way to access the static variable
    public static FlowTimer Instance()
    {
        // Check if no instances of FlowTimer have been created, stop the program if so.
        Debug.Assert(s_instance != null, "FlowTimer component must exist on an object in the scene.");

        return s_instance;
    }

    //the main timer
    public event Action OnTimerTick;

    private IEnumerator Timer()
    {
        while (true)
        {
            Debug.Assert(m_flowTime > 0, "flowTime must be greater than 0.");
            yield return new WaitForSeconds(m_flowTime);
            OnTimerTick?.Invoke();
        }
    }
}
