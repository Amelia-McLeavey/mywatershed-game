using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class VariablesAffected 
{
    public string variableName;
    public float influencePerVol;
    public float targetValue;
    public float dropOffRate;
}

public class VolunteerManager : MonoBehaviour
{
    public VariablesAffected[] variablesAffected;

    [SerializeField] private int m_totalVolunteers;
     private int m_maxVolunteers;
    [SerializeField] private TMP_Text m_totalVolunteerText;
    [SerializeField] private int m_numberOfVolunteersPerClick=5;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject volunteerPrefab;
    [SerializeField] private GameObject volunteerOverlay;

    // Start is called before the first frame update
    void Start()
    {
        m_totalVolunteerText.text = m_totalVolunteers.ToString();
        m_maxVolunteers = m_totalVolunteers;
    }

    public void AddVolunteersToTile()
    {
        Volunteers volScript = playerController.variableHolder.GetComponent<Volunteers>();
        if (m_totalVolunteers > 0 && volScript.value < 5* m_numberOfVolunteersPerClick)
        {
            m_totalVolunteers = Mathf.Max(m_totalVolunteers - m_numberOfVolunteersPerClick, 0);
            m_totalVolunteerText.text = m_totalVolunteers.ToString();

            if(volScript.value == 0)
            {
                GameObject volunteer = Instantiate(volunteerPrefab, playerController.variableHolder.transform.position, Quaternion.identity);
                volunteer.transform.localPosition = volunteer.transform.localPosition + new Vector3(0f, playerController.variableHolder.transform.localScale.z/6f, 0f);
                volunteer.transform.SetParent(playerController.variableHolder.transform);

                volScript.SetSurroundingTiles(playerController.variableHolder.GetComponent<Tile>().m_TileIndex);

                foreach(GameObject tile in volScript.affectedTiles)
                {
                    volunteer = Instantiate(volunteerOverlay, tile.transform.position, Quaternion.identity);
                    volunteer.transform.localPosition = volunteer.transform.localPosition + new Vector3(0f, tile.transform.localScale.z / 6f, 0f);
                    volunteer.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
                    volunteer.transform.SetParent(tile.transform);
                }
            }

            volScript.value += m_numberOfVolunteersPerClick;

            
        }
       
    }

    public void TakeVolunteersFromTile()
    {
        Volunteers volScript = playerController.variableHolder.GetComponent<Volunteers>();

        if (volScript.value > 0)
        {
            volScript.value -= m_numberOfVolunteersPerClick;
            m_totalVolunteers = Mathf.Min(m_totalVolunteers + m_numberOfVolunteersPerClick, m_maxVolunteers);
            m_totalVolunteerText.text = m_totalVolunteers.ToString();

            if (volScript.value == 0)
            {
                foreach (GameObject tile in volScript.affectedTiles)
                {
                    foreach (Transform child in tile.transform)
                    {
                        if (child.CompareTag("Meeple"))
                        {
                            Destroy(child.gameObject);
                        }
                    }
                }
                
            }
        }

    }
}
