using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This script currently handles tile interaction.

public class Tile : MonoBehaviour
{
    public BaseType m_Basetype;
    public PhysicalType m_PhysicalType;

    public Vector2 m_TileIndex;

    [HideInInspector]
    public List<GameObject> m_receiverNeighbours = new List<GameObject>();

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
    }


    /// <summary>
    /// Changes the colour of the clicked tile's edge. Can be used for showing selection.
    /// </summary>
    public void DirectEffect()
    {
        m_meshRenderer.materials[1].color = Color.red;
    }
}
