using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseTimeButton : MonoBehaviour
{
    private GameManager m_gameManager;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
    }
    public void PauseClicked()
    {
        m_gameManager.SetGameState(GameState.Pause, null);
    }

    public void PlayClicked()
    {
        m_gameManager.SetGameState(GameState.Game, null);
    }
}
