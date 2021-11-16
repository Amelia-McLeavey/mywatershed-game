using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PrototypeController : MonoBehaviour
{
    [SerializeField]
    private float m_cameraSpeed;

    [SerializeField]
    private WorldGenerator m_worldGenScript;

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private GameObject m_cameraContainer;

    //very rough tile ui stuff, this is obv awful
    [SerializeField] private GameObject tileUI;
    public bool activeTileUI = false;
    [SerializeField] Text tileType;
    [SerializeField] Text variable1;
    [SerializeField] Text variable2;
    [SerializeField] Text variable3;
    [SerializeField] Text variable4;
    [SerializeField] Text variable5;
    [SerializeField] Text variable6;

    [SerializeField] Text variable1data;
    [SerializeField] Text variable2data;
    [SerializeField] Text variable3data;
    [SerializeField] Text variable4data;
    [SerializeField] Text variable5data;
    [SerializeField] Text variable6data;


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GenerateWorldOnClick()
    {
        m_worldGenScript.GenerateWorld();

        m_cameraContainer.transform.position = new Vector3(20f, 20f, 20f);
    }

    public void IncreaseSeedValue()
    {
        m_worldGenScript.m_Seed++;
    }

    public void DecreaseSeedValue()
    {
        m_worldGenScript.m_Seed--;
    }

    private void Update()
    {


        // TILE CLICKS

        //if (activeTileUI == true) {
        //    //display and update variables based on tile raycasted

        //}
        if (Input.GetMouseButtonDown(1)) // left mouse click
        {
            //toggle tile ui on right click
            if (activeTileUI == false) { activeTileUI = true; } else if (activeTileUI == true) { activeTileUI = false; }


            // Create a ray from the point clicked on screen to the point in world space
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            // Pass ray into Raycast to get hit info
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider.gameObject.tag);
                tileType.text = hit.collider.gameObject.tag;
                if(hit.collider.gameObject.tag == "Agriculture") {
                    variable1.text = "Asphalt Density";
                    variable1data.text = hit.collider.gameObject.GetComponent<AsphaltDensity>().m_AsphaltDensity.ToString();

                    variable2.text = "Erosion Rate";
                    variable2data.text = hit.collider.gameObject.GetComponent<ErosionRate>().m_ErosionRate.ToString();

                    variable3.text = "Land Height";
                    variable3data.text = hit.collider.gameObject.GetComponent<LandHeight>().m_LandHeight.ToString();

                    variable4.text = "Water Temperature";
                    variable4data.text = hit.collider.gameObject.GetComponent<WaterTemperature>().m_waterTemperature.ToString();

                    variable5.text = "Pollution Level";
                    variable5data.text =  hit.collider.gameObject.GetComponent<PollutionLevel>().m_PolutionLevel.ToString();

                    variable6.text = "Sewage Level";
                    variable6data.text = hit.collider.gameObject.GetComponent<SewageLevel>().m_SewageLevel.ToString();
                }
                
                
                /*
             pseudo:
             - get the tag from the tile
             - based on what we know from the components assigned to tags, get the relevant variable info and pass it to the UI element
             - activate the ui game object so we can view it
             - maybe...start a coroutine to keep updating the ui element so we can see the changes while it is active? or...need a way to keep it updating based on current tile.
             - click again and ui element is closed, can just use a bool like "activeTileUI" or something.
             
             */
            }
        }

        // CAMERA MOVEMENT
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_cameraContainer.transform.position = Vector3.MoveTowards(m_cameraContainer.transform.position, m_cameraContainer.transform.position + direction, m_cameraSpeed);

    }
}
