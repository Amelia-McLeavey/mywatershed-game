using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Variables")]

    [SerializeField] private float m_cameraSpeed;
    [SerializeField] private float m_cameraDragSpeed;
    public Camera m_camera;
    [SerializeField] private GameObject m_cameraContainer;
    [SerializeField] private float m_cameraPadding;

    [SerializeField] private Vector2 m_minMaxCameraZoom;
    [SerializeField] private float m_cameraZoomSpeed;

    private Vector3 targetCamPos;
    private bool mouseDown = false;
    private Vector2 storedMousePos;
    private double mouseDownTime;
    [SerializeField] private float m_clickSpeed = 0.2f;

    //used to find the actual z length of the map as the hexagons stack
    private float mapHeightMultiplyer = 0.857f;
    //as the camera is at an angle the z position needs to be offset
    //this can be calculated with camY / Tan(camAngle), which with the current settings equals 20
    public float cameraZOffset = 14f;

    private Vector2 minMaxXPosition;
    private Vector2 minMaxZPosition;

    private GameManager m_gameManager;

    [Header("Select Tile Variables")]
    
    public GameObject variableHolder;
    private GameObject preVariableHolder;
    private Color storedColour;
    [SerializeField] private Color highlightColour;

    [Header("World Gen")]
    [SerializeField] private GameObject m_loadingPanel;
    [SerializeField] private WorldGenerator m_worldGenScript;
    private World m_world;

    [Header("Tile Variables")]

    [SerializeField] private TileInfo m_tileInfoObject;
    [SerializeField] private TileVariableDisplay[] m_tileVariableObjects;
    [SerializeField] private TMP_Text m_tileTitle;
    [SerializeField] private Image m_tileImage;
    [SerializeField] private TMP_Text m_volunteerNum;
    [SerializeField] private GameObject m_volunteerData;
    [SerializeField] private GameObject m_volunteerFillerText;

    [SerializeField] private Heatmap m_heatMap;
    [SerializeField] private HeatmapClickableOverlay m_heatMapClickableOverlay;

    private int mapRows;
    private int mapColumns;

    [SerializeField] private BarGraph m_barGraph;
    private char[] TitleChars;
    private string Capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SerializeField] private CardPlacementOverlay m_cardOverlay;
    [SerializeField] private GameObject cardPlacementMessage;
    [SerializeField] private GameObject cardPlaceButton;

    private PlayedCard prevCard;
    private bool freshClick = true;
    // This region just holds functions for the Dev Generate Buttons
    // Will not be needed in the final game as players will not have these buttons
    #region Dev Generation Functions

    private void OnDestroy()
    {
        WorldGenerator.OnWorldGenerationComplete -= hideLoadingPanel;
    }

    public void GenerateWorldOnClick()
    {
        m_loadingPanel.SetActive(true);
        WorldGenerator.OnWorldGenerationComplete += hideLoadingPanel;
        m_worldGenScript.GenerateWorld();      
    }

    private void hideLoadingPanel(int rows, int columns)
    {
        //center camera in the middle of the map
        targetCamPos = new Vector3(rows / 2, 20f, (columns * mapHeightMultiplyer / 2) - cameraZOffset);
        mapRows = rows;
        mapColumns = columns;
        //Debug.Log("Camera: " + m_camera + "   Container: " + m_cameraContainer + "   Target pos: " + targetCamPos);

        if (m_camera == null)
        {
            m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        if (m_cameraContainer == null)
        {
            m_cameraContainer = GameObject.Find("CameraContainer");
        }

        //Debug.Log("Camera: " + m_camera + "   Container: " + m_cameraContainer + "   Target pos: " + targetCamPos);

        m_cameraContainer.transform.position = targetCamPos;
        //calculate camera restraints
        CalculateCameraPadding();

        m_heatMap.GenerateMaps();
        m_loadingPanel.SetActive(false);
    }

    public void IncreaseSeedValue()
    {
        m_worldGenScript.m_Seed++;
    }

    public void DecreaseSeedValue()
    {
        m_worldGenScript.m_Seed--;
    }
    #endregion

    private void Start()
    {
        m_world = FindObjectOfType<World>();
        variableHolder = null;
        m_gameManager = GameManager.Instance;
        GenerateWorldOnClick();
    }

    private void Update()
    {
        //if the game is currently playing
        if (m_world.m_seasonState == SeasonState.Summer && m_gameManager.m_gameState != GameState.Pause)
        {
            DisplayTileValues();

            //Clicking down
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !mouseDown)
            {
                mouseDown = true;
                storedMousePos = Input.mousePosition;
                mouseDownTime = Time.realtimeSinceStartupAsDouble;
            }

            //click released
            if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && mouseDown)
            {
                mouseDown = false;
                //finds if the player has clicked quick enough and the mouse isnt currently over some UI
                if (Time.realtimeSinceStartupAsDouble - mouseDownTime < m_clickSpeed && !EventSystem.current.IsPointerOverGameObject())
                {
                    //select this tile
                    Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                   
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        SelectTile(hit.collider.gameObject);
                    }
                }
            }


            ///////////////////         Movement           ///////////////////////////////////
            
            Vector3 direction = Vector3.zero;
            if (mouseDown)
            {
                //move with dragging
                direction = new Vector3(storedMousePos.x - Input.mousePosition.x, 0f, storedMousePos.y - Input.mousePosition.y) * m_cameraDragSpeed;
                storedMousePos = Input.mousePosition;
            }
            else if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                //move camera with keyboard
                direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            }
            else
            {
                //move when mouse is off screen
                if (Input.mousePosition.x < 0)
                {
                    direction.x = -1f;
                }
                else if (Input.mousePosition.x>Screen.width)
                {
                    direction.x = 1f;
                }

                if (Input.mousePosition.y < 0)
                {
                    direction.z = -1f;
                }
                else if (Input.mousePosition.y > Screen.height)
                {
                    direction.z = 1f;
                }

                //Debug.Log(direction);

            }

            targetCamPos = m_cameraContainer.transform.position + (direction.normalized * m_cameraSpeed);
            targetCamPos = new Vector3(Mathf.Clamp(targetCamPos.x, minMaxXPosition.x, minMaxXPosition.y), targetCamPos.y, Mathf.Clamp(targetCamPos.z, minMaxZPosition.x, minMaxZPosition.y));


            //////////////////         Zooming         /////////////////////////////////

            //zoom with keybaord
            if (Input.GetKey(KeyCode.Equals) && m_camera.orthographicSize > m_minMaxCameraZoom.x)
            {
                m_camera.orthographicSize = Mathf.Max(m_camera.orthographicSize - (m_cameraZoomSpeed * Time.deltaTime), m_minMaxCameraZoom.x);
                CalculateCameraPadding();
                m_heatMapClickableOverlay.ChangeSize(m_camera.orthographicSize);
            }

            if (Input.GetKey(KeyCode.Minus) && m_camera.orthographicSize < m_minMaxCameraZoom.y)
            {
                m_camera.orthographicSize = Mathf.Min(m_camera.orthographicSize + (m_cameraZoomSpeed * Time.deltaTime), m_minMaxCameraZoom.y);
                CalculateCameraPadding();
                m_heatMapClickableOverlay.ChangeSize(m_camera.orthographicSize);
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                //scroll wheel zoom
                m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize - (Input.mouseScrollDelta.y * m_cameraZoomSpeed), m_minMaxCameraZoom.x, m_minMaxCameraZoom.y);
                CalculateCameraPadding();
                m_heatMapClickableOverlay.ChangeSize(m_camera.orthographicSize);
            }

        }

        m_cameraContainer.transform.position = Vector3.Lerp(m_cameraContainer.transform.position, targetCamPos, Time.deltaTime);

    }

    private void CalculateCameraPadding()
    {
        minMaxXPosition = new Vector2(m_cameraPadding + (m_camera.orthographicSize * 1.75f), mapRows - m_cameraPadding - (m_camera.orthographicSize * 1.75f));
        minMaxZPosition = new Vector2(-cameraZOffset + m_cameraPadding + (m_camera.orthographicSize * 1.4f),(-cameraZOffset + mapColumns - m_cameraPadding) * mapHeightMultiplyer - (m_camera.orthographicSize * 1.4f));
    }

    public void SelectTile(GameObject tileObject)
    {
        if (variableHolder != null)
        {
            variableHolder.GetComponent<MeshRenderer>().materials[1].color = storedColour;
        }

        //select new tile
        variableHolder = tileObject;
        storedColour = variableHolder.GetComponent<MeshRenderer>().materials[1].color;
        variableHolder.GetComponent<MeshRenderer>().materials[1].color = highlightColour;
        TitleChars = variableHolder.tag.ToCharArray();
        m_tileTitle.text = "";
        foreach (char letter in TitleChars)
        {
            if (Capitals.Contains(letter.ToString()))
            {
                m_tileTitle.text += " ";
            }
            m_tileTitle.text += letter;
        }

        if (m_cardOverlay.placing)
        {
            m_cardOverlay.ClickedOnTile();
        }

        freshClick = true;
    }

    public void DeselectTile()
    {
        if (variableHolder != null)
        {
            variableHolder.GetComponent<MeshRenderer>().materials[1].color = storedColour;
        }
        if (prevCard != null)
        {
            prevCard.doPopUp = false;
        }
        variableHolder = null;
        preVariableHolder = null;
        m_tileInfoObject.DeselectTile();
    }


    private int variableNum = 0;
    private VariableClass vols;
    private void DisplayTileValues()
    {
        if (variableHolder != null && freshClick)
        {
            freshClick = false;
            variableNum = 0;
            m_tileImage.color = variableHolder.GetComponent<MeshRenderer>().material.color + new Color(0f, 0f, 0f, 1f);

            m_barGraph.gameObject.SetActive(false);

            bool canAddVol = true;
            bool canShowBiotic = false;

            foreach (Transform child in variableHolder.transform)
            {
                if (child.CompareTag("VolunteerOverlay"))
                {
                    canAddVol = false;
                }
                if (child.CompareTag("Meeple"))
                {
                    canAddVol = true;
                    break;
                }
            }

            m_volunteerData.SetActive(canAddVol);
            m_volunteerFillerText.SetActive(!canAddVol);

            m_barGraph.ResetAllValues();

            if (variableHolder != preVariableHolder)
            {
                m_tileInfoObject.ChangeTile();
                preVariableHolder = variableHolder;
            }

            VariableClass[] varClass = variableHolder.GetComponents<VariableClass>();
            foreach (VariableClass v in varClass)
            {
                if (v.variableName == "Volunteers")
                {
                    vols = v;
                }
                else if (v.variableName.Contains("Population") || v.variableName.Contains("Riverbed") || v.variableName.Contains("Riparian"))
                {
                    canShowBiotic = true;
                    m_tileInfoObject.showButtons = true;
                    if (v.variableName.Contains("Red"))
                    {
                        m_barGraph.redDacePop = v.value;
                    }
                    else if (v.variableName.Contains("Chub"))
                    {
                        m_barGraph.chubPop = v.value;
                    }
                    else if (v.variableName.Contains("Trout"))
                    {
                        m_barGraph.troutPop = v.value;
                    }
                    else
                    {
                        m_tileVariableObjects[variableNum].SetVariableClass(v);
                        m_tileVariableObjects[variableNum].abiotic = false;

                        variableNum++;
                    }
                }
                else if (v.value != 0)
                {
                    m_tileVariableObjects[variableNum].SetVariableClass(v);
                    m_tileVariableObjects[variableNum].abiotic = true;

                    // tileVariableObjects[variableNum].SetText(v.variableName);
                    variableNum++;
                }
            }

            if (!m_tileInfoObject.displayAbiotic && !canShowBiotic)
            {
                m_tileInfoObject.displayAbiotic = true;
            }

            for (int i = variableNum; i < m_tileVariableObjects.Length; i++)
            {
                m_tileVariableObjects[i].gameObject.SetActive(false);
            }


            //Card Placement UI

            if (m_cardOverlay.placing)
            {
                if (m_cardOverlay.ableToPlaceCard)
                {
                    cardPlaceButton.SetActive(true);
                    cardPlacementMessage.SetActive(false);
                }
                else 
                {
                    cardPlaceButton.SetActive(false);
                    cardPlacementMessage.SetActive(true);
                }
            }
            else
            {
                cardPlaceButton.SetActive(false);
                cardPlacementMessage.SetActive(false);
            }

            if(prevCard != null)
            {
                prevCard.doPopUp = false;
            }
            if (variableHolder.GetComponent<Tile>().currentCard != null)
            {  
                prevCard = variableHolder.GetComponent<Tile>().currentCard;
                prevCard.doPopUp = true;
            }
            //m_tileInfoObject.UpdateTileCard(variableHolder.GetComponent<Tile>().currentCard);
        }

        if (!m_tileInfoObject.displayAbiotic)
        {
            m_barGraph.ShowGraph();
        }

        m_barGraph.gameObject.SetActive(!m_tileInfoObject.displayAbiotic);

        for (int i = 0; i < variableNum; i++)
        {
            m_tileVariableObjects[i].gameObject.SetActive(m_tileVariableObjects[i].abiotic == m_tileInfoObject.displayAbiotic);
        }

        if (vols != null)
        {
            m_volunteerNum.text = vols.value.ToString();
        }
    }

    public void SetCamPos(Vector3 position)
    {
        m_cameraContainer.transform.position = new Vector3(Mathf.Clamp(position.x, minMaxXPosition.x, minMaxXPosition.y), m_camera.transform.position.y, Mathf.Clamp(position.z - cameraZOffset, minMaxZPosition.x, minMaxZPosition.y));
        //targetCamPos = m_camera.transform.position;
    }

    public Vector2 GetCamPos()
    {
        return new Vector2(m_camera.transform.position.x, (m_camera.transform.position.z/ 0.857f) + cameraZOffset +1f);
    }

    public void CallUpdateTileCard()
    {
        if (variableHolder != null)
        {
           // m_tileInfoObject.UpdateTileCard(variableHolder.GetComponent<Tile>().currentCard);
        }
    }
}
