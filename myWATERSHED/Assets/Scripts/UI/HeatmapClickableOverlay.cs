using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapClickableOverlay : MonoBehaviour
{
    private RectTransform rect;

    private Vector2 BottomLeftCorner;

    private Vector2 mousePos;

    public PlayerController playerController;

    public RectTransform cameraOverlay;

    private World m_world;
    private GameManager m_gameManager;

    public Vector2 overlayOffset;

    // Start is called before the first frame update
    void Start()
    {
        m_world = FindObjectOfType<World>();
        m_gameManager = GameManager.Instance;
        rect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_world.m_seasonState == SeasonState.Summer && m_gameManager.m_gameState != GameState.Pause && (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)))
        {
            //mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //if (GetNormalizedPos().x > 0f && GetNormalizedPos().x < rect.rect.width && GetNormalizedPos().y > 0f && GetNormalizedPos().y < rect.rect.height)
            //{
            //    Debug.Log(GetNormalizedPos());
            //}
            
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out mousePos))
            {
                Debug.Log("Not returning");
            }

            if(mousePos.x>rect.rect.width/-2 && mousePos.x < rect.rect.width / 2 && mousePos.y > rect.rect.height / -2 && mousePos.y < rect.rect.height / 2)
            {
                //playerController.m_camera.transform.position = new Vector3(mousePos.x + rect.rect.width/2, playerController.m_camera.transform.position.y, mousePos.y+rect.rect.height / 2);
                playerController.SetCamPos(new Vector3(mousePos.x + rect.rect.width / 2, 0f, mousePos.y+ rect.rect.height / 2));
            }
        }

        cameraOverlay.anchoredPosition = playerController.GetCamPos() - new Vector2(rect.rect.width/2, rect.rect.height/2) + overlayOffset;
    }

    public void ChangeSize(float size)
    {
        cameraOverlay.sizeDelta = new Vector2(size * 4f, size* 3.5f);
    }
}
