using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This scrip handles the effects of played cards

public class CardActor : MonoBehaviour
{
    private CardDeckHandler m_cardDeckHandler;

    private void Start()
    {
        m_cardDeckHandler = GetComponent<CardDeckHandler>();
    }

    public void CardAction()
    {
        //handle passing the 'action' from the card to the relevant tiles
        //ie. "Lower temperature of water card" might then:
        // for every Tile game object tagged Water, affect the following variables
        // temperature -= card.variable.temperature; etc. etc.
        
    }
}
