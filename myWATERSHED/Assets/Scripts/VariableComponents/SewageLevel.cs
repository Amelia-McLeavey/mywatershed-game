using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewageLevel : MonoBehaviour
{
    public float m_SewageLevel;

    [HideInInspector]
    public List<float> m_GatheredSewageValues = new List<float>();
}
