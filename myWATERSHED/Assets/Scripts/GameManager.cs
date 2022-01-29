using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, Game, Pause }

/// <summary>
/// This script exists to track the different game state and provide a way to change between states.
/// </summary>

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance = null;

    public GameState m_gameState { get; private set; }

    private void Awake()
    {
        if (s_instance == null)
        {
            // Store the static variable
            s_instance = this;
            DontDestroyOnLoad(s_instance);
        }
        else
        {
            Debug.LogWarning("There is already an instance of GameManager");
            Destroy(gameObject);
        }
        m_gameState = GameState.Game;
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
        m_gameState = state;

        if (sceneName != null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnApplicationQuit()
    {
        s_instance = null;
    }

}
