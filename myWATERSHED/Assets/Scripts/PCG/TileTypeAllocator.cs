using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileBaseType { Water, Land };
public enum WaterTileClass { None, Shallow, Medium, Deep };
public enum LandTileClass { None, Nature, Human };

public class TileTypeAllocator : MonoBehaviour
{
    public static TileBaseType[,] BaseTypeMap;
    public static WaterTileClass[,] WaterClassMap;
    public static LandTileClass[,] LandClassMap;

    private static float waterHeightThreshhold;
    private static float shallowWaterThreshold;
    private static float deepWaterThreshold;
    private static float landDivideLevel;

    private static List<Vector2> creekTileStartingPositions = new List<Vector2>();

    public static void AllocateTileTypes(int size, int numberOfCreeks, float totalWaterPercent, float shorelinePercent, float deepWaterPercent)
    {
        // Initialize maps
        BaseTypeMap = new TileBaseType[size, size];
        WaterClassMap = new WaterTileClass[size, size];
        LandClassMap = new LandTileClass[size, size];

        DetermineHeightLevels(totalWaterPercent, shorelinePercent, deepWaterPercent);

        SetBaseTypes(size);
        SetWaterClassType(size);
        //CreateCreeks(size, numberOfCreeks);

        creekTileStartingPositions.Clear();

    }

    private static void DetermineHeightLevels(float waterPercentage, float shorelinePercent, float deepWaterPercent)
    {
        List<float> heights = new List<float>();
        foreach (float hValue in HeightmapGenerator.Heightmap)
        { heights.Add(hValue); }

        // TOTAL //
        float totalMin = Min(HeightmapGenerator.Heightmap[0, 0], heights);
        float totalMax = Max(HeightmapGenerator.Heightmap[0, 0], heights);
        float totalDifference = totalMax - totalMin;
        waterHeightThreshhold = totalMin + totalDifference * waterPercentage;

        // LAND //
        float landMin = waterHeightThreshhold;
        float landMax = totalMax;
        float landDifference = landMax - landMin;
        landDivideLevel = landMin + (landDifference / 2.0f);

        // WATER //
        float waterMin = totalMin;
        float waterMax = waterHeightThreshhold;
        float waterDifference = waterMax - waterMin;
        shallowWaterThreshold = waterMax - (waterDifference * shorelinePercent);
        deepWaterThreshold = waterMin + (waterDifference * deepWaterPercent);
    }

    private static void SetBaseTypes(int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (HeightmapGenerator.Heightmap[x, y] < waterHeightThreshhold)
                {
                    BaseTypeMap[x, y] = TileBaseType.Water;
                }
                else if (HeightmapGenerator.Heightmap[x, y] > waterHeightThreshhold)
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
                if (BaseTypeMap[x, y] == TileBaseType.Water)
                {
                    if (HeightmapGenerator.Heightmap[x, y] <= deepWaterThreshold)
                    {
                        WaterClassMap[x, y] = WaterTileClass.Deep;
                    }
                    else if (HeightmapGenerator.Heightmap[x, y] >= shallowWaterThreshold)
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

    //private static void SetLandClassType(int size)
    //{
    //    for (int x = 0; x < size; x++)
    //    {
    //        for (int y = 0; y < size; y++)
    //        {
    //            if (BaseTypeMap[x, y] == TileBaseType.Land)
    //            {
    //                LandClassMap[x,y] = 
    //            }
    //        }
    //    }
    //}

    private static void CreateCreeks(int size, int numberOfCreeks)
    {
        while (creekTileStartingPositions.Count < numberOfCreeks)
        {
            Vector2 creekStartPosition = new Vector2(Random.Range(0, size - 1), Random.Range(0, size - 1));
            if (BaseTypeMap[(int)creekStartPosition.x, (int)creekStartPosition.y] == TileBaseType.Land)
            {
                creekTileStartingPositions.Add(creekStartPosition);
                BaseTypeMap[(int)creekStartPosition.x, (int)creekStartPosition.y] = TileBaseType.Water;
                WaterClassMap[(int)creekStartPosition.x, (int)creekStartPosition.y] = WaterTileClass.Medium;
                Debug.Log(creekStartPosition);
            }
        }

        //"PATHFINDING" (NOT ACTUALLY)
        foreach (Vector2 creekStartPosition in creekTileStartingPositions)
        {

            int xPos = (int)creekStartPosition.x;
            while (BaseTypeMap[xPos + 1, (int)creekStartPosition.y] == TileBaseType.Land || WaterClassMap[xPos + 1, (int)creekStartPosition.y] == WaterTileClass.Shallow)
            {
                BaseTypeMap[xPos + 1, (int)creekStartPosition.y] = TileBaseType.Water;
                WaterClassMap[xPos + 1, (int)creekStartPosition.y] = WaterTileClass.Medium;

                xPos++;
            }
        }

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
