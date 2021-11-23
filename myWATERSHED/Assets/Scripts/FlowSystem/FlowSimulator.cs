using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowSimulator : MonoBehaviour
{
    private int m_rows;
    private int m_columns;

    private void OnEnable()
    {
        SystemGenerator.OnSystemGenerationComplete += InitializeFlow;
    }

    private void OnDisable()
    {
        SystemGenerator.OnSystemGenerationComplete -= InitializeFlow;
    }

    public void UpdateFlow()
    {
        SendLandFlowPulse();
        SendWaterFlowPulse();
    }

    private void SendLandFlowPulse()
    {
        //Debug.Log("SEND LAND FLOW");

        FlowStyle flowStyle = new LandFlowStyle();

        for (int x = m_rows; x > 0; x--)
        {
            for (int y = m_columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == BaseType.Land)
                    {
                        FlowPulseGather(value, flowStyle, tileIndex);
                    }         
                }
            }
        }
    }

    private void SendWaterFlowPulse()
    {
        //Debug.Log("SEND WATER FLOW");

        FlowStyle flowStyle = new WaterFlowStyle();

        SendTwoStageFlow(flowStyle, BaseType.Water);
       
        // RESET THE LIST THEN SCATTER AND ADD TO LIST
        // TAKES THE AVERAGE AND UPDATE OWN VALUE

    }

    private void SendTwoStageFlow(FlowStyle flowStyle, BaseType baseType)
    {
        for (int x = m_rows; x > 0; x--)
        {
            for (int y = m_columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == baseType)
                    {
                        FlowPulseScatter(value, flowStyle, tileIndex); // scatter
                    }
                }
            }
        }
        for (int x = m_rows; x > 0; x--)
        {
            for (int y = m_columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == baseType)
                    {
                        FlowPulseGather(value, flowStyle, tileIndex);
                    }
                }
            }
        }
    }

    private void FlowPulseScatter(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
    {
        List<GameObject> receiverTiles = GetRequiredNeighbours(senderTile);

        foreach (GameObject receiverTile in receiverTiles)
        {
            if (flowStyle.CanFlow(senderTile, receiverTile, indexForDebugging))
            {
                flowStyle.ScatterFlow(senderTile, receiverTile, indexForDebugging);
            }
            else
            {

            }
        }
    }

    private void FlowPulseGather(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
    {
        List<GameObject> receiverTiles = GetRequiredNeighbours(senderTile);

        foreach (GameObject receiverTile in receiverTiles)
        {
            if (flowStyle.CanFlow(senderTile, receiverTile, indexForDebugging))
            {
                flowStyle.GatherFlow(senderTile, indexForDebugging);
            }
            else
            {

            }
        }
    }


    private List<GameObject> GetRequiredNeighbours (GameObject senderTile)
    {
        return senderTile.GetComponent<Tile>().m_receiverNeighbours;
    }
     
    // Initializes tiles by finding the nieghbours of each tile that will receive information
    private void InitializeFlow(int rows, int columns)
    {
        //Debug.Log("ITIALIZE FLOW");

        m_rows = rows;
        m_columns = columns;

        for (int x = 0; x < m_rows; x++)
        { 
            for (int y = 0; y < m_columns; y++)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value)) 
                {
                    Tile tileScript = value.GetComponent<Tile>();
                    List<Vector2> neighbourIndices = NeighbourUtility.FindAllNeighbours(tileIndex);

                    if (tileScript.m_Basetype == BaseType.Water)
                    {
                        tileScript.m_receiverNeighbours = NeighbourUtility.FindLowestNeighbours(neighbourIndices);
                    }

                    if (tileScript.m_Basetype == BaseType.Land)
                    {
                        tileScript.m_receiverNeighbours = NeighbourUtility.FindLowestNeighbour(neighbourIndices, value);
                    }
                }
            }
        }
    }
}
