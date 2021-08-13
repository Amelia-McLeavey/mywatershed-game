using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeController : MonoBehaviour
{
    [SerializeField] private float m_cameraSpeed;

    [SerializeField]
    private WorldGenerator m_worldGenScript;
    [SerializeField]
    private FlowSimulator m_flowSimScript;

    [SerializeField]
    private Camera m_camera;

    public void GenerateWorldOnClick()
    {
        m_worldGenScript.GenerateWorld();
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
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the point clicked on screen to the point in world space
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            // Pass ray into Raycast to get hit info
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Debug.Log("HIT");
                hit.collider.gameObject.GetComponent<Tile>().AffectTileVariables();
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            //m_flowSimScript.FlowPulse();
        }

        // CAMERA CONTROL
    }

}
