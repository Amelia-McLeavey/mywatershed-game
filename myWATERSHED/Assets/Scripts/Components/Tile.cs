using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This script currently handles tile information.

public class Tile : MonoBehaviour
{
    public bool m_IsSpawner;

    public BaseType m_Basetype;
    public PhysicalType m_PhysicalType;

    public Vector2 m_TileIndex;

    // INFORMATION VARIABLES
    [SerializeField]
    public float newValue;
    [SerializeField]
    public float saturation;

    private List<float> m_infoItems = new List<float>();

    // NEIGHBOUR REFERENCES
    private List<GameObject> m_senderNeighbours = new List<GameObject>();
    private List<GameObject> m_recieverNeighbours = new List<GameObject>();

    // COMPONENT REFERENCES
    private MeshRenderer m_meshRenderer;

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Set the colour of a tile's material based on given type.
    /// </summary>
    /// <param name="colour"></param>
    public void SetTypeColor(Color colour)
    {
        m_meshRenderer.materials[0].color = colour;
        m_meshRenderer.materials[1].color = new Color(colour.r - 0.2f, colour.g - 0.2f, colour.b - 0.2f);
        //m_meshRenderer.materials[1].color = Color;
    }

    public void FindWaterNeighbours()
    {
        // Find neighbours using this tile's index
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

        // Add neighbours to the appropriate reference list
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
        // Find neighbours using this tile's index
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

    /// <summary>
    /// Designates the tile as a spawner tile.
    /// </summary>
    /// <param name="value"></param>
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

    /// <summary>
    /// Changes the colour of the clicked tile's edge. Can be used for showing selection.
    /// </summary>
    public void DirectEffect()
    {
        m_meshRenderer.materials[1].color = Color.red;
    }

    // Upon call in FlowSimulator script, cycles through each reciever and calls it to recieve information if not a spawner
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
        // For each call to this method, add the passed info to a list of info items
        m_infoItems.Add(info);

        // Refresh the variables when the list of info items is equal to the expected number of senders
        if (m_infoItems.Count == m_senderNeighbours.Count)
        {
            RefreshVariables();
        }
        
    }

    private void RefreshVariables()
    {
        // Calculate the sum of all the info
        float infoSum = m_infoItems.Sum();
        float finalValue;

        // Calculate the final value
        if (m_infoItems.Count > 1)
        { finalValue = infoSum / m_infoItems.Count; }
        else { finalValue = infoSum; }

        m_infoItems.Clear();

        // Refresh variables
        saturation = finalValue;
        saturation = Mathf.Clamp(saturation, 0f, 100f);
        if (saturation < 1f) { saturation = 0f; }
        newValue = Mathf.Clamp(saturation * 2.55f, 0f, 255f);

        // Apply visual changes
        m_meshRenderer.materials[1].color = new Color(newValue / 255f, 0f, 0f, m_meshRenderer.materials[1].color.a);
    }
}
