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

    [SerializeField]
    private GameObject m_buttonToDisable1;
    [SerializeField]
    private GameObject m_buttonToDisable2;
    [SerializeField]
    private GameObject m_buttonToDisable3;


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

    //empty object to store variables from tiles
    public GameObject variableHolder;
    //material to store tile material when swapping to highlight material
    public Material storedMat;
    public Material highlightMat;
    //how much the UI buttons increase/decrease variables in the prototype UI display
    public float UIButtonIncrementAmount;

    public void GenerateWorldOnClick()
    {
        m_buttonToDisable1.SetActive(false);
        m_buttonToDisable2.SetActive(false);
        m_buttonToDisable3.SetActive(false);

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

    private void Start()
    {
        variableHolder = null;
    }

    private void Update()
    {

        //passing info from tiles' scripts to the UI
        //all the land types
        if (variableHolder != null)
        {
         /*   tileType.text = variableHolder.tag;

            if (variableHolder.tag == "Agriculture" || variableHolder.tag == "Commercial" || variableHolder.tag == "EstateResidential" || variableHolder.tag == "Forest" || variableHolder.tag == "GolfCourse" || variableHolder.tag == "HighDensity" || variableHolder.tag == "Highway" || variableHolder.tag == "Industrial" || variableHolder.tag == "Institutional" || variableHolder.tag == "LowMidDensity" || variableHolder.tag == "Meadow" || variableHolder.tag == "RecreationCentreSpace" || variableHolder.tag == "Successional" || variableHolder.tag == "UrbanOpenSpace" || variableHolder.tag == "Vacant")
            {
                variable1.text = "Asphalt Density";
                variable1data.text = variableHolder.GetComponent<AsphaltDensity>().m_AsphaltDensity.ToString();

                variable2.text = "Erosion Rate";
                variable2data.text = variableHolder.GetComponent<ErosionRate>().m_ErosionRate.ToString();

                variable3.text = "Land Height";
                variable3data.text = variableHolder.GetComponent<LandHeight>().m_LandHeight.ToString();

                variable4.text = "Water Temperature";
                variable4data.text = variableHolder.GetComponent<WaterTemperature>().m_waterTemperature.ToString();

                variable5.text = "Pollution Level";
                variable5data.text = variableHolder.GetComponent<PollutionLevel>().m_PolutionLevel.ToString();

                variable6.text = "Sewage Level";
                variable6data.text = variableHolder.GetComponent<SewageLevel>().m_SewageLevel.ToString();
            }

            //all the water types
            if (variableHolder.tag == "EngineeredReservoir" || variableHolder.tag == "EngineeredStream" || variableHolder.tag == "NaturalStream" || variableHolder.tag == "Wetland")
            {
                variable1.text = "Brown Trout Population";
                variable1data.text = variableHolder.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation.ToString();

                variable2.text = "Creek Chub Population";
                variable2data.text = variableHolder.GetComponent<CreekChubPopulation>().m_CreekChubPopulation.ToString();

                variable3.text = "Insect Population";
                variable3data.text = variableHolder.GetComponent<InsectPopulation>().m_InsectPopulation.ToString();

                variable4.text = "Rate of Flow";
                variable4data.text = variableHolder.GetComponent<RateOfFlow>().m_RateOfFlow.ToString();

                variable5.text = "Red Dace Population";
                variable5data.text = variableHolder.GetComponent<RedDacePopulation>().m_RedDacePopulation.ToString();

                variable6.text = "Sewage Level";
                variable6data.text = variableHolder.GetComponent<SewageLevel>().m_SewageLevel.ToString();
            }*/
        }


        //}
        if (Input.GetMouseButtonDown(1)) // right mouse click
        {
            //TEMPORARILY REMOVED TOGGLE FOR PROTOTYPE, UNCOMMENT BELOW IF DESIRABLE

            ////toggle tile ui on right click
            //if (activeTileUI == false) { activeTileUI = true; tileUI.SetActive(true); } else if (activeTileUI == true) { activeTileUI = false; tileUI.SetActive(false); }

         

            // Create a ray from the point clicked on screen to the point in world space
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            // Pass ray into Raycast to get hit info
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //if we're storing a tile, reset it's stored material before assigning this new tile
                if (variableHolder != null)
                {
                    variableHolder.GetComponent<MeshRenderer>().material = storedMat;
                }

                //store the tile we hit so we can access it's variables
                variableHolder = hit.collider.gameObject;

                //store the material from this tile for later
                storedMat = variableHolder.GetComponent<MeshRenderer>().material;
                //set the material to be the highlighted colour
                variableHolder.GetComponent<MeshRenderer>().material = highlightMat;


            }
        }

        // CAMERA MOVEMENT
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_cameraContainer.transform.position = Vector3.MoveTowards(m_cameraContainer.transform.position, m_cameraContainer.transform.position + direction, m_cameraSpeed);

    }

    // TODO: Adjust these values to not break the system.

    //some sloppy throwaway code to make buttons for the prototype's variable UI display
    /*
    #region VariableButtons
    public void variable1ButtonPlus()
    {

        if (variableHolder.GetComponent<AsphaltDensity>())
        {
            variableHolder.GetComponent<AsphaltDensity>().m_AsphaltDensity += UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<BrownTroutPopulation>())
        {
            variableHolder.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation += (int)UIButtonIncrementAmount;
        }
    }

    public void variable1ButtonMinus()
    {
        if (variableHolder.GetComponent<AsphaltDensity>())
        {
            variableHolder.GetComponent<AsphaltDensity>().m_AsphaltDensity -= UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<BrownTroutPopulation>())
        {
            variableHolder.GetComponent<BrownTroutPopulation>().m_BrownTroutPopulation -= (int)UIButtonIncrementAmount;
        }
    }

    public void variable2ButtonPlus()
    {


        if (variableHolder.GetComponent<ErosionRate>())
        {
            variableHolder.GetComponent<ErosionRate>().m_ErosionRate += UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<CreekChubPopulation>())
        {
            variableHolder.GetComponent<CreekChubPopulation>().m_CreekChubPopulation += (int)UIButtonIncrementAmount;
        }
    }

    public void variable2ButtonMinus()
    {
        if (variableHolder.GetComponent<ErosionRate>())
        {
            variableHolder.GetComponent<ErosionRate>().m_ErosionRate -= UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<CreekChubPopulation>())
        {
            variableHolder.GetComponent<CreekChubPopulation>().m_CreekChubPopulation -= (int)UIButtonIncrementAmount;
        }
    }


    public void variable3ButtonPlus()
    {


        if (variableHolder.GetComponent<LandHeight>())
        {
            variableHolder.GetComponent<LandHeight>().m_LandHeight += UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<InsectPopulation>())
        {
            variableHolder.GetComponent<InsectPopulation>().m_InsectPopulation += (int)UIButtonIncrementAmount;
        }
    }

    public void variable3ButtonMinus()
    {


        if (variableHolder.GetComponent<LandHeight>())
        {
            variableHolder.GetComponent<LandHeight>().m_LandHeight -= UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<InsectPopulation>())
        {
            variableHolder.GetComponent<InsectPopulation>().m_InsectPopulation -= (int)UIButtonIncrementAmount;
        }
    }

    //for some reason rate of flow doesn't change...something weird with the flowtimer?
    public void variable4ButtonPlus()
    {

        if (variableHolder.GetComponent<WaterTemperature>())
        {
            variableHolder.GetComponent<WaterTemperature>().m_waterTemperature += UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<RateOfFlow>())
        {
            variableHolder.GetComponent<RateOfFlow>().m_RateOfFlow += UIButtonIncrementAmount;
        }
    }

    public void variable4ButtonMinus()
    {

        if (variableHolder.GetComponent<WaterTemperature>())
        {
            variableHolder.GetComponent<WaterTemperature>().m_waterTemperature -= UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<RateOfFlow>())
        {
            variableHolder.GetComponent<RateOfFlow>().m_RateOfFlow -= UIButtonIncrementAmount;
        }
    }

    public void variable5ButtonPlus()
    {

        if (variableHolder.GetComponent<PollutionLevel>())
        {
            variableHolder.GetComponent<PollutionLevel>().m_PolutionLevel += (int)UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<RedDacePopulation>())
        {
            variableHolder.GetComponent<RedDacePopulation>().m_RedDacePopulation += (int)UIButtonIncrementAmount;
        }
    }

    public void variable5ButtonMinus()
    {


        if (variableHolder.GetComponent<PollutionLevel>())
        {
            variableHolder.GetComponent<PollutionLevel>().m_PolutionLevel -= (int)UIButtonIncrementAmount;
        }
        else if (variableHolder.GetComponent<RedDacePopulation>())
        {
            variableHolder.GetComponent<RedDacePopulation>().m_RedDacePopulation -= (int)UIButtonIncrementAmount;
        }
    }

    public void variable6ButtonPlus()
    {

        variableHolder.GetComponent<SewageLevel>().m_SewageLevel += UIButtonIncrementAmount;
    }

    public void variable6ButtonMinus()
    {

        variableHolder.GetComponent<SewageLevel>().m_SewageLevel -= UIButtonIncrementAmount;
    }
    
    #endregion*/
}
