using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides different custom math functions specific to the myWatershed project.
/// </summary>

public class MathUtility : MonoBehaviour
{
    public static float FloatArrayAverage(float[] values, int amount)
    {
        float totalValue = 0;
        for (int i = 0; i < amount; i++)
        {
            totalValue += values[i];
        }
        float average =  totalValue / amount;
        return average;
    }
}
