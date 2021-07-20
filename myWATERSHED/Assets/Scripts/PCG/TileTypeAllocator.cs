using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileBaseType { Water, Land };
public enum WaterTileClass { None, Shallow, Medium, Deep };
public enum LandTileClass { Human, Nature };

public class TileTypeAllocator : MonoBehaviour
{
    public static TileBaseType[,] BaseTypeMap;
    public static WaterTileClass[,] WaterClassMap;
    public static LandTileClass[,] LandClassMap;

    private static float waterHeightThreshhold;
    private static float shallowWaterThreshold;
    private static float deepWaterThreshold;

    private static List<float> waterTileHeights = new List<float>();

    public static void AllocateTileBaseTypes(int size, float waterPercentage)
    {
        BaseTypeMap = new TileBaseType[size, size];
        DetermineWaterHeight(waterPercentage);
        SetBaseTypes(size);
    }

    public static void AllocateTileClasses(int size)
    {
        WaterClassMap = new WaterTileClass[size, size];
        DetermineWaterDepths();
        SetWaterClassType(size);
    }

    private static void DetermineWaterHeight(float waterPercentage)
    {
        List<float> heights = new List<float>();
        foreach (float hValue in HeightmapGenerator.Heightmap)
        { heights.Add(hValue); }

        float min = Min(HeightmapGenerator.Heightmap[0,0], heights); 
        float max = Max(HeightmapGenerator.Heightmap[0,0], heights);
        float difference = max - min;
        waterHeightThreshhold = min + difference * waterPercentage;
    }

    private static void DetermineWaterDepths()
    {
        float min = Min(waterTileHeights[0], waterTileHeights);
        float max = Max(waterTileHeights[0], waterTileHeights);
        float difference = max - min;
        shallowWaterThreshold = max - (difference / 3.0f); // Hardcoded for first pass test
        deepWaterThreshold = min + (difference / 3.0f);

        Debug.Log($"Min = {min}");
        Debug.Log($"Max = {max}");
        Debug.Log($"Difference {difference}");
        Debug.Log($"Shallow, Deep = {new Vector2(shallowWaterThreshold, deepWaterThreshold)}");
    }

    private static void SetBaseTypes(int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (HeightmapGenerator.Heightmap[x,y] < waterHeightThreshhold)
                { 
                    BaseTypeMap[x, y] = TileBaseType.Water;
                    waterTileHeights.Add(HeightmapGenerator.Heightmap[x, y]);
                } 
                else if (HeightmapGenerator.Heightmap[x,y] > waterHeightThreshhold)
                { 
                    BaseTypeMap[x, y] = TileBaseType.Land; 
                }
                else { Debug.LogError("Base Type could not be set."); }
            }
        }
    }

    private static void SetWaterClassType(int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (BaseTypeMap[x,y] == TileBaseType.Water)
                {
                    if (HeightmapGenerator.Heightmap[x,y] <= deepWaterThreshold)
                    {
                        WaterClassMap[x, y] = WaterTileClass.Deep;
                    }
                    else if (HeightmapGenerator.Heightmap[x,y] >= shallowWaterThreshold)
                    {
                        WaterClassMap[x, y] = WaterTileClass.Shallow;
                    }
                    else
                    {
                        WaterClassMap[x, y] = WaterTileClass.Medium;
                    }
                }
                else
                {
                    WaterClassMap[x, y] = WaterTileClass.None;
                }
            }
        }
    }

    private static void SetLandClassType()
    {

    }

    private static float Min(float firstValue, List<float> list)
    {
        float min = firstValue;
        foreach (float value in list) 
            if (value < min)
                min = value;
            return min;
    }

    private static float Max(float firstValue, List<float> list)
    {
        float max = firstValue;
        foreach (float value in list)
            if (value > max)
                max = value;
        return max;
    }
}
