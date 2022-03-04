using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayedCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    [SerializeField] private float m_cardSpeed;
    [SerializeField] private float m_hoverOverYPos;

    public CardInstance cardInstance;

    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardDuration;

    private float targetYPos=0f;

    [SerializeField] private float placingYPos = 0f;
    [SerializeField] private float popUpPos = 0f;
    public bool doPopUp = false;
    private CardPlacementOverlay m_overlay;

    private bool placingCurrently = false;
    private PlayerController playerController;
    private GameManager m_gameManager;

    public List<GameObject> tilesAffected = new List<GameObject>();

    [SerializeField] private GameObject m_tileButton;


    private PlayedCardHolder cardHolder;
    private World m_world;
    void Awake()
    {
        m_world = FindObjectOfType<World>();
        cardHolder = GameObject.FindObjectOfType<PlayedCardHolder>();
        m_overlay = GameObject.FindObjectOfType<CardPlacementOverlay>();
        m_gameManager = GameManager.Instance;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        rect = GetComponent<RectTransform>();

        Debug.Log(m_overlay.name);
    }

    public void SetUpCard(CardInstance card)
    {
        cardInstance = card;
        cardName.text = cardInstance.cardName;
        cardDescription.text = cardInstance.cardDescription;
        cardDuration.text = cardInstance.durationRemaining.ToString();
        if (!cardInstance.global)
        {
            PlacingCard();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (placingCurrently)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(rect.anchoredPosition.y, placingYPos, Time.deltaTime * m_cardSpeed));
        }
        else if (doPopUp)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(rect.anchoredPosition.y, popUpPos, Time.deltaTime * m_cardSpeed));
        }
        else
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(rect.anchoredPosition.y, targetYPos, Time.deltaTime * m_cardSpeed));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetYPos = m_hoverOverYPos;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        targetYPos = 0f;
    }


    private void PlacingCard()
    {
        placingCurrently = true;
        m_overlay.placing=true;
        m_gameManager.SetGameState(GameState.Placing, null);
    }

    public void CardPlaced()
    {
        //playerController.variableHolder.GetComponent<Tile>().currentCard = cardInstance;
        m_gameManager.SetGameState(GameState.Game, null);
        placingCurrently = false;
        tilesAffected.Clear();

        tilesAffected.Add(playerController.variableHolder);
        playerController.variableHolder.GetComponent<Tile>().currentCard = this;

        for (int i = 0; i < cardInstance.numberOfTiles - 1; i++)
        {
            if (TileManager.s_TilesDictonary.TryGetValue(m_overlay.GetAdditionalTileIndex(playerController.variableHolder.GetComponent<Tile>().m_TileIndex, i), out GameObject value))
            {
                tilesAffected.Add(value);
                value.GetComponent<Tile>().currentCard = this;
                Debug.Log(value.GetComponent<Tile>().currentCard);
                Debug.Log(this);
            }
        }

        //foreach(GameObject tileObject in tilesAffected)
        //{
        //    tileObject.GetComponent<Tile>().currentCard = this;
        //}

        m_world.ChangeSeason(SeasonState.Summer);
        m_overlay.placing=false;

        m_tileButton.SetActive(!cardInstance.global);
    }

    public IEnumerator NewYear()
    {
        doPopUp = true;
        yield return new WaitForSeconds(0.6f);
        UpdateTileValues();
        cardInstance.durationRemaining--;
        if (cardInstance.durationRemaining <= 0)
        {
            //Destroy card

            //remove references from all tiles
            foreach (GameObject tileObject in tilesAffected)
            {
                tileObject.GetComponent<Tile>().currentCard = null;
            }
            Disappear();
        }
        else
        {
            cardDuration.text = cardInstance.durationRemaining.ToString();
        }
        yield return new WaitForSeconds(0.3f);
        doPopUp = false;
    }

    public void Disappear()
    {
        cardHolder.extinctCards.Add(rect);
        this.gameObject.SetActive(false);
    }
    private void UpdateTileValues()
    {
        foreach (GameObject tileObject in tilesAffected)
        {
            //tileObject.GetComponent<Tile>().currentCard = null;

            if(cardInstance.brownTroutInfluence != 0)
            {
                if (tileObject.GetComponent<BrownTroutPopulation>() != null)
                {
                    tileObject.GetComponent<BrownTroutPopulation>().value += cardInstance.brownTroutInfluence;
                }
            }

            if(cardInstance.creekChubInfluence != 0)
            {
                if (tileObject.GetComponent<CreekChubPopulation>() != null)
                {
                    tileObject.GetComponent<CreekChubPopulation>().value += cardInstance.creekChubInfluence;
                }
            }

            if(cardInstance.insectInfluence != 0)
            {
                if (tileObject.GetComponent<InsectPopulation>() != null)
                {
                    tileObject.GetComponent<InsectPopulation>().value += cardInstance.insectInfluence;
                }
            }

            if(cardInstance.redDaceInfluence != 0)
            {
                if (tileObject.GetComponent<RedDacePopulation>() != null)
                {
                    tileObject.GetComponent<RedDacePopulation>().value += cardInstance.redDaceInfluence;
                }
            }

            if(cardInstance.riparianInfluence != 0)
            {
                if (tileObject.GetComponent<RiparianLevel>() != null)
                {
                    tileObject.GetComponent<RiparianLevel>().value += cardInstance.riparianInfluence;
                }
            }

            if(cardInstance.riverbedHealthInfluence != 0)
            {
                if (tileObject.GetComponent<RiverbedHealth>() != null)
                {
                    tileObject.GetComponent<RiverbedHealth>().value += cardInstance.riverbedHealthInfluence;
                }
            }

            if(cardInstance.asphaltDensityInfluence != 0)
            {
                if (tileObject.GetComponent<AsphaltDensity>() != null)
                {
                    tileObject.GetComponent<AsphaltDensity>().value += cardInstance.asphaltDensityInfluence;
                }
            }

            if(cardInstance.erosionInfluence != 0)
            {
                if (tileObject.GetComponent<ErosionRate>() != null)
                {
                    tileObject.GetComponent<ErosionRate>().value += cardInstance.erosionInfluence;
                }
            }

            if(cardInstance.landHeightInfluence != 0)
            {
                if (tileObject.GetComponent<LandHeight>() != null)
                {
                    tileObject.GetComponent<LandHeight>().value += cardInstance.landHeightInfluence;
                }
            }

            if(cardInstance.pollutionInfluence != 0)
            {
                if (tileObject.GetComponent<PollutionLevel>() != null)
                {
                    tileObject.GetComponent<PollutionLevel>().value += cardInstance.pollutionInfluence;
                }
            }

            if(cardInstance.flowRateInfluence != 0)
            {
                if (tileObject.GetComponent<RateOfFlow>() != null)
                {
                    tileObject.GetComponent<RateOfFlow>().value += cardInstance.flowRateInfluence;
                }
            }

            if(cardInstance.sewageInfluence != 0)
            {
                if (tileObject.GetComponent<SewageLevel>() != null)
                {
                    tileObject.GetComponent<SewageLevel>().value += cardInstance.sewageInfluence;
                }
            }

            if(cardInstance.sinuosityInfluence != 0)
            {
                if (tileObject.GetComponent<Sinuosity>() != null)
                {
                    tileObject.GetComponent<Sinuosity>().value += cardInstance.sinuosityInfluence;
                }
            }

            if(cardInstance.shadeInfluence != 0)
            {
                if (tileObject.GetComponent<ShadeCoverage>() != null)
                {
                    tileObject.GetComponent<ShadeCoverage>().value += cardInstance.shadeInfluence;
                }
            }

            if(cardInstance.turbidityInfluence != 0)
            {
                if (tileObject.GetComponent<Turbidity>() != null)
                {
                    tileObject.GetComponent<Turbidity>().value += cardInstance.turbidityInfluence;
                }
            }

            if(cardInstance.waterDepthInfluence != 0)
            {
                if (tileObject.GetComponent<WaterDepth>() != null)
                {
                    tileObject.GetComponent<WaterDepth>().value += cardInstance.waterDepthInfluence;
                }
            }

            if(cardInstance.waterTempInfluence != 0)
            {
                if (tileObject.GetComponent<WaterTemperature>() != null)
                {
                    tileObject.GetComponent<WaterTemperature>().value += cardInstance.waterTempInfluence;
                }
            }
        }
    }


    public void GoToTile()
    {
        if (tilesAffected.Count >= 1)
        {
            playerController.SetCamPos(tilesAffected[0].transform.position);
            playerController.SelectTile(tilesAffected[0]);
        }
    }
}
