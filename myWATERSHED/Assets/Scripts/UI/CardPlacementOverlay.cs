using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacementOverlay : MonoBehaviour
{
    private Camera m_camera;
    [SerializeField] private PlaceableCardUI placeableCard;
    [SerializeField] private MeshRenderer overlay;
    [SerializeField] private Color canBePlaced;
    [SerializeField] private Color canNotBePlaced;

    public bool ableToPlaceCard=false;

    void Start()
    {
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.transform.position + new Vector3(0f, hit.transform.localScale.z / 6f, 0f);


            if (hit.collider.gameObject.GetComponent<Tile>() != null)
            {
                if (hit.collider.gameObject.GetComponent<Tile>().m_PhysicalType.ToString() == placeableCard.cardInstance.tileType)
                {
                    overlay.material.color = canBePlaced;
                    ableToPlaceCard = true;
                }
                else
                {
                    overlay.material.color = canNotBePlaced;
                    ableToPlaceCard = false;
                }
            }
        }

    }
}
