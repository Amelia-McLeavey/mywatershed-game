using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{

    public bool m_IsSpawner;

    public BaseType m_Basetype;
    public PhysicalType m_PhysicalType;

    public Vector2 m_TileIndex;

    public float newValue;

    // INFORMATION VARIABLES

    public float saturation;

    private List<float> m_infoItems = new List<float>();

    private List<GameObject> m_senderNeighbours = new List<GameObject>();
    private List<GameObject> m_recieverNeighbours = new List<GameObject>();

    private MeshRenderer m_meshRenderer;

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetTypeColor(Color colour)
    {
        m_meshRenderer.materials[0].color = colour;
        m_meshRenderer.materials[1].color = new Color(colour.r - 0.2f, colour.g - 0.2f, colour.b - 0.2f);
        //m_meshRenderer.materials[1].color = Color;
    }

    public void FindWaterNeighbours()
    {
        // Find neighbours
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

    public void FindLandNeighbours()
    {
        // Find neighbours
        List<Vector2> neighbourIndexes = NeighbourUtility.GetNeighbours(m_TileIndex);

        // Select the neighbour with the lowest and highest height
        GameObject lowestNeighbour = gameObject;
        GameObject highestNeighbour = gameObject;

        foreach (Vector2 neighbourIndex in neighbourIndexes)
        {
            if (WorldGenerator.s_TilesDictonary.TryGetValue(neighbourIndex, out GameObject value))
            {
                if (value.transform.localScale.z < lowestNeighbour.transform.localScale.z)
                {
                    lowestNeighbour = value;
                }
                if (value.transform.localScale.z > highestNeighbour.transform.localScale.z)
                {
                    highestNeighbour = value;
                }
            }
        }

        // Add the lowest neighbour as a reciever and the highest neighbour as a sender
        m_recieverNeighbours.Add(lowestNeighbour);
        m_senderNeighbours.Add(highestNeighbour);
    }

    public void SetAsSpawner(bool value)
    {
        m_IsSpawner = value;

        if (m_Basetype == BaseType.Water)
        {
            saturation = 100.0f;
            m_meshRenderer.materials[1].color = Color.red;
        }

        if (m_Basetype == BaseType.Land)
        {
            saturation = 100.0f;
            m_meshRenderer.materials[1].color = Color.blue;
        }

    }

    public void DirectEffect()
    {
        m_meshRenderer.materials[1].color = Color.red;
    }

    public void SendInformationFlow()
    {
        foreach (GameObject reciever in m_recieverNeighbours)
        {
            // Preserves the spawn tile by only alllowing tiles that are not spawners to recieve information
            if (reciever.GetComponent<Tile>().m_IsSpawner == false)
            {
                reciever.GetComponent<Tile>().RecieveInformationFlow(saturation);
            }
        }
    }

    public void RecieveInformationFlow(float info)
    {
        m_infoItems.Add(info);
        if (m_infoItems.Count == m_senderNeighbours.Count)
        {
            RefreshVariables();
        }
        
    }

    private void RefreshVariables()
    {
        List<float> values = new List<float>();

        foreach (float c in m_infoItems)
        {
            values.Add(c);
        }

        float infoSum = values.Sum();
        float finalValue;

        if (m_infoItems.Count > 1)
        { finalValue = infoSum / m_infoItems.Count; }
        else { finalValue = infoSum; }

        m_infoItems.Clear();

        saturation = finalValue;
        saturation = Mathf.Clamp(saturation, 0f, 100f);
        if (saturation < 1f) { saturation = 0f; }
        newValue = Mathf.Clamp(saturation * 2.55f, 0f, 255f);

        // Refresh Variables and Apply Changes
        m_meshRenderer.materials[1].color = new Color(newValue / 255f, 0f, 0f, m_meshRenderer.materials[1].color.a);
    }
}
