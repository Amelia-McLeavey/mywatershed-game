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
    [SerializeField] private Color cardActive;

    private bool shift = false;

    public bool ableToPlaceCard = false;
    public bool placing = false;

    public Vector3[] tileIndexOffsets;
    public Transform[] additionalOverlays;
    [SerializeField] private List<MeshRenderer> additionalMR = new List<MeshRenderer>();
    [SerializeField] private List<GameObject> tilesWithSuccessfulPlacement = new List<GameObject>();

    private List<GameObject> clickedOverlays = new List<GameObject>();

    private World m_world;

    private Tile tileHit;

    private bool followMouse = true;

    public GameObject clickOverlayObject;
    void Start()
    {
        m_world = FindObjectOfType<World>();
        m_camera = Camera.main;

        foreach (Transform addOverlay in additionalOverlays)
        {
            additionalMR.Add(addOverlay.GetComponent<MeshRenderer>());
        }
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

                    for (int i = 0; i < tileIndexOffsets.Length; i++)
                    {
                        if (i < playedCardHolder.newestCard.cardInstance.numberOfTiles - 1)
                        {
                            Vector2 additionalTileIndex = GetAdditionalTileIndex(tileHit.m_TileIndex, i);

                            if (TileManager.s_TilesDictonary.TryGetValue(additionalTileIndex, out GameObject value))
                            {
                                additionalOverlays[i].gameObject.SetActive(true);
                                Vector3 pos = value.transform.position + new Vector3(0f, value.transform.localScale.z / 6f, 0f);
                                additionalOverlays[i].position = pos;

                                if (value.GetComponent<Tile>().m_PhysicalType.ToString() == playedCardHolder.newestCard.cardInstance.tileType)
                                {
                                    additionalMR[i].material.color = canBePlaced;
                                }
                                else
                                {
                                    additionalMR[i].material.color = canNotBePlaced;
                                }
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


    public void ClickedOnTile()
    {
        tilesWithSuccessfulPlacement.Clear();
        if (clickedOverlays.Count > 0)
        {
            foreach (GameObject clickedOverlay in clickedOverlays)
            {
                Destroy(clickedOverlay);
            }

            clickedOverlays.Clear();
        }

        if (TileManager.s_TilesDictonary.TryGetValue(tileHit.m_TileIndex, out GameObject mainTile))
        {
            GameObject newOverlay = Instantiate(clickOverlayObject, mainTile.transform.position, Quaternion.identity);
            newOverlay.transform.localPosition = newOverlay.transform.localPosition + new Vector3(0f, mainTile.transform.localScale.z / 6f, 0f);
            newOverlay.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
            newOverlay.GetComponent<MeshRenderer>().material.color = overlay.material.color + new Color(0f, 0f, 0f, 0.2f); //make colour stronger
            newOverlay.transform.SetParent(mainTile.transform);
            tilesWithSuccessfulPlacement.Add(newOverlay);
            clickedOverlays.Add(newOverlay);
        }

        for (int i = 0; i < tileIndexOffsets.Length; i++)
        {
            if (additionalOverlays[i].gameObject.activeSelf)
            {
                Vector2 additionalTileIndex = GetAdditionalTileIndex(tileHit.m_TileIndex, i);

                if (TileManager.s_TilesDictonary.TryGetValue(additionalTileIndex, out GameObject tile))
                {
                    GameObject newOverlay = Instantiate(clickOverlayObject, tile.transform.position, Quaternion.identity);
                    newOverlay.transform.localPosition = newOverlay.transform.localPosition + new Vector3(0f, tile.transform.localScale.z / 6f, 0f);
                    newOverlay.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
                    newOverlay.GetComponent<MeshRenderer>().material.color = additionalMR[i].material.color + new Color(0f,0f,0f,0.2f); //make colour stronger

                    if(tile.GetComponent<Tile>().m_PhysicalType.ToString() == playedCardHolder.newestCard.cardInstance.tileType)
                    {
                        tilesWithSuccessfulPlacement.Add(newOverlay);
                    }
                    newOverlay.transform.SetParent(tile.transform);

                    clickedOverlays.Add(newOverlay);
                }
            }
        }
    }

    public void PlacedOnTile()
    {
        if (clickedOverlays.Count > 0)
        {
            foreach(GameObject clickedOverlay in clickedOverlays)
            {
                if (tilesWithSuccessfulPlacement.Contains(clickedOverlay))
                {
                    clickedOverlay.GetComponent<MeshRenderer>().material.color = cardActive;
                }
                else
                {
                    Destroy(clickedOverlay);
                }
            }



            clickedOverlays.Clear();
        }
    }
}
