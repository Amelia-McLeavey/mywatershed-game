using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

/// <summary>
/// Handles creation of a shuffled card deck and ordering of cards to different states of use.
/// </summary>

public class CardDeckHandler : MonoBehaviour
{
    [SerializeField]
    private int m_cardDealAmount = 3;

    private List<int> m_idDeck = new List<int>();
    private List<CardInstance> m_cardsDealt = new List<CardInstance>();
    private List<CardInstance> m_cardsInPlay = new List<CardInstance>();

    public GameObject m_cardUIObjectHolder;

    public GameObject[] m_cardUIObjects;

    private Queue<int> m_shuffledIDDeck = new Queue<int>();

    private CardAsset[] m_cardAssets;

    private World m_world;

    private GameManager m_gameManager;

    [SerializeField]
    private PlayedCardHolder m_playedCardHolder;

    private int cardSelected=0;
    PlayedCardHolder cardHolder;

    [SerializeField] private PlaceableCardUI m_placeableCard;

    private PlayerController playerController;

    private void Awake()
    {
        m_world = FindObjectOfType<World>();
        playerController = FindObjectOfType<PlayerController>();

        // Reset leftover data 
        m_idDeck.Clear();

        // Load the deck of card IDs
        LoadDeck();

        //find the played card holder
        cardHolder = GameObject.Find("PlayedCardHolder").GetComponent<PlayedCardHolder>();
    }

    private void Start()
    {
        //get game manager
        m_gameManager = GameManager.Instance;

        m_shuffledIDDeck.Clear();
        m_cardsDealt.Clear();
        m_cardsInPlay.Clear();

        // Shuffle deck of card IDs once at the beginning of the game
        ShuffleDeck(m_idDeck);

        // Transfer Cards into a Queue for ease of access
        foreach (int card in m_idDeck)
        {
            m_shuffledIDDeck.Enqueue(card);
        }
    }

    private void LoadDeck()
    {
        m_cardAssets = Resources.LoadAll<CardAsset>("Cards");

        foreach (CardAsset currentCardAsset in m_cardAssets)
        {
            int cardID = currentCardAsset.m_id;
            int cardQuantity = currentCardAsset.m_quantity;

            for (int q = 0; q < cardQuantity; q++)
            {
                m_idDeck.Add(cardID);
            }
        }
        // Check the size of the deck
        //Debug.Log($"Deck size = {m_iDDeck.Count}");
    }

    private List<int> ShuffleDeck(List<int> deckIDs)
    {
        for (int i = 0; i < deckIDs.Count; i++)
        {
            int randomIndex = Random.Range(0, deckIDs.Count);
            int temp = deckIDs[i];
            deckIDs[i] = deckIDs[randomIndex];
            deckIDs[randomIndex] = temp;
        }
        return deckIDs;
    }

    private CardInstance CreateCardInstance(int id)
    {
        CardInstance dealtCard = new CardInstance
        {
            cardAssetID = id,
            cardName = m_cardAssets[id].m_name,
            cardDescription = m_cardAssets[id].m_description,
            durationRemaining = m_cardAssets[id].m_effectDuration,
            delayBeforeEffect = m_cardAssets[id].m_timeBeforeEffect,
            tileType = m_cardAssets[id].m_tileTypesAffected,
            numberOfTiles = m_cardAssets[id].m_amountOfTilesTargeted,
            global = (m_cardAssets[id].m_tileModificationScreen==0),
            brownTroutInfluence = m_cardAssets[id].m_brownTroutPopulation,
            creekChubInfluence = m_cardAssets[id].m_creekChubPopulation,
            insectInfluence = m_cardAssets[id].m_insectPopulation,
            redDaceInfluence = m_cardAssets[id].m_redDacePopulation,
            riparianInfluence = m_cardAssets[id].m_riparianQuality,
            riverbedHealthInfluence = m_cardAssets[id].m_riverbedHealth,
            asphaltDensityInfluence = m_cardAssets[id].m_asphaltDensity,
            erosionInfluence = m_cardAssets[id].m_erosionRate,
            landHeightInfluence = m_cardAssets[id].m_landHeight,
            pollutionInfluence = m_cardAssets[id].m_pollutionLevel,
            flowRateInfluence = m_cardAssets[id].m_flowRate,
            sewageInfluence = m_cardAssets[id].m_sewageLevel,
            sinuosityInfluence = m_cardAssets[id].m_sinuosity,
            shadeInfluence = m_cardAssets[id].m_shadeCoverage,
            turbidityInfluence = m_cardAssets[id].m_turbidity,
            waterDepthInfluence = m_cardAssets[id].m_waterDepth,
            waterTempInfluence = m_cardAssets[id].m_waterTemperature
        };

        return dealtCard;
    }

    public void DealCards()
    {
        playerController.DeselectTile();
        for (int i = 0; i < m_cardDealAmount; i++)
        {
            // Dequeue cards from the deck and add to list of cards dealt
            if (m_shuffledIDDeck.Count != 0)
            {
                CardInstance cardDealt = CreateCardInstance(m_shuffledIDDeck.Dequeue());
                m_cardsDealt.Add(cardDealt);

                // Activate UI card 
                m_cardUIObjects[i].SetActive(true);

                // Populate card UI elements with data from Card
                m_cardUIObjects[i].transform.Find("Card Name").GetComponent<TMP_Text>().text = cardDealt.cardName;
                m_cardUIObjects[i].transform.Find("Card Description").GetComponent<TMP_Text>().text = cardDealt.cardDescription;
                m_cardUIObjects[i].transform.Find("Card Duration").GetComponent<TMP_Text>().text = cardDealt.durationRemaining.ToString();

                string cardStats = cardDealt.tileType;

                cardStats += "\n\nTiles affected: " + cardDealt.numberOfTiles;

                cardStats += "\nDuration of Effect: " + cardDealt.durationRemaining;

                if (cardDealt.delayBeforeEffect != 0)
                {
                    cardStats += "\nDelay before Effect: " + cardDealt.delayBeforeEffect;
                }

                m_cardUIObjects[i].transform.Find("Card Stats").GetComponent<TMP_Text>().text = cardStats;
                //m_cardUIObjects[i].transform.Find("Card Duration").GetComponent<TMP_Text>().text = m.cardAssets[cardDealt.cardAssetID].m_effectDuration;
                // etc. all other info passed here
                // Ex:
                //m_cardUIObjects[i].transform.Find("Card Image").GetComponent<Image>().sprite = m_cardAssets[cardDealt.cardAssetID].m_sprite;
                //m_cardUIObjects[i].transform.Find("Cost").GetComponent<Text>().text = m_cardAssets[cardDealt.cardAssetID].m_initialCost.ToString();

                
            }
        }
    }

    public void PlayCard(GameObject chosenCard)
    {
        for (int i = 0; i < m_cardsDealt.Count; i++)
        {
            if (chosenCard.name == m_cardUIObjectHolder.transform.GetChild(i).name)
            {
                m_cardsInPlay.Add(m_cardsDealt[i]);
                cardSelected = i;              
            }
        }

        m_world.ChangeSeason(SeasonState.Summer);
        StartCoroutine(DiscardDealtCards());
        DiscardPlayedCards();
       
    }

    private IEnumerator DiscardDealtCards()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < m_cardDealAmount; i++)
        {
            m_cardUIObjects[i].SetActive(false);
        }

        //if (m_cardsDealt[cardSelected].global)
        //{
        //    m_playedCardHolder.AddNewCard(m_cardsDealt[cardSelected]);
        //}
        //else
        //{
        //    m_placeableCard.SetUpCard(m_cardsDealt[cardSelected]);

        //    m_gameManager.SetGameState(GameState.Frozen, null);
        //}

        m_playedCardHolder.AddNewCard(m_cardsDealt[cardSelected]);

        m_cardsDealt.Clear();


    }

    // TODO: 
    public void DiscardPlayedCards()
    {
        //take card being played and access it's duration variable
        //create an int that stores this number
        //create a reference to that card (we'll need this for it's effects as well no?)
        //every time we PlayCard, we change season. therefore remove 1 from this counter int every time
        //if the duration int is zero, discard that card by destroying the reference.
        int counter;
        foreach (CardInstance cardInPlay in m_cardsInPlay)
        {
            counter = cardInPlay.durationRemaining;
            counter--;
            if (counter <= 0) {
                //discard the card?
                cardHolder.DurationExpired();
            }
        }

        
    }
}
