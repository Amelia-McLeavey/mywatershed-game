using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private bool m_showTileInfo;
    [SerializeField] private float m_TileInfoSpeed;
    [SerializeField] private GameObject buttons;

    public bool displayAbiotic = true;

    public bool showButtons=false;

    private float offscreenYPos;
    public float onscreenYPos;
    private float xPos;

    private float targetYPos;



    private void Awake()
    {
        rect = this.GetComponent<RectTransform>();
        xPos = rect.anchoredPosition.x;
    }
    void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        offscreenYPos = rect.rect.height;
        targetYPos = offscreenYPos;
        m_showTileInfo = false;
    }
    
    void Update()
    {
        if(rect.anchoredPosition.y > targetYPos)
        {
            rect.anchoredPosition = new Vector2(xPos, Mathf.Max(rect.anchoredPosition.y - (Time.deltaTime * m_TileInfoSpeed), targetYPos));
        }
        else if (rect.anchoredPosition.y < targetYPos)
        {
            rect.anchoredPosition = new Vector2(xPos, Mathf.Min(rect.anchoredPosition.y + (Time.deltaTime * m_TileInfoSpeed), targetYPos));
        }

        buttons.SetActive(showButtons);
    }

    public void ChangeTile()
    {
        showButtons = false;
        if (!m_showTileInfo)
        {
            targetYPos = onscreenYPos;
            m_showTileInfo = true;
        }
    }

    public void DeselectTile()
    {
        if (m_showTileInfo)
        {
            m_showTileInfo = false;
            offscreenYPos = rect.rect.height;
            targetYPos = offscreenYPos;
        }
    }

    public void ChangeInfo(bool showAbiotic)
    {
        displayAbiotic = showAbiotic;
    }
}
