using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    public static float[,] StreamHeightmap;

    [SerializeField]
    private float m_maxWaterTileHeight;

    [Header("RIVER SETTINGS")]
    [Range(3, 50)]
    [SerializeField]
    private int m_startingYPosition = 3;
    [Range(1, 5)]
    [SerializeField]
    private int m_rMinWidth = 4;
    [Range(1, 5)]
    [SerializeField]
    private int m_rMaxWidth = 4;
    [Range(1, 10)]
    [SerializeField]
    private int m_rTexture = 1;

    [Header("BRANCH SETTINGS")]
    [Range(0, 3)]
    [SerializeField]
    private int m_branchCount;
    [Range(1, 10)]
    [SerializeField]
    private int m_bMinWidth = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_bMaxWidth = 2;
    [Range(1, 10)]
    [SerializeField]
    private int m_bTexture = 1;

    private int m_size;
    private readonly Queue<int> widthChangeValues = new Queue<int>();

    private readonly List<Vector2> branchStartPositionsL = new List<Vector2>();
    private readonly List<Vector2> branchStartPosistionsR = new List<Vector2>();

    public void CreateWater(int size, int seed)
    {
        // INITIALIZATION
        Random.InitState(seed);
        m_size = size;
        StreamHeightmap = new float[m_size, m_size];

        // RESET DATA
        widthChangeValues.Clear();
        branchStartPositionsL.Clear();
        branchStartPosistionsR.Clear();

        // GENERATION
        CreateRiver();
        // Create Branches
        for (int i = 0; i <= m_branchCount; i++)
        {
            if (i != 0)
            {
                if (i % 2 == 0)
                    CreateBranch(branchStartPositionsL, 0, 0);
                else
                    CreateBranch(branchStartPosistionsR, 1, 1);
            }
        }

        SetHeights();
    }

    private void CreateRiver()
    {
        // INITIALIZATION
        // Index Variables
        int yStart;
        int lastYStart = m_startingYPosition;
        int minModifier = 2;
        int maxModifier = -1;
        // Width Variables
        int minWidth = m_rMinWidth;
        int maxWidth = m_rMaxWidth;
        int lastStreamWidth = 4;
        int streamWidth = lastStreamWidth;

        // DRAW X DIMENSION
        for (int x = 0; x < m_size; x++)
        {
            // DEFINE LINE
            streamWidth = DefineWidth(streamWidth, minWidth, maxWidth);
            yStart = DefineCurvature(lastYStart, streamWidth, lastStreamWidth, minModifier, maxModifier, -1, 0);

            // CACHE VALUES
            lastStreamWidth = streamWidth;
            lastYStart = yStart;
           
            // DRAW Y DIMENSION
            for (int y = 0; y < m_size; y++)
            {
                if (y >= yStart && y <= yStart + (streamWidth - 1))
                {
                    StreamHeightmap[x, y] = 1f;

                    if (x > m_size * 0.30f && x < m_size * 0.90f)
                    {
                        if (y == yStart)
                        {
                            branchStartPositionsL.Add(new Vector2(x, y));
                        }
                        if (y == yStart + (streamWidth - 1))
                        {
                            branchStartPosistionsR.Add(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        widthChangeValues.Clear();
    }

    private void CreateBranch(List<Vector2> startPositions, int minModifier, int maxModifier)
    {
        // INITIALIZATION
        // Index Variables
        int randomIndex = Random.Range(0, startPositions.Count);
        int xStart = (int)startPositions[randomIndex].x;
        int yStart = (int)startPositions[randomIndex].y;
        int lastYStart = yStart;
        // Width Variables
        int minWidth = m_bMinWidth;
        int maxWidth = m_bMaxWidth;
        int lastStreamWidth = 1;
        int streamWidth = lastStreamWidth;

        // DRAW X DIMENSION
        for (int x = xStart; x > -1; x--)
        {
            // DEFINE LINE
            streamWidth = DefineWidth(streamWidth, minWidth, maxWidth);
            yStart = DefineCurvature(lastYStart,streamWidth, lastStreamWidth, minModifier, maxModifier, 0, -1);

            // CACHE VALUES
            lastYStart = yStart;
            lastStreamWidth = streamWidth;

            // DRAW Y DIMENSION
            for (int y = 0; y < m_size; y++)
            {
                if (y >= yStart && y <= yStart + (streamWidth - 1))
                {
                    StreamHeightmap[x, y] = 1f;
                }
            }
        }
        widthChangeValues.Clear();
    }

    private void SetHeights()
    {
        for (int x = 0; x < m_size; x++)
        {
            float minDecriment = x * 0.30f;
            float maxDecriment = x * 0.35f;
            float minHeight = m_maxWaterTileHeight - maxDecriment;
            float maxHeight = m_maxWaterTileHeight - minDecriment;

            for (int y = 0; y < m_size; y++)
            {
                if (StreamHeightmap[x, y] != 0)
                {
                    StreamHeightmap[x, y] = Random.Range(minHeight, maxHeight);
                }
                else
                {
                    StreamHeightmap[x, y] = 1f;
                }
            }
        }
    }

    private int DefineWidth(int streamWidth, int minWidth, int maxWidth)
    {
        CheckQueue(widthChangeValues);
        int textureChangeValue = widthChangeValues.Dequeue();
        if (textureChangeValue < m_rTexture)
        { 
            return Random.Range(minWidth, maxWidth + 1); 
        }
        else 
        { 
            return streamWidth; 
        }
    }

    private int DefineCurvature(int lastYStart, int streamWidth, int lastStreamWidth, int minModifier, int maxModifier, int evenAdjustment, int oddAdjustment)
    {
        int minYStart;
        int maxYStart;

        // If the last y start is an even number
        if (lastYStart % 2 == 0)
        {
            minYStart = SetMinStart(lastYStart, streamWidth, evenAdjustment);
            maxYStart = SetMaxStart(lastYStart, lastStreamWidth, evenAdjustment);
        }
        else // If the last y start is an odd number
        {
            minYStart = SetMinStart(lastYStart, streamWidth, oddAdjustment);
            maxYStart = SetMaxStart(lastYStart, lastStreamWidth, oddAdjustment);
        }
        return Random.Range(minYStart + minModifier, maxYStart + maxModifier);
    }
    private int SetMinStart(int lastYStart, int streamWidth, int adjustment)
    {
        return lastYStart - (streamWidth + adjustment);
    }

    private int SetMaxStart(int lastYStart, int lastStreamWidth, int adjustment)
    {
        int evenAdjustment;
        int oddAdjustment;

        // [do not change hardcoded values] //
        if (adjustment == 0)
        { evenAdjustment = -1; oddAdjustment = 0; }
        else
        { evenAdjustment = 0; oddAdjustment = -1; }

        // If the last width is an even number
        if (lastStreamWidth % 2 == 0)
        {
            return lastYStart + (lastStreamWidth + evenAdjustment);
        }
        else // If the last width is an odd number
        {
            return lastYStart + (lastStreamWidth + oddAdjustment);
        }
    }

    private void CheckQueue(Queue<int> queue)
    {
        if (queue.Count == 0)
        {
            AddToQueue(queue);
        }
    }

    private void AddToQueue(Queue<int> queue)
    {
        List<int> values = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };  // DO NOT CHANGE VALUES

        values = Shuffle(values);

        foreach (var value in values)
        {
            queue.Enqueue(value);
        }
    }

    private static List<T> Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int k = Random.Range(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }
}
