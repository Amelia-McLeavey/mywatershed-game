using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseTimeButton : MonoBehaviour
{
    //this can be replaced with images instead of text
    [SerializeField] private TMP_Text m_butttonText;
    private GameManager m_gameManager;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
    }
    public void clicked()
    {
        m_butttonText.text = "Play Time";
        m_gameManager.SetGameState(GameState.Pause, null);
    }
}
