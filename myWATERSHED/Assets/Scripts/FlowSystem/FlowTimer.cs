using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class FlowTimer : MonoBehaviour
{
    public float m_flowTime;
    public float m_summerLength; //the amount of time to elapse in seconds before switching to winter
    public bool isSummer; //keep it simple? a bool to track the season. if it's not summer (false) it's obv winter. shrug!
    [SerializeField]
    CardDeckHandler m_cardDeckHandler;

    private static FlowTimer s_instance;
    public Text yearText;
    public int currentYear = 1;

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
        isSummer = true;
        yearText.text = currentYear.ToString();
        StartCoroutine("Timer");
        Invoke("SeasonChange", m_summerLength);
    }



    // Create a way to access the static variable
    public static FlowTimer Instance()
    {
        // Check if no instances of FlowTimer have been created, stop the program if so.
        Debug.Assert(s_instance != null, "FlowTimer component must exist on an object in the scene.");

        return s_instance;
    }

    // The timer event
    public event Action OnTimerTick;

    private IEnumerator Timer()
    {
        while (isSummer)
        {
            // Check that the flowTime variable is greater than 0, stop the program if so.
            Debug.Assert(m_flowTime > 0, "flowTime must be greater than 0.");
            yield return new WaitForSeconds(m_flowTime);
            OnTimerTick?.Invoke();
        }
        yield return null; //p sure this has to be here else it will divide by zero and crash
    }

    public void SeasonChange() 
    {
        //Debug.Log("Change to Winter Mode");

        isSummer = false; //it's winter
        m_cardDeckHandler.DealCards();
        /*
         
        pseudo for cards, prob want this to live in another script

        - shuffle card deck and show player however many cards they get per winter turn
        - spawn X number of cards from scriptable object
        - assign values to card obj from parser info
        
         */
        

    }

    //can def work this into the seasonchange method, just getting something quick for testing
    public void NextSeason() {
        currentYear++;
        yearText.text = currentYear.ToString();
        isSummer = true;
        Invoke("SeasonChange", m_summerLength);
    }
}
