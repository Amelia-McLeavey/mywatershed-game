using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private List<Material> m_testMaterials;

    [SerializeField]
    private List<Material> m_baseMaterials;

    public Material ReturnTileType(PhysicalType physicalType) => m_testMaterials[(int)physicalType];
}
