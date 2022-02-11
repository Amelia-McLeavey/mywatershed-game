using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolunteerManager : MonoBehaviour
{
    [SerializeField] private int m_totalVolunteers;
     private int m_maxVolunteers;
    [SerializeField] private TMP_Text m_totalVolunteerText;
    [SerializeField] private int m_numberOfVolunteersPerClick=5;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject volunteerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        m_totalVolunteerText.text = m_totalVolunteers.ToString();
        m_maxVolunteers = m_totalVolunteers;
    }

    public void AddVolunteersToTile()
    {
        if (m_totalVolunteers > 0)
        {
            m_totalVolunteers = Mathf.Max(m_totalVolunteers - m_numberOfVolunteersPerClick, 0);
            m_totalVolunteerText.text = m_totalVolunteers.ToString();

            if(playerController.variableHolder.GetComponent<Volunteers>().value == 0)
            {
                GameObject volunteer = Instantiate(volunteerPrefab, playerController.variableHolder.transform.position, Quaternion.identity);
                volunteer.transform.localPosition = volunteer.transform.localPosition + new Vector3(0f, playerController.variableHolder.transform.localScale.z/6f, 0f);
                volunteer.transform.SetParent(playerController.variableHolder.transform);
            }

            playerController.variableHolder.GetComponent<Volunteers>().value += m_numberOfVolunteersPerClick;

           
        }
       
    }

    public void TakeVolunteersFromTile()
    {
        if (playerController.variableHolder.GetComponent<Volunteers>().value > 0)
        {
            playerController.variableHolder.GetComponent<Volunteers>().value -= m_numberOfVolunteersPerClick;
            m_totalVolunteers = Mathf.Min(m_totalVolunteers + m_numberOfVolunteersPerClick, m_maxVolunteers);
            m_totalVolunteerText.text = m_totalVolunteers.ToString();

            if (playerController.variableHolder.GetComponent<Volunteers>().value == 0)
            {
                foreach(Transform child in playerController.variableHolder.transform)
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
