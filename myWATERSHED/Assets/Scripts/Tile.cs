using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{
    public BaseType m_Basetype;
    public PhysicalType m_PhysicalType;

    public Vector2 m_TileIndex;

    // INFORMATION VARIABLES
    private float m_redValue;

    private List<float> m_colorInfoItems = new List<float>();

    private List<GameObject> m_senderNeighbours = new List<GameObject>();
    private List<GameObject> m_recieverNeighbours = new List<GameObject>();

    private MeshRenderer m_meshRenderer;

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    //public void SetLandFamilyType(int enumIndex)
    //{
    //    //Debug.Log($"enum index = {enumIndex}");
    //    switch (enumIndex) 
    //    {
    //        case 1: m_meshRenderer.material = m_industrialMat; break;
    //        case 2: m_meshRenderer.material = m_infrastructureMat; break;
    //        case 3: m_meshRenderer.material = m_residentialMat; break;
    //        case 4: m_meshRenderer.material = m_agricultureMat; break;
    //        case 5: m_meshRenderer.material = m_recreationalMat; break;
    //        default: Debug.LogError("Could not assign new material"); break;
    //    }
    //}

    public void SetMaterial(Material material)
    {
        m_meshRenderer.material = material;
    }

    public void FindWaterNeighbours()
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

    public void DirectEffect()
    {
        m_redValue = 255f / 255f;
        m_meshRenderer.material.color = new Color(m_redValue, 124f / 255f, 200f / 255f, 200f / 255f);
    }

    //public void ChangeMaterial(UrbanFamilyType landFamilyType)
    //{
    //    switch ((int)landFamilyType)
    //    {
    //        case 1: m_meshRenderer.material = m_industrialMat; break;
    //        case 2: m_meshRenderer.material = m_infrastructureMat; break;
    //        case 3: m_meshRenderer.material = m_residentialMat; break;
    //        default: break;
    //    }
    //}

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
