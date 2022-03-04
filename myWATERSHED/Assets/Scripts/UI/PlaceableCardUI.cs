using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaceableCardUI : MonoBehaviour
{
    public CardInstance cardInstance;

    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    private GameManager m_gameManager;

    private PlayerController playerController;


    [SerializeField] private Vector2 onScreenPos;
    [SerializeField] private Vector2 offScreenPos;

    [SerializeField] private GameObject m_overlay;

    private CardPlacementOverlay cpo;
    private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        rect = this.GetComponent<RectTransform>();
        m_gameManager = GameManager.Instance;
        cpo = m_overlay.GetComponent<CardPlacementOverlay>();
    }

    public void SetUpCard(CardInstance card)
    {
        cardInstance = card;
        cardName.text = cardInstance.cardName;
        cardDescription.text = cardInstance.cardDescription;
        rect.anchoredPosition = onScreenPos;
        m_overlay.SetActive(true);
    }

    public void cardPlaced()
    {
        //playerController.variableHolder.GetComponent<Tile>().currentCard = cardInstance;
        m_gameManager.SetGameState(GameState.Game, null);
        rect.anchoredPosition = offScreenPos;
        m_overlay.SetActive(false);
    }
}
