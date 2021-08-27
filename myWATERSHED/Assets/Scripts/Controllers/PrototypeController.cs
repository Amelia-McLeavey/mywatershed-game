using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeController : MonoBehaviour
{
    [SerializeField]
    private float m_cameraSpeed;

    [SerializeField]
    private UrbanFamilyType landFamilyType;

    [SerializeField]
    private WorldGenerator m_worldGenScript;
    [SerializeField]
    private FlowSimulator m_flowSimScript;

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private GameObject m_cameraContainer;

    public void GenerateWorldOnClick()
    {
        m_worldGenScript.GenerateWorld();
        m_cameraContainer.transform.position = new Vector3(29, 22, 21);

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
        // left mouse click
        if (Input.GetMouseButton(1))
        {
            // Create a ray from the point clicked on screen to the point in world space
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            // Pass ray into Raycast to get hit info
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Debug.Log("HIT");
                //hit.collider.gameObject.GetComponent<Tile>().DirectEffect();
                //hit.collider.gameObject.GetComponent<Tile>().ChangeMaterial(landFamilyType);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            //m_flowSimScript.FlowPulse();
        }



        if (Input.GetAxis("Horizontal") < 0)
        {
            //left
            m_cameraContainer.transform.position = new Vector3(transform.position.x + (-1f * m_cameraSpeed), transform.position.y, transform.position.z);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            //right

        }

        if (Input.GetAxis("Vertical") < 0)
        {

        }
        if (Input.GetAxis("Vertical") > 0)
        {

        }
        // CAMERA CONTROL
        //oh no 
        //map to the mouse or wsad
    }

}
