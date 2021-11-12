using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDeckHandler : MonoBehaviour
{
    [SerializeField]
    private int m_cardDealAmount = 3;

    public Card[] m_deck;
    public List<Card> m_cardsDealt;
    public List<Card> m_cardsInPlay;

    public GameObject m_cardUIObjectHolder;

    public GameObject[] m_cardUIObjects;

    ////ui stuff to pass from card script to UI element
    //public Text nameText;
    //public Text descriptionText;
    ////public Image artwork;
    ////public Text cost;
    ////etc...

    private Queue<Card> m_orderedDeck = new Queue<Card>();

    private void Start()
    {
        // Reset any leftover data 
        m_orderedDeck.Clear();
        m_cardsDealt.Clear();
        m_cardsInPlay.Clear();

        // Load in the Card data to an array
        m_deck = Resources.LoadAll<Card>("Cards");

        // Shuffle Cards once at the beginning of the game
        ShuffleDeck(m_deck);

        // Transfer Cards into a Queue for ease of access
        foreach (Card card in m_deck)
        {
            m_orderedDeck.Enqueue(card);
        }
    }

    public void DealCards()
    {
        for (int i = 0; i < m_cardDealAmount; i++)
        {
            // Dequeue cards from the deck and add to list of cards dealt
            if (m_orderedDeck.Count != 0)
            {
                m_cardsDealt.Add(m_orderedDeck.Dequeue());
            }

            // Activate UI card 
            m_cardUIObjects[i].SetActive(true);

            // Populate card UI elements with data from Card
            m_cardUIObjects[i].transform.Find("Card Name").GetComponent<Text>().text = m_cardsDealt[i].name;
            m_cardUIObjects[i].transform.Find("Card Description").GetComponent<Text>().text = m_cardsDealt[i].description;
            // etc. all other info passed here
            // Ex:
            //m_cardUIObjects[i].transform.Find("Card Image").GetComponent<Image>().sprite = m_cardsDealt[i].image;
            //m_cardUIObjects[i].transform.Find("Cost").GetComponent<Text>().text = m_cardsDealt[i].cost;
        }
    }

    public void PlayCard(GameObject chosenCard)
    {
        for (int i = 0; i < m_cardsDealt.Count; i++)
        {
            if (chosenCard.name == m_cardUIObjectHolder.transform.GetChild(i).name)
            {
                m_cardsInPlay.Add(m_cardsDealt[i]);
            }
        }
        DiscardDealtCards();
    }

    // TODO: 
    public void DiscardPlayedCards()
    {

    }

    private void DiscardDealtCards()
    {
        for (int i = 0; i < m_cardDealAmount; i++)
        {
            m_cardUIObjects[i].SetActive(false);
        }

        m_cardsDealt.Clear();
    }

    
    private Card[] ShuffleDeck(Card[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            int randomIndex = Random.Range(0, deck.Length);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        return deck;
    } 
}
