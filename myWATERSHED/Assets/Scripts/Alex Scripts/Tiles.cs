using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tiles : MonoBehaviour
{
    public Text tileText;
    string tileType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tileText.text = tileType;
    }

    //for testing, we can assign tile type from UI buttons
    //this will be handled by the spawner
    //just trying to prove it can be assigned by external input

    public void AssignWater() {
        tileType = "Water";
        gameObject.AddComponent<Temperature>();
    }

    public void VolunteerAction() {
        /*
         idk if this is dumb...but all the possible ways a volunteer can
         interact witha  tile and the potential variables that are attached
         can be listed here waiting to be utilized? sleepybrain code but let's try...
         */

        if (tileType == "Water") {
            GetComponent<Temperature>().ReduceTemperature(7);
        }

    }

    //this will need to change from 2D in our game but just using for testing purposes
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Meeple")
        {
            Debug.Log("Volunteer placed");

        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Meeple")
        {
            Debug.Log("Volunteer removed");
        }

    }

    public void GameOver() {
        Text gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        gameOverText.enabled = true;
    }
}
