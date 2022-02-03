using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapTab : MonoBehaviour
{
    [SerializeField] private Heatmap heatmap;
    [SerializeField] private Heatmap.varOptions varToSelect;

    public void Clicked()
    {
        heatmap.ChangeHeatmap(varToSelect);
    }
}
