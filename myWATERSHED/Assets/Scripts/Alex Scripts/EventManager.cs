using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        //set this as a static script so it can be accessed by all scripts
        current = this;
    }

    //the main timer
    public event Action<int> timerEvent;
    public void timerTrigger(int id) {
        if (timerEvent != null) {
            timerEvent(id);
        }
    }


}
