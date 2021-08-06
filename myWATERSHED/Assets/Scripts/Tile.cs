using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{
    public BaseType m_Basetype;

    [HideInInspector] 
    public Vector2 m_TileIndex;

    [SerializeField] 
    private Material m_waterMaterial;
    [SerializeField] 
    private Material m_genericLandMaterial;

    private float m_redValue;

    private List<float> colorInfoItems = new List<float>();
    private List<GameObject> senderNeighbours = new List<GameObject>();
    private List<GameObject> recieverNeighbours = new List<GameObject>();

    private MeshRenderer m_meshRenderer;

    public void SetBaseType(BaseType baseType)
    {
        m_Basetype = baseType;

        // SET MATERIAL
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (m_Basetype == BaseType.Water)
        {
            m_meshRenderer.material = m_waterMaterial;
        } 
        else if (m_Basetype == BaseType.Land)
        {
            m_meshRenderer.material = m_genericLandMaterial;
        }
        else { Debug.LogError("Failed to set a base type."); }
    }

    public void FindNeighbours()
    {
        // Indexes for each of a tile's 6 neighbours
        Vector2 tileUpIndex = new Vector2(m_TileIndex.x - 1, m_TileIndex.y);
        Vector2 tileDownIndex = new Vector2(m_TileIndex.x + 1, m_TileIndex.y);
        Vector2 tileUpLeftIndex;
        Vector2 tileUpRightIndex;
        Vector2 tileDownLeftIndex;
        Vector2 tileDownRightIndex;

        // If the Y of the index is an even number
        if (m_TileIndex.y % 2 == 0) 
        {
            tileUpLeftIndex = new Vector2(m_TileIndex.x - 1, m_TileIndex.y - 1);
            tileUpRightIndex = new Vector2(m_TileIndex.x - 1, m_TileIndex.y + 1);
            tileDownLeftIndex = new Vector2(m_TileIndex.x, m_TileIndex.y - 1);
            tileDownRightIndex = new Vector2(m_TileIndex.x, m_TileIndex.y + 1);
        }
        // If the Y of the index is an odd number
        else
        {
            tileUpLeftIndex = new Vector2(m_TileIndex.x, m_TileIndex.y - 1);
            tileUpRightIndex = new Vector2(m_TileIndex.x, m_TileIndex.y + 1);
            tileDownLeftIndex = new Vector2(m_TileIndex.x + 1, m_TileIndex.y - 1);
            tileDownRightIndex = new Vector2(m_TileIndex.x + 1, m_TileIndex.y + 1);
        }

        // Store all the gathered indexes in seperate lists
        List<Vector2> neighbourAboveIndexes = new List<Vector2>();
        List<Vector2> neighbourBelowIndexes = new List<Vector2>();

        neighbourAboveIndexes.Add(tileUpLeftIndex); //////////// 0
        neighbourAboveIndexes.Add(tileUpIndex); //////////////// 1
        neighbourAboveIndexes.Add(tileUpRightIndex); /////////// 2
        neighbourBelowIndexes.Add(tileDownLeftIndex); ////////// 0
        neighbourBelowIndexes.Add(tileDownIndex); ////////////// 1
        neighbourBelowIndexes.Add(tileDownRightIndex); ///////// 2

        foreach (Vector2 neighbourAboveIndex in neighbourAboveIndexes)
        {
            if (WorldGenerator.s_Tiles.TryGetValue(new Vector2(neighbourAboveIndex.x, neighbourAboveIndex.y), out GameObject neighbourAbove))
            {
                if (neighbourAbove.GetComponent<Tile>().m_Basetype == BaseType.Water)
                {
                    senderNeighbours.Add(neighbourAbove);
                }
            }
        }
        foreach (Vector2 neighbourBelowIndex in neighbourBelowIndexes)
        {
            if (WorldGenerator.s_Tiles.TryGetValue(new Vector2(neighbourBelowIndex.x, neighbourBelowIndex.y), out GameObject neighbourBelow))
            {
                if (neighbourBelow.GetComponent<Tile>().m_Basetype == BaseType.Water)
                {
                    recieverNeighbours.Add(neighbourBelow);
                }
            }
        }
    }

    public void AffectTileVariables()
    {
        m_redValue = 255f / 255f;
        m_meshRenderer.material.color = new Color(m_redValue, 124f / 255f, 200f / 255f, 200f / 255f);
    }

    public void SendInformationFlow()
    {
        foreach (GameObject reciever in recieverNeighbours)
        {
            reciever.GetComponent<Tile>().RecieveInformationFlow(m_redValue);
        }
    }

    public void RecieveInformationFlow(float colorInfo)
    {
        colorInfoItems.Add(colorInfo);
        if (colorInfoItems.Count == senderNeighbours.Count)
        {
            RefreshVariables();
        }
    }

    private void RefreshVariables()
    {
        float infoSum = colorInfoItems.Sum();
        float finalValue;

        // Calculate the final value depending on amount of info recieved
        if (colorInfoItems.Count > 1)
        { finalValue = infoSum / colorInfoItems.Count; }
        else { finalValue = infoSum / 2f; }
        colorInfoItems.Clear();

        // Refresh Variables and Apply Changes
        m_redValue  = finalValue;
        m_meshRenderer.material.color = new Color(m_redValue, 124f / 255f, 200f / 255f, 110f / 255f);
    }
}
