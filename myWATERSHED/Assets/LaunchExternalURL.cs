using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchExternalURL : MonoBehaviour
{
    public string externalURL;

    // Update is called once per frame

    public void LaunchURL() {
        Application.OpenURL(externalURL);
    }
}
