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
        if (Input.GetMouseButton(1)) // left mouse click
        {
            // Create a ray from the point clicked on screen to the point in world space
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            // Pass ray into Raycast to get hit info
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Debug.Log("HIT");
                //hit.collider.gameObject.GetComponent<Tile>().DirectEffect();
            }
        }

        // CAMERA MOVEMENT
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_cameraContainer.transform.position = Vector3.MoveTowards(m_cameraContainer.transform.position, m_cameraContainer.transform.position + direction, m_cameraSpeed);

    }
}
