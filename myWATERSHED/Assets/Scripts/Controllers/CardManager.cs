using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Card[] deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = Resources.LoadAll<Card>("Cards");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleDeal() {
        Debug.Log("shuffle and deal cards");

    }
}
