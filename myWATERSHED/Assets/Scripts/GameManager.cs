using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, Game, Pause, Frozen, Placing}

/// <summary>
/// This script exists to track the different game state and provide a way to change between states.
/// </summary>
/// 

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance = null;
    private PlayedCardHolder cardHolder;

    private GameObject fishCam;

    public GameState m_gameState { get; private set; }

    private World world;

    private void Awake()
    {
        if (s_instance == null)
        {
            // Store the static variable
            s_instance = this;
            DontDestroyOnLoad(s_instance);

            // Force state transition code to run because sometimes we playtest game without first running MainMenu scene
            m_gameState = GameState.MainMenu;
            SetGameState(GameState.Game, null);
        }
        else
        {
            Debug.LogWarning("There is already an instance of GameManager");
            Destroy(gameObject);
        }      
    }

    /// <summary>
    /// Provides access to the static variable instance of GameManager.
    /// </summary>
    /// <returns></returns>
    public static GameManager Instance
    {
        get
        {
            // Check if no instances of GameManager have been created, stop the program if so.
            Debug.Assert(s_instance != null, "GameManager component must exist on an object in the scene.");

            return s_instance;
        }
    }

    public void SetGameState(GameState state, string sceneName)
    {
        // Initialize references so that they are not lost in state transition after going from Game to MainMenu to Game again

        if (world == null)
        {
            world = GameObject.FindObjectOfType<World>();
        }

        if(m_gameState == GameState.MainMenu && state == GameState.Game)
        {
            OnState_MainMenuToGame();
        }

        if(state == GameState.Game && world.m_seasonState == SeasonState.Summer)
        {
            fishCam.SetActive(true);
        }
        else
        {
            fishCam.SetActive(false);
        }

        if(m_gameState == GameState.Placing && state == GameState.Game)
        {
            // UPDATE TILES 
            cardHolder.StartCoroutine("UpdateValues");
        }
        m_gameState = state;

        if (sceneName != null)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }

    private void OnState_MainMenuToGame()
    {
        fishCam = GameObject.Find("Fishcam-Camera");
        cardHolder = GameObject.FindObjectOfType<PlayedCardHolder>();
    }

    public void OnApplicationQuit()
    {
        s_instance = null;
    }

}
