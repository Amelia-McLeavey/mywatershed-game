using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotButton : MonoBehaviour
{
    public Animator fishPopUp;
    public void Clicked()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            fishPopUp.SetTrigger("Enter");
        }
        else
        {
            Debug.Log(System.DateTime.Now);
            ScreenCapture.CaptureScreenshot("Screenshot.png");
        }
    }
}
