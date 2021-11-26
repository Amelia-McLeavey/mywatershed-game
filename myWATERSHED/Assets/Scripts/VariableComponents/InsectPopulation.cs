using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectPopulation : MonoBehaviour
{
    public int m_InsectPopulation;

    [HideInInspector]
    public List<int> m_GatheredInsectPopulationValues = new List<int>();
}
