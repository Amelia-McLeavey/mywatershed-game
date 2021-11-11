using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public Card[] deck;
    int cardIndex;
    public Card cardsInHand;

    //ui stuff to pass from card script to UI element
    public Text nameText;
    public Text descriptionText;
    //public Image artwork;
    //public Text cost;
    //etc...

    //placeholder stuff for card, will need to have this automate later
    public GameObject cardUI;

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
        cardIndex = Random.Range(0, deck.Length);
        cardsInHand = deck[cardIndex];

        ////randomly select X number of cards from the Deck array
        //for (int i = 0; i < cardsInHand.Length; i++)
        //{
        //    cardIndex = Random.Range(0, deck.Length);
        //    cardsInHand[deck[cardIndex]];
        //}



        //spawn UI cards and populate them with the required info passed from the card.
        cardUI.SetActive(true); //for now, this will need to be an Instantiate automation in actual game
        nameText.text = cardsInHand.name;
        descriptionText.text = cardsInHand.description;
        //etc. all other info passed here
                       
    }

    public void CardAction() {
        //handle passing the 'action' from the card to the relevant tiles
        //ie. "Lower temperature of water card" might then:
        // for every Tile game object tagged Water, affect the following variables
        // temperature -= card.variable.temperature; etc. etc.
        Debug.Log("USE CARD");

    }
}
