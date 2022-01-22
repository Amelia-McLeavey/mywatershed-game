using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Variables")]

    [SerializeField] private float m_cameraSpeed;
    [SerializeField] private Camera m_camera;
    [SerializeField] private GameObject m_cameraContainer;
    [SerializeField] private float m_cameraPadding;

    [SerializeField] private Vector2 minMaxCameraZoom;
    [SerializeField] private float cameraZoomSpeed;

    private Vector3 targetCamPos;
    private bool mouseDown = false;
    private Vector2 storedMousePos;
    private double mouseDownTime;
    [SerializeField] private float clickSpeed = 0.2f;

    //used to find the actual z length of the map as the hexagons stack
    private float mapHeightMultiplyer = 0.86f;
    //as the camera is at an angle the z position needs to be offset
    //this can be calculated with camY / Tan(camAngle), which with the current settings equals 20
    private float cameraZOffset = 20f;

    private Vector2 minMaxXPosition;
    private Vector2 minMaxZPosition;

    [Header("Select Tile Variables")]
    
    public GameObject variableHolder;
    [SerializeField] private Material highlightMat;

    [Header("World Gen")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private WorldGenerator m_worldGenScript;

    [Header("Tile Variables")]

    [SerializeField] private GameObject tileInfoObject;
    [SerializeField] private TileVariableDisplay[] tileVariableObjects;
    [SerializeField] private TMP_Text tileTitle;
    [SerializeField] private Image tileImage;

    

    // This region just holds functions for the Dev Generate Buttons
    // Will not be needed in the final game as players will not have these buttons
    #region Dev Generation Functions
    public void GenerateWorldOnClick()
    {
        loadingPanel.SetActive(true);
        WorldGenerator.OnWorldGenerationComplete += hideLoadingPanel;
        m_worldGenScript.GenerateWorld();      
    }

    private void hideLoadingPanel(int rows, int columns)
    {
        //center camera in the middle of the map
        targetCamPos = new Vector3(rows / 2, 20f, columns * mapHeightMultiplyer / 2);

        m_cameraContainer.transform.position = targetCamPos;
        //calculate camera restraints
        minMaxXPosition = new Vector2(0 + m_cameraPadding, rows - m_cameraPadding);
        minMaxZPosition = new Vector2(-cameraZOffset + m_cameraPadding, (columns * mapHeightMultiplyer) - cameraZOffset - m_cameraPadding);
        loadingPanel.SetActive(false);
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
        variableHolder = null;

        GenerateWorldOnClick();
    }

    private void Update()
    {
        DisplayTileValues();

        //Clicking
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&&!mouseDown)
        {
            mouseDown = true;
            storedMousePos= Input.mousePosition;
            mouseDownTime = Time.realtimeSinceStartupAsDouble;
        }

        if((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))&&mouseDown)
        {
            mouseDown = false;  
            if(Time.realtimeSinceStartupAsDouble - mouseDownTime < clickSpeed)
            {
                //select this tile
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    //reset current material 

                    variableHolder = hit.collider.gameObject;
                    variableHolder.GetComponent<MeshRenderer>().materials[1] = highlightMat;                 
                }
            }
        }


        ///////////////////         Movement           ///////////////////////////////////
        
        if (mouseDown)
        {
            //move with dragging
        }
        else
        {
            //move camera with keyboard
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetCamPos = m_cameraContainer.transform.position + (direction.normalized * m_cameraSpeed);
            targetCamPos = new Vector3(Mathf.Clamp(targetCamPos.x, minMaxXPosition.x, minMaxXPosition.y), targetCamPos.y, Mathf.Clamp(targetCamPos.z, minMaxZPosition.x, minMaxZPosition.y));
        }


        
        //////////////////         Zooming         /////////////////////////////////
        
        //zoom with keybaord
        if(Input.GetKey(KeyCode.Equals) && m_camera.orthographicSize > minMaxCameraZoom.x)
        {
            m_camera.orthographicSize = Mathf.Max(m_camera.orthographicSize- (cameraZoomSpeed * Time.deltaTime), minMaxCameraZoom.x);
        }

        if (Input.GetKey(KeyCode.Minus) && m_camera.orthographicSize < minMaxCameraZoom.y)
        {
            m_camera.orthographicSize = Mathf.Min(m_camera.orthographicSize + (cameraZoomSpeed*Time.deltaTime), minMaxCameraZoom.y);
        }

        //scroll wheel zoom
        m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize - (Input.mouseScrollDelta.y * cameraZoomSpeed), minMaxCameraZoom.x, minMaxCameraZoom.y);
    }

    private void LateUpdate()
    {
        m_cameraContainer.transform.position = Vector3.Lerp(m_cameraContainer.transform.position, targetCamPos, 0.1f);
    }

    private void DisplayTileValues()
    {
        int variableNum = 0;
        if (variableHolder != null)
        {
            VariableClass[] varClass = variableHolder.GetComponents<VariableClass>();
            foreach (VariableClass v in varClass)
            {
                tileVariableObjects[variableNum].SetVariableClass(v);
                tileVariableObjects[variableNum].gameObject.SetActive(true);
                // tileVariableObjects[variableNum].SetText(v.variableName);
                variableNum++;
            }

            for(int i = variableNum; i< tileVariableObjects.Length; i++)
            {
                tileVariableObjects[i].gameObject.SetActive(false);
            }
        }
    }
}
