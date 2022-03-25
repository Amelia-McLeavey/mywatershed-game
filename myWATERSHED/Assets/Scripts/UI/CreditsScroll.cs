using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
    public Scrollbar scroll;
    public float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        ResetCredits();
    }

    public void ResetCredits()
    {
        scroll.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        scroll.value = Mathf.Max(scroll.value - scrollSpeed * Time.deltaTime, 0f);
    }
}
