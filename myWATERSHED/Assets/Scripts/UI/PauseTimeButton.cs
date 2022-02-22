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
        if(m_gameManager.m_gameState == GameState.Frozen)
        {
            m_gameManager.SetGameState(GameState.Game, null);
        }
        else
        {
            m_gameManager.SetGameState(GameState.Frozen, null);
        }
    }

}
