using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileTypeAllocator : MonoBehaviour
{
    public static BaseTileType[,] BaseTypeMap;

    [HideInInspector]
    public static float waterHeightThreshhold;

    public static void AllocateBaseTypes(int size, float waterPercentage)
    {
        BaseTypeMap = new BaseTileType[size, size];
        DetermineWaterHeight(waterPercentage);
        SetBaseTypes(size);

        // DEBUGS
        //Debug.Log($"Water Height Threshold = {waterHeightThreshhold}");
        //Debug.Log(BaseTypeMap.Length);
        //foreach (BaseTileType b in BaseTypeMap) { Debug.Log(b); }
    }

    private static void DetermineWaterHeight(float waterPercentage)
    {
        float min = Min();
        float max = Max();
        float difference = max - min;
        waterHeightThreshhold = min + difference * waterPercentage;
    }

    private static void SetBaseTypes(int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (HeightmapGenerator.Heightmap[x,y] < waterHeightThreshhold)
                { BaseTypeMap[x, y] = BaseTileType.Water; } 
                else if (HeightmapGenerator.Heightmap[x,y] > waterHeightThreshhold)
                { BaseTypeMap[x, y] = BaseTileType.Land; }
                else { Debug.LogError("Base Type could not be set."); }
            }
        }
    }

    private static float Min()
    {
        float min = HeightmapGenerator.Heightmap[0,0];
        foreach (float hValue in HeightmapGenerator.Heightmap) 
            if (hValue < min)
                min = hValue;
            return min;
    }

    private static float Max()
    {
        float max = HeightmapGenerator.Heightmap[0, 0];
        foreach (float hValue in HeightmapGenerator.Heightmap)
            if (hValue > max)
                max = hValue;
        return max;
    }
}
