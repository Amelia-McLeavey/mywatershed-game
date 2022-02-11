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
    private float cameraZOffset = 14f;

    private Vector2 minMaxXPosition;
    private Vector2 minMaxZPosition;

    private GameManager m_gameManager;

    [Header("Select Tile Variables")]
    
    public GameObject variableHolder;
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


    [SerializeField] private Heatmap m_heatMap;
    [SerializeField] private HeatmapClickableOverlay m_heatMapClickableOverlay;

    private int mapRows;
    private int mapColumns;

    
    // This region just holds functions for the Dev Generate Buttons
    // Will not be needed in the final game as players will not have these buttons
    #region Dev Generation Functions


    public void GenerateWorldOnClick()
    {
        m_loadingPanel.SetActive(true);
        WorldGenerator.OnWorldGenerationComplete += hideLoadingPanel;
        m_worldGenScript.GenerateWorld();      
    }

    private void hideLoadingPanel(int rows, int columns)
    {
        //center camera in the middle of the map
        targetCamPos = new Vector3(rows / 2, 20f, (columns * mapHeightMultiplyer / 2)-cameraZOffset);
        mapRows = rows;
        mapColumns = columns;
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
        if (m_world.m_seasonState == SeasonState.Summer)
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
                        //reset current material 
                        if (variableHolder != null)
                        {
                            variableHolder.GetComponent<MeshRenderer>().materials[1].color = storedColour;
                        }

                        //select new tile
                        variableHolder = hit.collider.gameObject;
                        storedColour = variableHolder.GetComponent<MeshRenderer>().materials[1].color;
                        variableHolder.GetComponent<MeshRenderer>().materials[1].color = highlightColour;
                        m_tileTitle.text = variableHolder.tag;
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


    public void DeselectTile()
    {
        if (variableHolder != null)
        {
            variableHolder.GetComponent<MeshRenderer>().materials[1].color = storedColour;
        }

        variableHolder = null;

        m_tileInfoObject.DeselectTile();
    }

    private void DisplayTileValues()
    {
        int variableNum = 0;

        if (variableHolder != null)
        {
            m_tileInfoObject.ChangeTile();
            VariableClass[] varClass = variableHolder.GetComponents<VariableClass>();
            foreach (VariableClass v in varClass)
            {
                if (v.variableName == "Volunteers")
                {
                    m_volunteerNum.text = v.value.ToString();
                }
                else if (v.value != 0)
                {
                    m_tileVariableObjects[variableNum].SetVariableClass(v);
                    m_tileVariableObjects[variableNum].gameObject.SetActive(true);
                    // tileVariableObjects[variableNum].SetText(v.variableName);
                    variableNum++;
                }
            }

            for(int i = variableNum; i< m_tileVariableObjects.Length; i++)
            {
                m_tileVariableObjects[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetCamPos(Vector3 position)
    {
        m_cameraContainer.transform.position = new Vector3(Mathf.Clamp(position.x, minMaxXPosition.x, minMaxXPosition.y), m_camera.transform.position.y, Mathf.Clamp(position.z - cameraZOffset, minMaxZPosition.x, minMaxZPosition.y));
        //targetCamPos = m_camera.transform.position;
    }

    public Vector2 GetCamPos()
    {
        return new Vector2(m_camera.transform.position.x, (m_camera.transform.position.z/ 0.857f) + cameraZOffset);
    }
}
