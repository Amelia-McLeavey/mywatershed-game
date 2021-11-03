using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTimer : MonoBehaviour
{
    public int id;
    public float flowTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("timer");
    }

    IEnumerator timer() {
        while (true) {
            EventManager.current.timerTrigger(id);
            yield return new WaitForSeconds(flowTime);
        }
    }
}
