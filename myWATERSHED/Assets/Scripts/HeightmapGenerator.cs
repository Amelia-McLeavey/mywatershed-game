using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightmapGenerator : MonoBehaviour
{
    public static float[,] Heightmap;

    public static int N;
    private static int s_edgeLength;

    private static float s_randomnessMagnitude;
    private static float s_magnitudeReductionRate;

    public static float[,] CreateHeightmap(int n, int seed, float randomnessMagnitude, float magnitudeReductionRate)
    {
        N = n;
        s_randomnessMagnitude = randomnessMagnitude;
        s_magnitudeReductionRate = magnitudeReductionRate;

        Random.InitState(seed);
        s_edgeLength = CalculateLength(n);
        Heightmap = new float[s_edgeLength, s_edgeLength];

        ClearHeightmap();
        SetRandomCornerSeeds();
        MidpointDisplacement();

        foreach (float hValue in Heightmap) { Debug.Log(hValue); }

        return Heightmap;
    }

    private static int CalculateLength(int n)
    {
        return (int)Mathf.Pow(2, n) + 1;
    }

    // Sets all the values in the Heightmap to zero
    private static void ClearHeightmap()
    {
        for (int x = 0; x < s_edgeLength; x++)
        {
            for (int y = 0; y < s_edgeLength; y++)
            {
                Heightmap[x, y] = 0.0f;
            }
        }
    }

    private static void SetRandomCornerSeeds()
    {
        Heightmap[0, 0] = GetRandom();
        Heightmap[0, s_edgeLength - 1] = GetRandom();
        Heightmap[s_edgeLength - 1, 0] = GetRandom();
        Heightmap[s_edgeLength - 1, s_edgeLength - 1] = GetRandom();
    }

    private static float GetRandom()
    {
        return Random.Range(0f, 14f);
    }

    private static void MidpointDisplacement()
    {
        for (int i = 0; i < N; i++)
        {
            int numberOfQuads = (int)Mathf.Pow(4, i); // Begins at 1 because 4^0 = 1.
            int quadsPerRow = (int)Mathf.Sqrt(numberOfQuads);
            int quadLength = (s_edgeLength - 1) / quadsPerRow;

            for (int x = 0; x < quadsPerRow; x++)
            {
                for (int y = 0; y < quadsPerRow; y++)
                {
                    // Pass the indexes of each quad
                    CalculateMidpointValues(quadLength * x, quadLength * (x + 1), quadLength * y, quadLength * (y + 1));
                }
            }
            s_randomnessMagnitude *= s_magnitudeReductionRate;
        }
    }

    private static void CalculateMidpointValues(int x0, int x1, int y0, int y1)
    {
        // Calculate index of midpoint (m)
        int mx = GetMidpoint(x0, x1);
        int my = GetMidpoint(y0, y1);

        // Assign values to midpoints
        float top = Heightmap[mx, y1] = GetAvg2(Heightmap[x0, y1], Heightmap[x1, y1]) + GetOffset();
        float bottom = Heightmap[mx, y0] = GetAvg2(Heightmap[x0, y0], Heightmap[x1, y0]) + GetOffset();
        float left = Heightmap[x0, my] = GetAvg2(Heightmap[x0, y0], Heightmap[x0, y1]) + GetOffset();
        float right = Heightmap[x1, my] = GetAvg2(Heightmap[x1, y0], Heightmap[x1, y1]) + GetOffset();
        Heightmap[mx, my] = GetAvg4(top, bottom, left, right) + GetOffset();
    }

    private static void NormalizeHeighmap()
    {
        float min = 0f;
        float max = 14f;
    }

    private static int GetMidpoint(int a, int b)
    {
        return a + ((b - a) / 2);
    }

    private static float GetOffset()
    {
        return GetRandom() * s_randomnessMagnitude;
    }

    private static float GetAvg2(float a, float b)
    {
        return (a + b) / 2.0f;
    }

    private static float GetAvg4(float a, float b, float c, float d)
    {
        return (a + b + c + d) / 4.0f;
    }
}
