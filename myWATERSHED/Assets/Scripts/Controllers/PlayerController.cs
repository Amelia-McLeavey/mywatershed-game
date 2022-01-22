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

    [Header("World Gen")]

    [SerializeField] private WorldGenerator m_worldGenScript;

    [Header("Tile Variables")]

    [SerializeField] private GameObject tileInfoObject;
    [SerializeField] private GameObject tileVariableObject;
    [SerializeField] private TMP_Text tileTitle;
    [SerializeField] private Image tileImage;

    public GameObject variableHolder;


    // This region just holds functions for the Dev Generate Buttons
    // Will not be needed in the final game as players will not have these buttons
    #region Dev Generation Functions
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
    #endregion

    private void Start()
    {
        variableHolder = null;
    }


}
