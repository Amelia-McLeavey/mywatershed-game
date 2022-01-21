using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script to handle the effect of played cards.
/// </summary>

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

    private void UpdateDurations() 
    { 
        // Check if any card have expired and if so dicard them
    }

    private void CardsInPlayUpdater()
    {
        // Apply the card actions
    }
}
