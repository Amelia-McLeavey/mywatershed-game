using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    public static Dictionary<Vector2, BaseType> s_WaterTiles = new Dictionary<Vector2, BaseType>();

    [Header("RIVER SETTINGS")]
    [Range(3, 50)]
    [SerializeField]
    private int m_startingYPosition = 30;
    [Range(1, 5)]
    [SerializeField]
    private int m_rMinWidth = 4;
    [Range(1, 5)] 
    [SerializeField]
    private int m_rMaxWidth = 4;
    [Range(1, 10)]
    [SerializeField]
    private int m_rTexture = 9;

    [Header("BRANCH SETTINGS")]
    [Range(0, 6)]
    [SerializeField]
    private int m_branchCount = 3;
    [Range(1, 10)]
    [SerializeField]
    private int m_bMinWidth = 2;
    [Range(1, 10)]
    [SerializeField]
    private int m_bMaxWidth = 2;
    [Range(1, 10)]
    [SerializeField]
    private int m_bTexture = 5;

    private int m_rows;
    private int m_columns;

    private readonly Queue<int> m_widthChangeValues = new Queue<int>();

    private readonly List<Vector2> m_branchStartPositionsL = new List<Vector2>();
    private readonly List<Vector2> m_branchStartPosistionsR = new List<Vector2>();

    public void CreateWater(int rows, int columns, int seed)
    {
        // INITIALIZATION
        Random.InitState(seed);
        m_rows = rows;
        m_columns = columns;

        // RESET DATA
        m_widthChangeValues.Clear();
        m_branchStartPositionsL.Clear();
        m_branchStartPosistionsR.Clear();

        // GENERATION
        CreateRiver();
        // Create Branches
        for (int i = 0; i <= m_branchCount; i++)
        {
            if (i != 0)
            {
                // If the branch number is even, pass appropriate modifier: 0
                if (i % 2 == 0)
                    CreateBranch(m_branchStartPositionsL, 0, 0);
                else // if the branch number is odd, pass appropriate modifier: 1
                    CreateBranch(m_branchStartPosistionsR, 1, 1);
            }
        }
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
        for (int x = 0; x < m_rows; x++)
        {
            // DEFINE LINE
            streamWidth = DefineWidth(streamWidth, minWidth, maxWidth, m_rTexture);
            yStart = DefineCurvature(lastYStart, streamWidth, lastStreamWidth, minModifier, maxModifier, -1, 0);

            // CACHE VALUES
            lastStreamWidth = streamWidth;
            lastYStart = yStart;
           
            // DRAW Y DIMENSION
            for (int y = 0; y < m_columns; y++)
            {
                if (y >= yStart && y <= yStart + (streamWidth - 1))
                {
                    // Add to list of water tiles with appropriate type and remove from generic list
                    s_WaterTiles.Add(new Vector2(x, y), BaseType.Water);
                    WorldGenerator.s_UndefinedTiles.Remove(new Vector2(x, y));

                    // Add tiles to list of potential branch start positions 
                    if (x > m_rows * 0.30f && x < m_rows * 0.90f)
                    {
                        if (y == yStart)
                        {
                            m_branchStartPositionsL.Add(new Vector2(x, y));
                        }
                        if (y == yStart + (streamWidth - 1))
                        {
                            m_branchStartPosistionsR.Add(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        m_widthChangeValues.Clear();
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
            streamWidth = DefineWidth(streamWidth, minWidth, maxWidth, m_bTexture);
            yStart = DefineCurvature(lastYStart,streamWidth, lastStreamWidth, minModifier, maxModifier, 0, -1);

            // CACHE VALUES
            lastYStart = yStart;
            lastStreamWidth = streamWidth; 

            // DRAW Y DIMENSION
            for (int y = 0; y < m_columns; y++)
            {
                if (y >= yStart && y <= yStart + (streamWidth - 1))
                {
                    // Add to list of water tiles with appropriate type and remove from generic list
                    if (s_WaterTiles.ContainsKey(new Vector2(x,y))) { break; }
                    s_WaterTiles.Add(new Vector2(x, y), BaseType.Water);
                    WorldGenerator.s_UndefinedTiles.Remove(new Vector2(x, y));
                }
            }
        }
        m_widthChangeValues.Clear();
    }

    /// <summary>
    /// Sets the desired stream width with some degree of randomness.
    /// </summary>
    /// <param name="streamWidth"></param>
    /// <param name="minWidth"></param>
    /// <param name="maxWidth"></param>
    /// <param name="texture"></param>
    /// <returns></returns>
    private int DefineWidth(int streamWidth, int minWidth, int maxWidth, int texture)
    {
        CheckQueue(m_widthChangeValues);
        int textureChangeValue = m_widthChangeValues.Dequeue();
        if (textureChangeValue < texture)
        { 
            return Random.Range(minWidth, maxWidth + 1); 
        }
        else 
        { 
            return streamWidth; 
        }
    }

    /// <summary>
    /// Changes the starting Y index for the row. 
    /// </summary>
    /// <param name="lastYStart"></param>
    /// <param name="streamWidth"></param>
    /// <param name="lastStreamWidth"></param>
    /// <param name="minModifier"></param>
    /// <param name="maxModifier"></param>
    /// <param name="evenAdjustment"></param>
    /// <param name="oddAdjustment"></param>
    /// <returns></returns>
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

    // BELOW METHODS USED FOR SHUFFLING LISTS | RANDOMIZATION //

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
