using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum SeasonState { Summer, Winter }
public enum RedDacePopulationState { High, Average, Low, Extinct }

/// <summary>
/// A script to track the state of the world.
/// </summary>

public class World : MonoBehaviour
{
    public delegate void OnSeasonChangeHandler(SeasonState season);
    public static event OnSeasonChangeHandler OnSeasonChange;

    [SerializeField]
    public float m_summerLengthInSeconds = 30f;

    [SerializeField]
    private int m_currentYear = 1;

    [SerializeField]
    private Text m_currentYearText;
    [SerializeField]
    private Text m_redDacePopulationText;

    [SerializeField]
    private DaceHealthUI m_daceHealthScript;

    [SerializeField]
    private GameObject m_failStateObject;

    private int m_redDaceTotalPopulation;

    private GameManager m_gameManager;
    private WorldGenerator m_worldGenerator;
    private CardDeckHandler m_cardDeckHandler;

    public SeasonState m_seasonState { get; private set; }

    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_worldGenerator = FindObjectOfType<WorldGenerator>();
        m_cardDeckHandler = FindObjectOfType<CardDeckHandler>();
        m_seasonState = SeasonState.Summer;
        ChangeSeason(m_seasonState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_gameManager.SetGameState(GameState.MainMenu, "PrototypeMenuScene");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            m_gameManager.SetGameState(GameState.Pause, null);
        }
    }

    public void OnClickReturnToMenu()
    {
        m_gameManager.SetGameState(GameState.MainMenu, "PrototypeMenuScene");
    }

    public void ChangeSeason(SeasonState season)
    {
        // Change to Summer
        if (m_seasonState == SeasonState.Winter && season == SeasonState.Summer)
        {
            m_currentYear++;
            SetYear();
        }
        // Change to winter
        else if (m_seasonState == SeasonState.Summer && season == SeasonState.Winter)
        {
            UpdateTotalDacePopulation();
            m_cardDeckHandler.DealCards();
        }

        // Change the season state and callback the event
        m_seasonState = season;
        OnSeasonChange?.Invoke(season);
    }

    private void SetYear()
    {
        m_currentYearText.text = m_currentYear.ToString();
    }

    private void UpdateTotalDacePopulation()
    {
        List<int> dace = new List<int>();

        for (int x = 0; x < m_worldGenerator.m_rows; x++)
        {
            for (int y = 0; y < m_worldGenerator.m_columns; y++)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == BaseType.Water)
                    {
                        dace.Add(value.GetComponent<RedDacePopulation>().m_RedDacePopulation);
                    }
                }
            }
        }

        m_redDaceTotalPopulation = dace.Sum();
        m_daceHealthScript.SetHealth(m_redDaceTotalPopulation);
        m_redDacePopulationText.text = m_redDaceTotalPopulation.ToString();

        if (m_redDaceTotalPopulation <= 0)
        {
            m_failStateObject.SetActive(true);
        }
    }

}
