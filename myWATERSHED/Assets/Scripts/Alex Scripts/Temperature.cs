using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temperature : MonoBehaviour
{
    public float tempLevel = 0;
    public float tempIncrement = 10;
    public Text tempText;
    //i forget why id is necessary...hard coding it to match for purpose of POC
    //investigate later (actually though, this will come up again)
    public int id = 1;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.timerEvent += AddTemperature;
        Debug.Log("temp component added and initialized");
        //this will DEFINITELY be terrible with a million things, but using for testing
        tempText = GameObject.Find("Stat_Temperature").GetComponent<Text>();
    }

    private void AddTemperature(int id) {
        if (id == this.id)
        {
            tempLevel += tempIncrement;
            Debug.Log("increasing temperature");
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        //update UI
        tempText.text = tempLevel.ToString("0");
        //if temp reaches certain point, lose game etc
        if (tempLevel >= 100) {
            GetComponent<Tiles>().GameOver();
        }
    }

    /*
     let's say there are methods within each variable that handle how they can
     be affected by the work of Volunteers...but since there may be many to manage,
     the Tile script will act as a middleman to handle these, but the methods will
     live inside the variables that use them? this might be sleepy brain thought
     anyway let's try...
         */
    public void ReduceTemperature(float tempReduce) {
        tempLevel -= tempReduce;
    }

    private void OnDestroy()
    {
        EventManager.current.timerEvent -= AddTemperature;
        
    }
}
