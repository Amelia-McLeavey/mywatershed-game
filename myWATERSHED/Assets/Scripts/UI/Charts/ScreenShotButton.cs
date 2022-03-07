using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png");
    }
}
