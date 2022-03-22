using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayedCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    [SerializeField] private float m_cardSpeed;
    [SerializeField] private float m_hoverOverYPos;

    public CardInstance cardInstance;

    private int maxDelayTime;

    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardStats;
    [SerializeField] private TMP_Text cardDuration;
    [SerializeField] private TMP_Text cardTileType;
    [SerializeField] private Image cardDurationRing;
    [SerializeField] private Image cardDurationCircle;
    [SerializeField] private Color cardDurationGreen;
    [SerializeField] private Image cardIcon;

    private float targetYPos=0f;

    [SerializeField] private float placingYPos = 0f;
    [SerializeField] private float popUpPos = 0f;
    public bool doPopUp = false;
    private CardPlacementOverlay m_overlay;

    public bool placingCurrently = false;
    private PlayerController playerController;
    private GameManager m_gameManager;

    public List<GameObject> tilesAffected = new List<GameObject>();

    [SerializeField] private GameObject m_tileButton;

    private PlayedCardHolder cardHolder;
    private CardDeckHandler cardDeckHandler;
    private World m_world;
    private WorldGenerator m_worldGenerator;

    private Animator anim;
    private string Capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    void Awake()
    {
        m_world = FindObjectOfType<World>();
        m_worldGenerator = FindObjectOfType<WorldGenerator>();
        cardHolder = GameObject.FindObjectOfType<PlayedCardHolder>();
        cardDeckHandler = GameObject.FindObjectOfType<CardDeckHandler>();
        m_overlay = GameObject.FindObjectOfType<CardPlacementOverlay>();
        m_gameManager = GameManager.Instance;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        rect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
    }

    public void SetUpCard(CardInstance card)
    {
        cardInstance = card;
        cardName.text = cardInstance.cardName;
        cardDescription.text = cardInstance.cardDescription;
        cardDuration.text = (cardInstance.durationRemaining + cardInstance.delayBeforeEffect).ToString();
        cardIcon.sprite = cardDeckHandler.icons[cardInstance.iconNumber];
        maxDelayTime = cardInstance.delayBeforeEffect;


        string tileType = "";
        foreach (char letter in cardInstance.tileType)
        {
            if (Capitals.Contains(letter.ToString()) && tileType != "")
            {
                tileType += " ";
            }
            tileType += letter;
        }

        cardTileType.text = tileType;

        string stats = "<b>TILES AFFECTED:</b>\n";
        if (cardInstance.numberOfTiles == 0)
        {
            stats += " All water tiles";
        }
        else
        {
            stats += cardInstance.numberOfTiles + " tiles";
        }

        stats += "\n\n<b>DURATION OF EFFECT: </b>\n" + cardInstance.durationRemaining + " years";

        if (cardInstance.delayBeforeEffect != 0)
        {
            stats += "\n\n<b>DELAY BEFORE EFFECT: </b>\n " + cardInstance.delayBeforeEffect + " years";
        }
        else
        {
            stats += "\n\n<b>NO DELAY</b>";
        }

        cardStats.text = stats;

        if (cardInstance.delayBeforeEffect <= 0)
        {
            ChangeDurationColours();
        }

        if (!cardInstance.global)
        {
            PlacingCard();
        }
        else
        {
            for (int y = 0; y < m_worldGenerator.m_rows; y++)
            {
                for (int x = 0; x < m_worldGenerator.m_columns; x++)
                {
                    if (TileManager.s_TilesDictonary.TryGetValue(new Vector2(x,y), out GameObject value))
                    {
                        if(value.GetComponent<Tile>().m_Basetype == BaseType.Water)
                        {
                            tilesAffected.Add(value);
                        }
                    }
                }
            }
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

        for (int i = 0; i < m_overlay.tilesWithSuccessfulPlacement.Count; i++)
        {
            if (m_overlay.tilesWithSuccessfulPlacement[i].transform.GetComponentInParent<Tile>() !=null)
            {
                tilesAffected.Add(m_overlay.tilesWithSuccessfulPlacement[i].transform.parent.gameObject);
                m_overlay.tilesWithSuccessfulPlacement[i].transform.GetComponentInParent<Tile>().currentCard = this;
                      
            }
        }

        //for (int i = 0; i < cardInstance.numberOfTiles - 1 && i < m_overlay.tileIndexOffsets.Length; i++)
        //{
        //    if (TileManager.s_TilesDictonary.TryGetValue(m_overlay.GetAdditionalTileIndex(playerController.variableHolder.GetComponent<Tile>().m_TileIndex, i), out GameObject value))
        //    {
        //        tilesAffected.Add(value);
        //        value.GetComponent<Tile>().currentCard = this;

        //    }
        //}

        m_world.m_endResultManager.cardInstances.Add(cardInstance);
        m_world.ChangeSeason(SeasonState.Summer);
        m_overlay.placing=false;

        m_tileButton.SetActive(!cardInstance.global);
    }

    public IEnumerator NewYear()
    {
        doPopUp = true;
        yield return new WaitForSeconds(0.5f);
        UpdateTileValues();
        if (cardInstance.delayBeforeEffect > 0)
        {
            cardInstance.delayBeforeEffect--;
            cardDurationRing.fillAmount = (float)cardInstance.delayBeforeEffect / (float)maxDelayTime;

            if (cardInstance.delayBeforeEffect <= 0)
            {
                ChangeDurationColours();
            }
            yield return new WaitForSeconds(0.3f);
            cardDuration.text = (cardInstance.durationRemaining + cardInstance.delayBeforeEffect).ToString();  
        }
        else
        {       
            cardInstance.durationRemaining--;
            if (cardInstance.durationRemaining <= 0)
            {
                //Destroy card
                anim.SetTrigger("Destroy");
                yield return new WaitForSeconds(0.5f);
                //remove references from all tiles
                foreach (GameObject tileObject in tilesAffected)
                {
                    foreach (Transform child in tileObject.transform)
                    {
                        if (child.CompareTag("CardOverlay"))
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    tileObject.GetComponent<Tile>().currentCard = null;
                }
                Disappear();
            }
            else
            {
                anim.SetTrigger("Shake");
                yield return new WaitForSeconds(0.3f);
                cardDuration.text = cardInstance.durationRemaining.ToString();
            }
        }
        
        doPopUp = false;
    }

    private void ChangeDurationColours()
    {
        cardDuration.color = Color.white;
        cardDurationCircle.color = cardDurationGreen;
        cardDurationRing.enabled = false;
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
                    tileObject.GetComponent<BrownTroutPopulation>().value *= 1+(cardInstance.brownTroutInfluence/100);
                }
            }

            if(cardInstance.creekChubInfluence != 0)
            {
                if (tileObject.GetComponent<CreekChubPopulation>() != null)
                {
                    tileObject.GetComponent<CreekChubPopulation>().value *= 1 + (cardInstance.creekChubInfluence/100);
                }
            }

            if(cardInstance.insectInfluence != 0)
            {
                if (tileObject.GetComponent<InsectPopulation>() != null)
                {
                    tileObject.GetComponent<InsectPopulation>().value *= 1 + (cardInstance.insectInfluence/100);
                }
            }

            if(cardInstance.redDaceInfluence != 0)
            {
                if (tileObject.GetComponent<RedDacePopulation>() != null)
                {
                    tileObject.GetComponent<RedDacePopulation>().value *= 1 + (cardInstance.redDaceInfluence/100);
                }
            }

            if(cardInstance.riparianInfluence != 0)
            {
                if (tileObject.GetComponent<RiparianLevel>() != null)
                {
                    tileObject.GetComponent<RiparianLevel>().value *= 1 + (cardInstance.riparianInfluence/100);
                }
            }

            if(cardInstance.riverbedHealthInfluence != 0)
            {
                if (tileObject.GetComponent<RiverbedHealth>() != null)
                {
                    tileObject.GetComponent<RiverbedHealth>().value *= 1 + (cardInstance.riverbedHealthInfluence/100);
                }
            }

            if(cardInstance.asphaltDensityInfluence != 0)
            {
                if (tileObject.GetComponent<AsphaltDensity>() != null)
                {
                    tileObject.GetComponent<AsphaltDensity>().value *= 1 + (cardInstance.asphaltDensityInfluence/100);
                }
            }

            if(cardInstance.erosionInfluence != 0)
            {
                if (tileObject.GetComponent<ErosionRate>() != null)
                {
                    tileObject.GetComponent<ErosionRate>().value *= 1 + (cardInstance.erosionInfluence/100);
                }
            }

            if(cardInstance.landHeightInfluence != 0)
            {
                if (tileObject.GetComponent<LandHeight>() != null)
                {
                    tileObject.GetComponent<LandHeight>().value *= 1 + (cardInstance.landHeightInfluence/100);
                }
            }

            if(cardInstance.pollutionInfluence != 0)
            {
                if (tileObject.GetComponent<PollutionLevel>() != null)
                {
                    tileObject.GetComponent<PollutionLevel>().value *= 1 + (cardInstance.pollutionInfluence/100);
                }
            }

            if(cardInstance.flowRateInfluence != 0)
            {
                if (tileObject.GetComponent<RateOfFlow>() != null)
                {
                    tileObject.GetComponent<RateOfFlow>().value *= 1 + (cardInstance.flowRateInfluence/100);
                }
            }

            if(cardInstance.sewageInfluence != 0)
            {
                if (tileObject.GetComponent<SewageLevel>() != null)
                {
                    tileObject.GetComponent<SewageLevel>().value *= 1 + (cardInstance.sewageInfluence/100);
                }
            }

            if(cardInstance.sinuosityInfluence != 0)
            {
                if (tileObject.GetComponent<Sinuosity>() != null)
                {
                    tileObject.GetComponent<Sinuosity>().value *= 1 + (cardInstance.sinuosityInfluence/100);
                }
            }

            if(cardInstance.shadeInfluence != 0)
            {
                if (tileObject.GetComponent<ShadeCoverage>() != null)
                {
                    tileObject.GetComponent<ShadeCoverage>().value *= 1 + (cardInstance.shadeInfluence/100);
                }
            }

            if(cardInstance.turbidityInfluence != 0)
            {
                if (tileObject.GetComponent<Turbidity>() != null)
                {
                    tileObject.GetComponent<Turbidity>().value *= 1 + (cardInstance.turbidityInfluence/100);
                }
            }

            if(cardInstance.waterDepthInfluence != 0)
            {
                if (tileObject.GetComponent<WaterDepth>() != null)
                {
                    tileObject.GetComponent<WaterDepth>().value *= 1 + (cardInstance.waterDepthInfluence/100);
                }
            }

            if(cardInstance.waterTempInfluence != 0)
            {
                if (tileObject.GetComponent<WaterTemperature>() != null)
                {
                    tileObject.GetComponent<WaterTemperature>().value *= 1 + (cardInstance.waterTempInfluence/100);
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
