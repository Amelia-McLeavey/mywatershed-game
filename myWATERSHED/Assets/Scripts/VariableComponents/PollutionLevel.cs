using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionLevel : MonoBehaviour
{
    public float m_PolutionLevel;

    [HideInInspector]
    public List<float> m_GatheredPolutionValues = new List<float>();
}
