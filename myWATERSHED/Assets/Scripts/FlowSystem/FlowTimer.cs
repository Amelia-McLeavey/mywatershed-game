using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class FlowTimer : MonoBehaviour
{
    public Text yearText;
    public int currentYear = 1;
    public float m_flowTime;
    public float m_summerLength; //the amount of time to elapse in seconds before switching to winter
    public bool isSummer; //keep it simple? a bool to track the season. if it's not summer (false) it's obv winter. shrug!
    [SerializeField]
    CardDeckHandler m_cardDeckHandler;

    // TODO: We may need to change when the Coroutine is started and stopped.
    void Start()
    {
        isSummer = true;
        yearText.text = currentYear.ToString();
        StartCoroutine("Timer");
        Invoke("SeasonChange", m_summerLength);
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
        /*

     probably put the check for red side dace population in here?

      where are we getting this total red side dace population from?

      if(total redside dace population <=0){
      failStateMessage.SetActive(true);
      } else...
       */

        isSummer = false; //it's winter
        m_cardDeckHandler.DealCards();
    }

    //can def work this into the seasonchange method, just getting something quick for testing
    public void NextSeason() {
        currentYear++;
        yearText.text = currentYear.ToString();
        isSummer = true;
        Invoke("SeasonChange", m_summerLength);
    }
}
