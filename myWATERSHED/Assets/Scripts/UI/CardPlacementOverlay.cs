using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacementOverlay : MonoBehaviour
{
    private Camera m_camera;
    [SerializeField] private PlayedCardHolder playedCardHolder;
    [SerializeField] private MeshRenderer overlay;
    [SerializeField] private Color canBePlaced;
    [SerializeField] private Color canNotBePlaced;

    private bool shift=false;

    public bool ableToPlaceCard=false;
    public bool placing=false;

    public Vector3[] tileIndexOffsets;
    public Transform[] additionalOverlays;

    private World m_world;

    private Tile tileHit;
    void Start()
    {
        m_world = FindObjectOfType<World>();
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (placing)
        {
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                transform.position = hit.transform.position + new Vector3(0f, hit.transform.localScale.z / 6f, 0f);

                if (hit.collider.gameObject.GetComponent<Tile>() != null)
                {
                    tileHit = hit.collider.gameObject.GetComponent<Tile>();

                    if (tileHit.m_PhysicalType.ToString() == playedCardHolder.newestCard.cardInstance.tileType)
                    {
                        overlay.material.color = canBePlaced;
                        ableToPlaceCard = true;
                    }
                    else
                    {
                        overlay.material.color = canNotBePlaced;
                        ableToPlaceCard = false;
                    }

                    if (tileHit.m_TileIndex.y % 2 == 1)
                    {
                        shift = true;
                    }
                    else
                    {
                        shift = false;
                    }

                    for (int i =0; i<tileIndexOffsets.Length; i++)
                    {
                        if(i < playedCardHolder.newestCard.cardInstance.numberOfTiles - 1)
                        {
                            Vector2 additionalTileIndex = GetAdditionalTileIndex(tileHit.m_TileIndex,i);

                            if (TileManager.s_TilesDictonary.TryGetValue(additionalTileIndex, out GameObject value))
                            {
                                additionalOverlays[i].gameObject.SetActive(true);
                                Vector3 pos = value.transform.position + new Vector3(0f, value.transform.localScale.z / 6f, 0f);
                                additionalOverlays[i].position = pos;// new Vector3(pos.x, pos.z, pos.y);
                            }
                        }
                        else
                        {
                            additionalOverlays[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            transform.position = Vector3.zero;
        }

    }

    public Vector2 GetAdditionalTileIndex(Vector2 index, int num)
    {
        Vector2 outVec = Vector2.zero;
        if (index.y % 2 == 1)
        {
            outVec = index + new Vector2(tileIndexOffsets[num].x, tileIndexOffsets[num].y);
        }
        else
        {
            outVec = index + new Vector2(tileIndexOffsets[num].z, tileIndexOffsets[num].y);
        }
        
        return outVec;
    }
}
