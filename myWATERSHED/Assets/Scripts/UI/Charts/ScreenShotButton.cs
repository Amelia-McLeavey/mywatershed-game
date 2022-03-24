using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotButton : MonoBehaviour
{
    public Animator fishPopUp;
    public void Clicked()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            fishPopUp.ResetTrigger("Exit");

            fishPopUp.SetTrigger("Enter");
        }
        else
        {
            ScreenCapture.CaptureScreenshot("Screenshot.png");
        }
    }
}
