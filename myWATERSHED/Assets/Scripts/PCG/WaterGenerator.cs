using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    public static float[,] StreamMap;

    [Header("RIVER SETTINGS")]
    [Range(1, 10)]
    [SerializeField]
    private int m_rMinWidth = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_rMaxWidth = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_rTexture = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_rCurvature = 1;

    [Header("BRANCH SETTINGS")]
    [Range(0, 3)]
    [SerializeField]
    private int m_branchCount = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_bMinWidth = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_bMaxWidth = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_bTexture = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_bCurvature = 1;

    [Header("CREEK SETTINGS")]
    [Range(0, 2)]
    [SerializeField]
    private int m_creekCount = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_cMinWidth = 1;
    [Range(1, 10)]
    [SerializeField]
    private int m_cMaxWidth = 1;


    private int m_size;
    private readonly int m_left = -1;
    private readonly int m_right = 1;

    private readonly Queue<int> curveChangeValues = new Queue<int>();
    private readonly Queue<int> textureChangeValues = new Queue<int>();

    private readonly List<Vector2> branchStartPositionsL = new List<Vector2>();
    private readonly List<Vector2> branchStartPosistionsR = new List<Vector2>();
    private readonly List<Vector2> creekStartPositionsL = new List<Vector2>();
    private readonly List<Vector2> creekStartPositionsR = new List<Vector2>();
    private readonly List<Vector2> creekStartPositionsRL = new List<Vector2>();

    public void CreateWater(int size, int seed)
    {
        // Initialization
        Random.InitState(seed);
        m_size = size;
        StreamMap = new float[m_size, m_size];

        ResetData();

        // Generation
        CreateRiver();
     
        for (int i = 0; i <= m_branchCount; i++)
        {
            if (i != 0)
            {
                if (i % 2 == 0)
                    CreateBranch(branchStartPositionsL, m_left, 2, 1);
                else
                    CreateBranch(branchStartPosistionsR, m_right, 1, 2);
            }     
        }

        for (int i = 0; i <= m_creekCount; i++)
        {
            switch (i)
            {
                case 0: break;
                case 1: CreateCreek(creekStartPositionsR, m_right); break;
                case 2: CreateCreek(creekStartPositionsR, m_right); break;
                //case 3: CreateCreek(creekStartPositionsL, m_left); break;
                //case 4: CreateCreek(creekStartPositionsR, m_right); break;
                //case 5: CreateCreek(creekStartPositionsL, m_left); break;
            }
        }
    }

    private void ResetData()
    {
        curveChangeValues.Clear();
        textureChangeValues.Clear();

        branchStartPositionsL.Clear();
        branchStartPosistionsR.Clear();
        creekStartPositionsL.Clear();
        creekStartPositionsR.Clear();
        creekStartPositionsRL.Clear();
}

    private void CreateRiver()
    {
        // Initialization
        int yStart = 0;
        int minChange = 1;
        int maxChange = m_rMaxWidth - 3;
        int minWidth = m_rMinWidth;
        int maxWidth = m_rMaxWidth;
        int streamWidth = m_rMaxWidth;

        // DRAW X DIMENSION
        for (int x = 0; x < m_size; x++)
        {
            // ADJUST WIDTHS [this is all hardcoded]
            if (x > m_size * 0.90f)
            { minWidth = m_rMinWidth + 9; maxWidth = m_rMaxWidth + 9; maxChange = streamWidth - 6; }
            else if (x > m_size * 0.75f)
            { minWidth = m_rMinWidth+ 6; maxWidth = m_rMaxWidth + 6; maxChange = streamWidth - 5; }
            else if (x > m_size * 0.50f)
            { minWidth = m_rMinWidth + 3; maxWidth = m_rMaxWidth + 3; maxChange = streamWidth - 4; }
            else if (x > m_size * 0.30f)
            { minWidth = m_rMinWidth + 1; maxWidth = m_rMaxWidth + 1; maxChange = streamWidth - 3; }

            // DEFINE CURVATURE
            CheckQueue(curveChangeValues);
            int curveChanceValue = curveChangeValues.Dequeue();
            if (curveChanceValue < m_rCurvature)
            {
                yStart += Random.Range(minChange, maxChange);
            }
            else
            {
                yStart = Random.Range(yStart - 1, yStart + 2);
            }

            // DEFINE TEXTURE
            CheckQueue(textureChangeValues);
            int textureChangeValue = textureChangeValues.Dequeue();
            if (textureChangeValue < m_rTexture)
            {
                streamWidth = Random.Range(minWidth, maxWidth + 1);
            }

            // DRAW Y DIMENSION
            for (int y = 0; y < m_size; y++)
            {
                if (y >= yStart && y <= yStart + (streamWidth - 1))
                {
                    StreamMap[x, y] = 5f; // This is where the height is assigned

                    if (x < m_size * 0.90f && x > m_size * 0.60f)
                    {
                        if (y == yStart)
                        {
                            branchStartPositionsL.Add(new Vector2(x, y));
                        }
                    }
                    if (x < m_size * 0.60f && x > m_size * 0.30f)
                    {
                        if (y == yStart + (streamWidth - 1))
                        {
                            branchStartPosistionsR.Add(new Vector2(x, y));
                        }
                    }
                    if (x < m_size * 0.80f && x > m_size * 0.60f)
                    {
                        if (y == yStart + (streamWidth - 1))
                        {
                            creekStartPositionsR.Add(new Vector2(x, y));
                        }
                    }
                }
            }
        }
        curveChangeValues.Clear();
        textureChangeValues.Clear();
    }

    private void CreateBranch(List<Vector2> startPositions, int direction, int minOffset, int maxOffset)
    {

        int randomIndex = Random.Range(0, startPositions.Count);
        int xStart = (int)startPositions[randomIndex].x + 1;
        int yStart = (int)startPositions[randomIndex].y - 2;
        int minChange = 1;
        int maxChange = m_bMaxWidth - 1;
        int minWidth = m_bMinWidth;
        int maxWidth = m_bMaxWidth;
        int streamWidth = m_bMaxWidth;

        // DRAW X DIMENSION
        for (int x = xStart; x > -1; x--)
        {
            // ADJUST WIDTHS [this is all hardcoded]
            if (x < (m_size - xStart) * 0.90f)
            { minWidth = m_bMinWidth; maxWidth = m_bMinWidth; maxChange = streamWidth - 1; }
            else if (x < (m_size - xStart) * 0.60f)
            { minWidth = m_bMinWidth + 1; maxWidth = m_bMaxWidth - 1; maxChange = streamWidth - 2; }
            else if (x < (m_size - xStart) * 0.35f)
            { minWidth = m_bMinWidth + 2; maxWidth = m_bMaxWidth; maxChange = streamWidth - 3; }

            // DEFINE CURVATURE
            CheckQueue(curveChangeValues);
            int curveChanceValue = curveChangeValues.Dequeue();
            if (curveChanceValue < m_bCurvature)
            {
                yStart += Random.Range(minChange * direction, maxChange * direction);
            }
            else
            {
                yStart = Random.Range(yStart - minOffset, yStart + maxOffset);
            }

            // DEFINE TEXTURE
            CheckQueue(textureChangeValues);
            int textureChangeValue = textureChangeValues.Dequeue();
            if (textureChangeValue < m_bTexture)
            {
                streamWidth = Random.Range(minWidth, maxWidth + 1);
            }

            // DRAW Y DIMENSION
            for (int y = 0; y < m_size; y++)
            {
                if (y >= yStart && y <= yStart + (streamWidth - 1))
                {
                    StreamMap[x, y] = 3f; // This is where the height is assigned

                    if (direction == m_right)
                    {
                        if (x < m_size * 0.50f && x > m_size * 0.20f)
                        {
                            if (y == yStart)
                            {
                                creekStartPositionsRL.Add(new Vector2(x, y));
                            }
                            if (y == yStart + (streamWidth - 1))
                            {
                                creekStartPositionsR.Add(new Vector2(x, y));
                            }
                        }

                        else if (direction == m_left)
                        {
                            if (y == yStart)
                            {
                                creekStartPositionsL.Add(new Vector2(x, y));
                            }
                        }
                    }
                }
            }
        }
        curveChangeValues.Clear();
        textureChangeValues.Clear();
    }

    private void CreateCreek(List<Vector2> startPositions, int direction)
    {
        int randomIndex = Random.Range(0, startPositions.Count);
        int xStart = (int)startPositions[randomIndex].x + 1;
        int yStart = (int)startPositions[randomIndex].y - 1;
        int streamWidth = m_cMaxWidth;

        for (int y = yStart; y < m_size; y++)
        {
            if (y >= yStart && y <= yStart + (streamWidth - 1))
            {
                StreamMap[xStart, y] = 1f;
            }
        }

        // DRAW X DIRECTION
        for (int x = xStart + 1; x > -1; x--)
        {
            yStart += (streamWidth - 1 * direction);

            if (x % 2 == 0)
            { streamWidth = m_cMinWidth; }
            else
            { streamWidth = m_cMaxWidth; }

            if (direction == m_right)
            {
                // DRAW Y DIMENSION
                for (int y = yStart; y < m_size; y++)
                {
                    if (y >= yStart && y <= yStart + (streamWidth - 1))
                    {
                        StreamMap[x, y] = 1f;
                    }
                }
            }
            //if (direction == m_left)
            //{
            //    // DRAW Y DIMENSION
            //    for (int y = yStart; y > -1; y--)
            //    {
            //        if (y >= yStart && y <= yStart + (streamWidth - 1))
            //        {
            //            StreamMap[x, y] = 1f;
            //        }
            //    }
            //}
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
