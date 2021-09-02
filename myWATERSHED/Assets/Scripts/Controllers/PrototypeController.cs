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
    private FlowSimulator m_flowSimScript;

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private GameObject m_cameraContainer;

    [SerializeField]
    private int m_redDaceSpawnerCount;
    [SerializeField]
    private int m_infoSpreaderCount;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GenerateWorldOnClick()
    {
        m_worldGenScript.GenerateWorld();

        RedDaceSpawn();
        InfoSpreaderSpawn();

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

        // FLOW TEST
        if (Input.GetButtonDown("Jump"))
        {
            m_flowSimScript.FlowPulse();
        }

        // CAMERA MOVEMENT
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_cameraContainer.transform.position = Vector3.MoveTowards(m_cameraContainer.transform.position, m_cameraContainer.transform.position + direction, m_cameraSpeed);

    }

    ////////// THESE MEHTODS LIKELY TO BE TOSSED //////////

    private void RedDaceSpawn() 
    {
        // DEFINE TILE SET
        List<Vector2> tileSet = new List<Vector2>();

        foreach (KeyValuePair<Vector2, GameObject> tile in WorldGenerator.s_TilesDictonary)
        {
            if (tile.Value.GetComponent<Tile>().m_PhysicalType == PhysicalType.NaturalStream)
            {
                tileSet.Add(tile.Key);
            }
        }

        // Set a number of tiles to be spawners
        for (int i = 0; i < m_redDaceSpawnerCount; i++)
        {
            int randomIndex = Random.Range(0, tileSet.Count);

            if (WorldGenerator.s_TilesDictonary.TryGetValue(tileSet[randomIndex], out GameObject value))
            {
                value.GetComponent<Tile>().SetAsSpawner(true);
            }

        }
    }

    private void InfoSpreaderSpawn()
    {
        // DEFINE TILE SET
        List<Vector2> tileSet = new List<Vector2>();

        foreach (KeyValuePair<Vector2, GameObject> tile in WorldGenerator.s_TilesDictonary)
        {
            if (tile.Value.GetComponent<Tile>().m_PhysicalType == PhysicalType.Agriculture)
            {
                tileSet.Add(tile.Key);
            }
        }

        // Set a number of tiles to be spawners
        for (int i = 0; i < m_infoSpreaderCount; i++)
        {
            int randomIndex = Random.Range(0, tileSet.Count);

            if (WorldGenerator.s_TilesDictonary.TryGetValue(tileSet[randomIndex], out GameObject value))
            {
                value.GetComponent<Tile>().SetAsSpawner(true);
            }
        }
    }
}
