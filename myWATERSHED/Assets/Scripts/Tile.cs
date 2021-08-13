using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{
    public BaseType m_Basetype;

    public Vector2 m_TileIndex;

    [SerializeField]
    private Material m_genericMaterial;
    [SerializeField] 
    private Material m_waterMaterial;
    [SerializeField] 
    private Material m_landMaterial;

    private float m_redValue;

    private List<float> m_colorInfoItems = new List<float>();
    private List<GameObject> m_senderNeighbours = new List<GameObject>();
    private List<GameObject> m_recieverNeighbours = new List<GameObject>();

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
            m_meshRenderer.material = m_landMaterial;
        }
        else if (m_Basetype == BaseType.None)
        {
            m_meshRenderer.material = m_genericMaterial;
        }
        else { Debug.LogError("Failed to set a base type."); }
    }

    public void FindNeighbours()
    {
        List<Vector2> neighbourIndexes = NeighbourUtility.GetNeighbours(m_TileIndex);

        // Separate neighbours by position and store
        List<Vector2> neighbourAboveIndexes = new List<Vector2>
        {
            neighbourIndexes[0], // 0
            neighbourIndexes[1], // 1
            neighbourIndexes[2]  // 2
        };
        List<Vector2> neighbourBelowIndexes = new List<Vector2>
        {
            neighbourIndexes[3], // 0
            neighbourIndexes[4], // 1
            neighbourIndexes[5]  // 2
        };

        foreach (Vector2 neighbourAboveIndex in neighbourAboveIndexes)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(neighbourAboveIndex, out GameObject neighbourAbove))
            {
                if (neighbourAbove.GetComponent<Tile>().m_Basetype == BaseType.Water)
                {
                    m_senderNeighbours.Add(neighbourAbove);
                }
            }
        }
        foreach (Vector2 neighbourBelowIndex in neighbourBelowIndexes)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(neighbourBelowIndex, out GameObject neighbourBelow))
            {
                if (neighbourBelow.GetComponent<Tile>().m_Basetype == BaseType.Water)
                {
                    m_recieverNeighbours.Add(neighbourBelow);
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
        foreach (GameObject reciever in m_recieverNeighbours)
        {
            reciever.GetComponent<Tile>().RecieveInformationFlow(m_redValue);
        }
    }

    public void RecieveInformationFlow(float colorInfo)
    {
        m_colorInfoItems.Add(colorInfo);
        if (m_colorInfoItems.Count == m_senderNeighbours.Count)
        {
            RefreshVariables();
        }
    }

    private void RefreshVariables()
    {
        float infoSum = m_colorInfoItems.Sum();
        float finalValue;

        // Calculate the final value depending on amount of info recieved
        if (m_colorInfoItems.Count > 1)
        { finalValue = infoSum / m_colorInfoItems.Count; }
        else { finalValue = infoSum / 2f; }
        m_colorInfoItems.Clear();

        // Refresh Variables and Apply Changes
        m_redValue  = finalValue;
        m_meshRenderer.material.color = new Color(m_redValue, 124f / 255f, 200f / 255f, 110f / 255f);
    }
}
