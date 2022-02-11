using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides different custom math functions specific to the myWatershed project.
/// </summary>

public abstract class MathUtility
{
    public static float FloatArrayAverage(float[] values, int count)
    {
        float average; 
        if (count == 0)
        {
            return 0;
        }

        float totalValue = 0;

        for (int i = 0; i < count; i++)
        {
            totalValue += values[i];
        }
        average = totalValue / count;

        return average;
    }
}
