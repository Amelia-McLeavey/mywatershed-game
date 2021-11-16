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
        //prob dont need the bool can just check if the game object is active
        //    //display and update variables based on tile raycasted

        /*
        pseudo:
       - store the tile in a gameobject holder so we can keep referrencing the variables updating outside of the
       raycast
       - 
        */

        //}
        if (Input.GetMouseButtonDown(1)) // left mouse click
        {
            //toggle tile ui on right click
            if (activeTileUI == false) { activeTileUI = true; tileUI.SetActive(true); } else if (activeTileUI == true) { activeTileUI = false; tileUI.SetActive(false); }


            // Create a ray from the point clicked on screen to the point in world space
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            // Pass ray into Raycast to get hit info
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Debug.Log(hit.collider.gameObject.tag);
                
                //pass tile type to UI
                tileType.text = hit.collider.gameObject.tag;
               
                //all the land types
                if (hit.collider.gameObject.tag == "Agriculture" || hit.collider.gameObject.tag == "Commercial" || hit.collider.gameObject.tag == "EstateResidential" || hit.collider.gameObject.tag == "Forest" || hit.collider.gameObject.tag == "GolfCourse" || hit.collider.gameObject.tag == "HighDensity" || hit.collider.gameObject.tag == "Highway" || hit.collider.gameObject.tag == "Industrial" || hit.collider.gameObject.tag == "Institutional" || hit.collider.gameObject.tag == "LowMidDensity" || hit.collider.gameObject.tag == "Meadow" || hit.collider.gameObject.tag == "RecreationCentreSpace" || hit.collider.gameObject.tag == "Successional" || hit.collider.gameObject.tag == "UrbanOpenSpace" || hit.collider.gameObject.tag == "Vacant")
                {
                    variable1.text = "Asphalt Density";
                    variable1data.text = hit.collider.gameObject.GetComponent<AsphaltDensity>().m_AsphaltDensity.ToString();

                    variable2.text = "Erosion Rate";
                    variable2data.text = hit.collider.gameObject.GetComponent<ErosionRate>().m_ErosionRate.ToString();

                    variable3.text = "Land Height";
                    variable3data.text = hit.collider.gameObject.GetComponent<LandHeight>().m_LandHeight.ToString();

                    variable4.text = "Water Temperature";
                    variable4data.text = hit.collider.gameObject.GetComponent<WaterTemperature>().m_waterTemperature.ToString();

                    variable5.text = "Pollution Level";
                    variable5data.text = hit.collider.gameObject.GetComponent<PollutionLevel>().m_PolutionLevel.ToString();

                    variable6.text = "Sewage Level";
                    variable6data.text = hit.collider.gameObject.GetComponent<SewageLevel>().m_SewageLevel.ToString();
                }

                //all the water types
                if (hit.collider.gameObject.tag == "EngineeredReservoir" || hit.collider.gameObject.tag == "EngineeredStream" || hit.collider.gameObject.tag == "NaturalStream" || hit.collider.gameObject.tag == "Wetland")
                {
                    variable1.text = "Brown Trout Population";
                    variable1data.text = hit.collider.gameObject.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation.ToString();

                    variable2.text = "Creek Chub Population";
                    variable2data.text = hit.collider.gameObject.GetComponent<CreekChubPopulation>().m_CreekChubPopulation.ToString();

                    variable3.text = "Insect Population";
                    variable3data.text = hit.collider.gameObject.GetComponent<InsectPopulation>().m_InsectPopulation.ToString();

                    variable4.text = "Rate of Flow";
                    variable4data.text = hit.collider.gameObject.GetComponent<RateOfFlow>().m_RateOfFlow.ToString();

                    variable5.text = "Red Dace Population";
                    variable5data.text = hit.collider.gameObject.GetComponent<RedDacePopulation>().m_RedDacePopulation.ToString();

                    variable6.text = "Sewage Level";
                    variable6data.text = hit.collider.gameObject.GetComponent<SewageLevel>().m_SewageLevel.ToString();
                }

             
           

            }
        }

        // CAMERA MOVEMENT
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_cameraContainer.transform.position = Vector3.MoveTowards(m_cameraContainer.transform.position, m_cameraContainer.transform.position + direction, m_cameraSpeed);

    }
}
