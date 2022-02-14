using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacementOverlay : MonoBehaviour
{
    public bool placingCard= true;
    private Camera m_camera;
    void Start()
    {
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (placingCard)
        {
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    transform.position = hit.transform.position + new Vector3(0f, hit.transform.localScale.z / 6f, 0f); ;                 
                }
        }
    }
}
