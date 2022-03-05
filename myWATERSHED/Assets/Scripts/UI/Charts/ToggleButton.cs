using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    public GameObject toggleImage;

    public void Clicked()
    {
        toggleImage.SetActive(!toggleImage.activeSelf);
    }
}
