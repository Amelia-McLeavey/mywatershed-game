using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURLButton : MonoBehaviour
{
    public string link;

    public void Clicked()
    {
        Application.OpenURL(link);
    }
}
